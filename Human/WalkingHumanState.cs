using Random = UnityEngine.Random;
using UnityEngine;

namespace Mutanium.Human
{
    internal class WalkingHumanState : HumanState
    {
        private const float WALKING_LENGTH_MIN = 8f;
        private const float WALKING_LENGTH_MAX = 16f;
        private const float MAX_WALKING_DISTANCE = 10f;
        private const int MAX_POINTS = 5;

        private readonly Vector3[] Waypoints;
        private int currentWaypoint;

        public WalkingHumanState(HumanController hc) : base(hc)
        {
            var num = Random.Range(2, MAX_POINTS + 1);
            Waypoints = new Vector3[num];
            for (var i = 0; i < num; i++)
            {
                var distance = Random.Range(MAX_WALKING_DISTANCE * 0.25f, MAX_WALKING_DISTANCE);
                var time = distance / hc.Agent.speed * 0.8f;
                var sign = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
                var angle = Random.Range(15f, 150f) * sign;
                Waypoints[i] = Quaternion.AngleAxis(angle, Vector3.up) * new Vector3(0, 0, distance);
            }
        }

        protected override void OnStart()
        {
            currentWaypoint = 0;
            Controller.Agent.enabled = true;
            Controller.Agent.destination = Controller.transform.position + Waypoints[currentWaypoint];
        }

        protected override bool OnUpdate()
        {
            var agent = Controller.Agent;

            if (!agent.pathPending && agent.remainingDistance < 2.0f)
            {
                if (currentWaypoint < Waypoints.Length)
                    agent.destination = Controller.transform.position + Waypoints[currentWaypoint++];
                else return true;
            }
            return false;
        }

        protected override void OnEnd()
        {
            Controller.Agent.enabled = false;
        }
    }
}