using UnityEngine;

using System;
using System.Collections;

namespace Mutanium.Human
{
    /// <summary>
    /// Состояние отдыха. Юнит идёт в направлении дома, затем исчезает. 
    /// После определённого времени юнит снова появляется и отходит от дома на определённое расстояние.
    /// </summary>
    internal class RestHumanState : HumanState
    {
        private const float MAX_REST_AT_ONCE = 0.75f;
        private const float MIN_DISTANCE = 5f;
        private const float LEAVING_DISTANCE = 8f;
        private const float REST_LENGTH_MIN = 10f;
        private const float REST_LENGTH_MAX = 20f;

        private float waitingTime;

        public RestHumanState(HumanController hc) : base(hc)
        {
        }

        protected override void OnStart()
        {
            // Коэффициент продолжительности отдыха [1-MAX_REST_AT_ONCE, 1]
            var restingTime = 1f - Math.Max(0, MAX_REST_AT_ONCE - Controller.Human.fatigue);
            // Перемасштабируем пределы на [0;1]
            restingTime -= 1 - MAX_REST_AT_ONCE;
            restingTime /= MAX_REST_AT_ONCE;

            waitingTime = REST_LENGTH_MIN + (REST_LENGTH_MAX - REST_LENGTH_MIN) * restingTime;

            Controller.Agent.enabled = true;
        }

        public override IEnumerator UpdateState(Action onStateEnded)
        {
            OnStart();

            yield return Controller.StartCoroutine(Act());

            OnEnd();
            onStateEnded();
        }

        private IEnumerator Act()
        {
            var agent = Controller.Agent;
            ReferencedId<HouseInfo> house = Controller.Human.AssignedHouse;

            if (house != null)
            {
                Vector3 target = house.Get().position;
                agent.destination = target;

                // Идём к дому
                yield return new WaitUntil(() =>
                {
                    return agent.remainingDistance < MIN_DISTANCE;
                });

                // Исчезаем
                Renderer renderer = Controller.gameObject.GetComponent<Renderer>();
                renderer.enabled = false;
                agent.ResetPath();

                // Ждём в доме
                yield return new WaitForSeconds(waitingTime);

                // Снова появляемся
                renderer.enabled = true;

                // Отходим от дома
                Vector3 pos = Controller.transform.position;
                Vector3 dir = (pos - target).normalized * LEAVING_DISTANCE;
                agent.destination = pos + dir;
                yield return new WaitUntil(() => agent.remainingDistance < 2f);
            }
            else
            {
                // Если дома нет, то выходим из состояния
                yield break;
            }
        }

        protected override bool OnUpdate()
        {
            return false;
        }

        protected override void OnEnd()
        {
            Controller.Agent.ResetPath();
            Controller.Agent.enabled = false;

            //Пересчитываем восстановленную усталость
            Controller.Human.fatigue -= Math.Min(Controller.Human.fatigue, MAX_REST_AT_ONCE);
        }
    }
}