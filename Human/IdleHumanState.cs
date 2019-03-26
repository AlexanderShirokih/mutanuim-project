using UnityEngine;

namespace Mutanium.Human
{
    internal class IdleHumanState : HumanState
    {
        private const float IDLE_LENGTH_MIN = 10f;
        private const float IDLE_LENGTH_MAX = 20f;

        private float length;
        private float time;

        public IdleHumanState(HumanController hc) : base(hc)
        {
        }

        protected override void OnStart()
        {
            time = 0f;
            length = Random.Range(IDLE_LENGTH_MIN, IDLE_LENGTH_MAX);
        }

        protected override bool OnUpdate()
        {
            time += Time.deltaTime;
            return time >= length;
        }

        protected override void OnEnd()
        {
        }
    }
}
