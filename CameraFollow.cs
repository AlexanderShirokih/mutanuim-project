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
            distance = Vector3.Distance(transform.position, target.position);
        }

        void LateUpdate()
        {
            xAngle += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            Quaternion rot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(yAngle, xAngle, 0), Time.deltaTime * smoothing);
            var newPos = target.position - rot * (Vector3.forward * distance);

            transform.position = newPos;
            transform.rotation = rot;
        }
    }
}