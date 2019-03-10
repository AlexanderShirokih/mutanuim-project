using UnityEngine;

namespace Mutanium.Human
{
    internal class PlayHumanState : HumanState
    {
        private const float PLAY_LENGTH_MIN = 10f;
        private const float PLAY_LENGTH_MAX = 20f;

        private float length;
        private float time;


        public PlayHumanState(HumanController hc) : base(hc)
        {
        }

        protected override void OnStart()
        {
            time = 0f;
            length = Random.Range(PLAY_LENGTH_MIN, PLAY_LENGTH_MAX);
        }

        protected override bool OnUpdate()
        {
            time += Time.deltaTime;
            return time >= length;
        }

        protected override void OnEnd()
        { }
    }
}