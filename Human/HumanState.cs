using System.Collections;
using UnityEngine;
using System;

namespace Mutanium.Human
{

    public abstract class HumanState
    {
        protected HumanState(HumanController hc)
        {
            Controller = hc;
        }

        protected HumanController Controller { get; }

        protected abstract void OnStart();
        protected abstract bool OnUpdate();
        protected abstract void OnEnd();


        public IEnumerator UpdateState(Action onStateEnded)
        {
            OnStart();
            yield return new WaitUntil(OnUpdate);
            OnEnd();
            onStateEnded();
        }

        public static HumanState Create(HumanController hc, HumanStateName stateName)
        {
            switch (stateName)
            {
                case HumanStateName.IDLE:
                    return new IdleHumanState(hc);
                case HumanStateName.REST:
                    return new RestHumanState(hc);
                case HumanStateName.PLAY:
                    return new PlayHumanState(hc);
                case HumanStateName.WALKING:
                    return new WalkingHumanState(hc);
                case HumanStateName.DEFEND:
                    break;
                case HumanStateName.CHASING:
                    break;
                case HumanStateName.ATTACK:
                    break;
                case HumanStateName.WORK:
                    break;
                case HumanStateName.DYING:
                    break;
                case HumanStateName.PLAYER:
                    return new PlayerControlState(hc);
            }
            return null;
        }
    }
}
