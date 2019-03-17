using UnityEngine;
using Mutanium.Human;

namespace Mutanium
{
    public class PlayerController : MonoBehaviour
    {
        private HumanController humanController;
        private Rigidbody mRigidbody;

        public float rotationSpeed = 2f;

        private Vector3 moveDir;

        public float speed = 8f;

        void Start()
        {
            humanController = GetComponent<HumanController>();
            mRigidbody = GetComponent<Rigidbody>();
            moveDir = Vector3.forward;

            humanController.SetPlayerControl(true);
        }

        void Update()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            moveDir.Set(h, 0, v);
            moveDir = moveDir.normalized * speed * Time.deltaTime;

            mRigidbody.MovePosition(transform.position + moveDir);
        }

        void OnDestroy()
        {
            humanController.SetPlayerControl(false);
        }
    }
}