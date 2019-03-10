using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Mutanium.Human;

namespace Mutanium.Animal
{
    /// <summary>
    /// Available animal states(idle, walking, chasing, attack or defend).
    /// </summary>
    public enum AnimalState
    {
        IDLE, ATTACK, DEFEND, WALKING, CHASING, DYING
    };

    /// <summary>
    /// Class is used to control an animal behaviour.
    /// </summary>
    public class AnimalController : MonoBehaviour
    {
        private NavMeshAgent agent;
        /// <summary>
        /// Cached instance to an Animator object
        /// </summary>
        private Animator anim;
        /// <summary>
        /// Current animator state
        /// </summary>
        private AnimalState currentState = AnimalState.IDLE;
        /// <summary>
        /// Root object for all characters
        /// </summary>
        private Transform playersRoot;
        /// <summary>
        /// Current target object to attack or for defending 
        /// </summary>
        private Transform target;
        /// <summary>
        /// The number frames to skip. Three for process one of three frames. 
        /// </summary>
        private int interval = 3;

        private float defaultAgentSpeed;

        private float currentOutlookDistance;

        private Vector3 nextDestination;

        private Coroutine currentCoroutine;

        /// <summary>
        /// The range of attack or defense activation.
        /// </summary>
        public float activeDistance = 20f;

        /// <summary>
        /// The distance of one run.
        /// </summary>
        public float maxWalkingDistance = 25f;

        /// <summary>
        /// Distance from which to start the attack. Must be less or equal than activeDistane.
        /// </summary>
        public float attackingDistance = 6.5f;

        /// <summary>
        /// distance from which to start the attack
        /// </summary>
        public float runAcceleration = 1.5f;
        /// <summary>
        /// The relationship type.
        /// </summary>
        public Relationship relationship;

        public AnimalInfo animal;

        /// <summary>
        /// Extention multiplier of the outlook when animal in the attacking state
        /// </summary>
        public float attackingOutlookScale = 1.5f;

        void Awake ()
        {
            animal = new AnimalInfo();
        }

        void Start()
        {
            // Init and cache some variables.
            playersRoot = GameObject.Find("PlayersRoot").transform;
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            defaultAgentSpeed = agent.speed;
            currentOutlookDistance = activeDistance;
            nextDestination = transform.position;
            SetState(AnimalState.IDLE);
        }

        void Update()
        {
            #region Check for attackers
            // Skip frames for better performance.
            if (Time.frameCount % interval == 0)
            {
                Transform nearestT = null;
                var dist = float.MaxValue;

                foreach (Transform t in playersRoot)
                {
                    // Find the distance between each object and yourself. 
                    var offset = t.position - transform.position;
                    var sqrLen = offset.sqrMagnitude;
                    if (sqrLen < currentOutlookDistance * currentOutlookDistance)
                    {
                        // We found the same target, so we don't need to find another.
                        if (t == target)
                        {
                            nearestT = t;
                            break;
                        }

                        // Get the nearest target
                        if (sqrLen < dist)
                        {
                            dist = sqrLen;
                            nearestT = t;
                        }
                    }
                }

                bool hasTarget = nearestT;

                // Obtain the new target or keep going
                if (hasTarget)
                {
                    if (nearestT != target)
                        SetTarget(nearestT);
                }
                else if (target)
                    SetTarget(null);

                if (agent.enabled && !agent.pathPending && agent.remainingDistance < 2.0f)
                    agent.destination = nextDestination;
            }
            #endregion
        }

        /// <summary>
        /// Call it to send damage to the animal.
        /// </summary>
        /// <param name="human">Human that sends damage</param>
        /// <param name="damage">damage level in scale [0..1]</param>
        public void ReceiveDamage(HumanController human, float damage)
        {
            animal.health -= damage;
            if (animal.health <= 0.0f)
            {
                SetState(AnimalState.DYING);
                return;
            }

            SetTarget(human.transform);
            var rel = RelationshipResolver.Resolve(relationship, human.relationship);
            if (rel == RelationshipResolver.GOOD)
            {
                SetState(AnimalState.DEFEND);
            }
            else
            {
                SetState(AnimalState.CHASING);
            }
        }

        /// <summary>
        /// Sets the new target. Depends on relation to that object sets the state to attack or defend.
        /// </summary>
        /// <param name="t">The new target.</param>
        private void SetTarget(Transform t)
        {
            target = t;
            if (!t)
            {
                SetState(AnimalState.IDLE);
            }
            else
            {
                HumanController hc = t.GetComponent<HumanController>();
                int rel = RelationshipResolver.Resolve(relationship, hc.relationship);
                if (rel == RelationshipResolver.BAD)
                    SetState(AnimalState.CHASING);
            }
        }

        /// <summary>
        /// Sets the current state and switches the Animator.
        /// </summary>
        /// <param name="state">The new AnimalState.</param>
        private void SetState(AnimalState state)
        {
            if (state == currentState)
                return;
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            anim.SetInteger("state", (int)state);
            currentState = state;
            currentOutlookDistance = activeDistance;

            if (state == AnimalState.IDLE || state == AnimalState.WALKING)
            {
                currentCoroutine = StartCoroutine(GetNextMovement());
            }
            else if (state == AnimalState.CHASING)
            {
                currentOutlookDistance *= attackingOutlookScale;
                currentCoroutine = StartCoroutine(ChasingMovement());
            }
            else if (state == AnimalState.ATTACK)
            {
                currentCoroutine = StartCoroutine(AttackMovement());
            }
            else if (state == AnimalState.DEFEND)
            {
                currentCoroutine = StartCoroutine(DefendMovement());
            }
            else if (state == AnimalState.DYING)
            {
                currentCoroutine = StartCoroutine(DyingCoroutine());
            }
        }

        /// <summary>
        /// Coroutine that generates the next movement and its duration.
        /// </summary>
        IEnumerator GetNextMovement()
        {
            var isWalking = Random.Range(0, 2) == 0;
            var distance = Random.Range(maxWalkingDistance * 0.25f, maxWalkingDistance);
            var time = isWalking ? (distance / agent.speed * 0.8f) : Random.Range(2f, 5f);

            yield return new WaitForSeconds(time);

            if (isWalking)
            {
                var sign = Random.Range(0, 2) == 0 ? -1.0f : 1.0f;
                var angle = Random.Range(15f, 150f) * sign;
                var direction = Quaternion.AngleAxis(angle, Vector3.up) * new Vector3(0, 0, distance);
                nextDestination = transform.position + direction;
                agent.enabled = true;
                SetState(AnimalState.WALKING);
            }
            else
            {
                agent.enabled = false;
                SetState(AnimalState.IDLE);
            }
        }

        IEnumerator ChasingMovement()
        {
            yield return new WaitWhile(new Func<bool>(() =>
            {
                if (!target) return false;
                var offset = target.position - transform.position;
                var distanceToTarget = offset.sqrMagnitude;
                nextDestination = target.position - offset.normalized * attackingDistance * 0.8f;
                nextDestination.y = target.position.y;
                agent.enabled = true;
                agent.speed = defaultAgentSpeed * runAcceleration;
                return distanceToTarget > attackingDistance * attackingDistance;
            }));
            agent.speed = defaultAgentSpeed;
            SetState(target ? AnimalState.ATTACK : AnimalState.IDLE);
        }

        IEnumerator AttackMovement()
        {
            agent.enabled = false;
            yield return new WaitWhile(new Func<bool>(() =>
            {
                if (!target) return false;
                var lookVector = target.position;
                lookVector.y = transform.position.y;
                transform.LookAt(lookVector);
                var offset = target.position - transform.position;
                var distanceToTarget = offset.sqrMagnitude;
                return distanceToTarget < attackingDistance * attackingDistance;
            }));
            SetState(target ? AnimalState.CHASING : AnimalState.IDLE);
        }


        IEnumerator DefendMovement()
        {
            yield return new WaitWhile(new Func<bool>(() =>
            {
                if (!target) return false;
                var offset = transform.position - target.position;
                var distanceToTarget = Vector3.Distance(target.position, transform.position);
                var normalized = offset / distanceToTarget;
                var angle = Random.Range(-25f, 25f);
                var distance = Random.Range(maxWalkingDistance * 0.2f, maxWalkingDistance * 0.35f);
                var direction = Quaternion.AngleAxis(angle, Vector3.up) * (normalized * distance);
                nextDestination = transform.position + direction;
                Debug.DrawRay(transform.position, direction);
                Debug.DrawLine(transform.position, agent.destination, Color.red);
                agent.enabled = true;
                agent.speed = defaultAgentSpeed * runAcceleration;
                return distanceToTarget < activeDistance;
            }));
            agent.speed = defaultAgentSpeed;
            SetState(AnimalState.IDLE);
        }

        IEnumerator DyingCoroutine()
        {
            agent.enabled = false;
            yield return new WaitUntil(() =>
            {
                AnimatorStateInfo asi = anim.GetCurrentAnimatorStateInfo(0);
                return asi.IsName("Dying") && asi.normalizedTime >= 0.9f;
            });
            OnDie();
        }

        /// <summary>
        /// Called when animal health becomes zero.
        /// </summary>
        void OnDie()
        {
            Destroy(gameObject, 5f);
        }
    }
}
