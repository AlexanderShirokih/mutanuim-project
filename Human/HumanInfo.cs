using System;
using Random = UnityEngine.Random;

namespace Mutanium.Human
{
    public enum HumanStateName
    {
        IDLE, REST, PLAY, WALKING, DEFEND, CHASING, ATTACK, WORK, DYING, PLAYER
    };

    [Serializable]
    public class HumanInfo
    {
        private static readonly ProbablyHumanStateName[] BABY_STATES =  {
                 new ProbablyHumanStateName(HumanStateName.IDLE, 0.20f),
                 new ProbablyHumanStateName(HumanStateName.REST, 0.15f),
                 new ProbablyHumanStateName(HumanStateName.PLAY, 0.65f)
                 };
        private static readonly ProbablyHumanStateName[] TEEN_STATES =  {
                 new ProbablyHumanStateName(HumanStateName.IDLE, 0.3f),
                 new ProbablyHumanStateName(HumanStateName.REST, 0.2f),
                 new ProbablyHumanStateName(HumanStateName.WALKING, 0.5f)
                 };
        private static readonly ProbablyHumanStateName[] ADULT_STATES =
        {
                 new ProbablyHumanStateName(HumanStateName.IDLE, 0.1f),
                 new ProbablyHumanStateName(HumanStateName.REST, 0.15f),
                 new ProbablyHumanStateName(HumanStateName.WALKING, 0.1f),
                 new ProbablyHumanStateName(HumanStateName.WORK, 0.65f)
                 };
        private static readonly ProbablyHumanStateName[] OLDER_STATES =
        {
                 new ProbablyHumanStateName(HumanStateName.IDLE, 0.2f),
                 new ProbablyHumanStateName(HumanStateName.REST, 0.3f),
                 new ProbablyHumanStateName(HumanStateName.WALKING, 0.5f)
                 };

        public Date BirthDate { get; set; }
        public bool IsMen { get; set; }
        public int Age { get; set; }


        public bool CanDefends() => Age >= 6;
        public bool CanAttack() => Age >= 14 && Age <= 65;

        private ProbablyHumanStateName[] AvailableStates
        {
            get
            {
                if (Age <= 5)
                {
                    return BABY_STATES;
                }
                if (Age <= 14)
                {
                    return TEEN_STATES;
                }
                if (Age <= 65)
                {
                    return ADULT_STATES;
                }
                return OLDER_STATES;
            }
        }

        public HumanStateName NextState
        {
            get
            {
                float rand = Random.value;
                float cumulative = 0f;
                HumanStateName last = HumanStateName.IDLE;

                foreach (var state in AvailableStates)
                {
                    cumulative += state.probability;
                    if (rand < cumulative)
                    {
                        last = state.state;
                        break;
                    }
                }

                return last;
            }
        }
    }
}
