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

        internal NavMeshAgent Agent { get; private set; }
        internal Animator Animator { get; private set; }
        internal HumanInfo Human { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();

            OnBirth();
            LazyUpdate();
        }

        /// <summary>
        /// Sets the unit state.
        /// </summary>
        /// <param name="state">New state.</param>
        private void SetState(HumanStateName state)
        {
            Debug.Log($"Name={transform.name}, State={state}");
            currentHumanState = HumanState.Create(this, state);
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(currentHumanState.UpdateState(OnStateEnded));
            currentStateName = state;
            Animator.SetInteger("state", (int)state);
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
        /// Called when unit was born.
        /// </summary>
        private void OnBirth()
        {
            SetState(HumanStateName.REST);
        }

        /// <summary>
        /// Chooses random state among available states.
        /// </summary>
        private void OnStateEnded()
        {
            SetState(Human.NextState);
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
