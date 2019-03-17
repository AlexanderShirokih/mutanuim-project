using UnityEngine;

namespace Mutanium.Human
{
    internal class RestHumanState : HumanState
    {
        private const float REST_LENGTH_MIN = 10f;
        private const float REST_LENGTH_MAX = 20f;

        private float length;
        private float time;

        public RestHumanState(HumanController hc) : base(hc)
        {
        }

        protected override void OnStart()
        {
            time = 0f;
            length = Random.Range(REST_LENGTH_MIN, REST_LENGTH_MAX);
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
