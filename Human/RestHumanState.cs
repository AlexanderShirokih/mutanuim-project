namespace Mutanium.Human
{
    internal class RestHumanState : HumanState
    {
        public RestHumanState(HumanController hc) : base(hc)
        {
        }

        protected override void OnStart()
        {
            //Nothing to do
        }

        protected override bool OnUpdate()
        {
            //TODO: 
            return true;
        }

        protected override void OnEnd()
        {
        }
    }
}
