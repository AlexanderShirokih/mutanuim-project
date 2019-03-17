using UnityEngine;

namespace Mutanium
{
    public class CameraFollow : MonoBehaviour
    {

        public Transform target;

        public float smoothing = 5f;

        private float distance;

        private float xAngle;

        public float yAngle = 25f;

        public float rotationSpeed = 5f;

        void Start()
        {
            Vector3 targetPos = target ? target.position : transform.position + Vector3.forward * 5;
            distance = Vector3.Distance(transform.position, targetPos);
        }

        void LateUpdate()
        {
            if (!target) return;
            xAngle += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            Quaternion rot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(yAngle, xAngle, 0), Time.deltaTime * smoothing);
            var newPos = target.position - rot * (Vector3.forward * distance);

            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 5f);
            transform.rotation = rot;
        }
    }
}