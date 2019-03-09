using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Mutanium.Utils;

namespace Mutanium
{
    public enum HumanState
    {
        IDLE, REST, PLAY, WALKING, DEFEND, CHASING, ATTACK, WORK, DYING
    };

    public struct ScaledHumanState
    {
        public HumanState state;
        public float lengthScale;

        public ScaledHumanState(HumanState state, float lengthScale)
        {
            this.state = state;
            this.lengthScale = lengthScale;
        }
    }

    public class HumanController : MonoBehaviour
    {
        private static readonly ScaledHumanState[] BABY_STATES =  {
                 new ScaledHumanState(HumanState.IDLE, 0.3f),
                 new ScaledHumanState(HumanState.REST, 0.3f),
                 new ScaledHumanState(HumanState.PLAY, 0.9f)
                 };
        private static readonly ScaledHumanState[] TEEN_STATES =  {
                 new ScaledHumanState(HumanState.IDLE, 0.5f),
                 new ScaledHumanState(HumanState.REST, 0.3f),
                 new ScaledHumanState(HumanState.WALKING, 0.6f),
                 new ScaledHumanState(HumanState.DEFEND, 0.7f)
                 };
        private static readonly ScaledHumanState[] ADULT_STATES =
        {
            new ScaledHumanState(HumanState.IDLE, 0.3f),
                 new ScaledHumanState(HumanState.REST, 0.3f),
                 new ScaledHumanState(HumanState.WALKING, 0.5f),
                 new ScaledHumanState(HumanState.DEFEND, 1.0f),
                 new ScaledHumanState(HumanState.ATTACK, 1.0f),
                 new ScaledHumanState(HumanState.WORK, 1.0f)
                 };
        private static readonly ScaledHumanState[] OLDER_STATES =
        {
                 new ScaledHumanState(HumanState.IDLE, 0.7f),
                 new ScaledHumanState(HumanState.REST, 0.6f),
                 new ScaledHumanState(HumanState.WALKING, 0.7f),
                 new ScaledHumanState(HumanState.DEFEND, 0.7f)
                 };
        private const int lazySkipFrames = 4;

        public Relationship relationship;

        private NavMeshAgent agent;
        private Animator anim;
        private HumanState currentState;

        private ScaledHumanState[] availableStates;

        private Date birthDate;
        private bool isMen;
        private int age;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();

            OnBirth();
            LazyUpdate();
        }

        /// <summary>
        /// Sets the unit state.
        /// </summary>
        /// <param name="state">New state.</param>
        private void SetState(HumanState state)
        {
            //TODO
        }

        public void Update()
        {
            // Skip frames for better performance.
            if (Time.frameCount % lazySkipFrames == 0)
                LazyUpdate();
        }

        private void LazyUpdate()
        {
            var newAge = birthDate.MinutesFromNow() / 240;
            if (age != newAge)
            {
                age = newAge;
                OnAgeChanged();
            }
        }

        /// <summary>
        /// Called when unit was born.
        /// </summary>
        private void OnBirth()
        {
            birthDate = new Date();
            isMen = RandomUtils.GetRandomBool();
            SetState(HumanState.REST);
        }

        /// <summary>
        /// Called when unit age was increased.
        /// </summary>
        private void OnAgeChanged()
        {
            availableStates = GetAvailableStates();
        }

        /// <summary>
        /// Chooses random state among available states.
        /// </summary>
        internal void OnStateEnded()
        {
            //TODO: stub
        }

        private ScaledHumanState[] GetAvailableStates()
        {
            if (age <= 5)
            {
                return BABY_STATES;
            }
            if (age <= 14)
            {
                return TEEN_STATES;
            }
            if (age <= 65)
            {
                return ADULT_STATES;
            }
            return OLDER_STATES;
        }
    }
}
