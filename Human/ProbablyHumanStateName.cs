namespace Mutanium.Human
{
    public struct ProbablyHumanStateName
    {
        public HumanStateName state;
        public float probability;

        public ProbablyHumanStateName(HumanStateName state, float probability)
        {
            this.state = state;
            this.probability = probability;
        }
    }
}
