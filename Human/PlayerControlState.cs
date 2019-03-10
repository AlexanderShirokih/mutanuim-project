namespace Mutanium.Human
{
    internal class PlayerControlState : HumanState
    {

        public bool Active { get; internal set; }

        public PlayerControlState(HumanController hc) : base(hc)
        {
        }

        protected override void OnStart()
        {
            Active = true;
        }

        protected override bool OnUpdate()
        {
            return !Active;
        }

        protected override void OnEnd() { }
    }
}