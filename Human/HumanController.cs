using UnityEngine;
using UnityEngine.AI;
using Mutanium.Utils;

namespace Mutanium.Human
{
    public class HumanController : MonoBehaviour
    {
        private const int LAZY_SKIP_FRAMES = 4;
        private const int MINUTES_IN_HUMAN_YEAR = 240;
        private Coroutine currentCoroutine;
        private HumanStateName currentStateName;
        private HumanState currentHumanState;

        public Relationship relationship;
        public Renderer meshRenderer;
        internal NavMeshAgent Agent { get; private set; }
        internal Animator Animator { get; private set; }
        internal HumanInfo Human { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();

            SetState(Human.NextState);

            LazyUpdate();
        }
        public void Update()
        {
            // Skip frames for better performance.
            if (Time.frameCount % LAZY_SKIP_FRAMES == 0)
                LazyUpdate();
        }

        private void LazyUpdate()
        {
            Human.Age = Human.BirthDate.MinutesFromNow() / MINUTES_IN_HUMAN_YEAR;


        }

        /// <summary>
        /// Sets the unit state.
        /// </summary>
        /// <param name="state">New state.</param>
        private void SetState(HumanStateName state)
        {
            Debug.Log($"Name={transform.name}, State={state} Age={Human.Age}");
            currentHumanState = HumanState.Create(this, state);
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(currentHumanState.UpdateState(OnStateEnded));
            currentStateName = state;
            Animator.SetInteger("state", (int)state);
        }



        /// <summary>
        /// Chooses random state among available states.
        /// </summary>
        private void OnStateEnded()
        {
            SetNextState();
        }

        private void SetNextState()
        {
            if (currentStateName != HumanStateName.REST && IsRestStateNeeded())
            {
                SetState(HumanStateName.REST);
            }

            HumanStateName nextState = 0;
            var availAttepmts = 100;

            while (availAttepmts > 0)
            {
                availAttepmts--;
                nextState = Human.NextState;
                if (nextState != currentStateName)
                    break;
            }

            SetState(nextState);
        }

        /// <summary>
        /// Определят нуждается ли персонаж в срочном отдыхе.
        /// </summary>
        /// <returns><c>true</c>, если усталость персонажа выше 70%, <c>false</c> иначе.</returns>
        public bool IsRestStateNeeded()
        {
            return Human.fatigue >= 0.7f;
        }

        public void SetPlayerControl(bool useControl)
        {
            if (useControl)
            {
                SetState(HumanStateName.PLAYER);
            }
            else if (currentHumanState is PlayHumanState)
            {
                (currentHumanState as PlayerControlState).Active = false;
            }
        }
    }
}
