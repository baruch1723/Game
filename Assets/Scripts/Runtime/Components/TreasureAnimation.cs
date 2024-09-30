using UnityEngine;

namespace Runtime.Components
{
    public class TreasureAnimation : MonoBehaviour
    {
        private Vector3 _startPosition;

        public float RotationSpeed = 50f;
        public float Amplitude = 1f;
        public float MoveSpeed = 1f;

        private void Start()
        {
            _startPosition = transform.position;
            var randomYRotation = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f);
        }

        private void Update()
        {
            var newY = _startPosition.y + Mathf.Sin(Time.time * MoveSpeed) * Amplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            transform.Rotate(0, RotationSpeed * Time.deltaTime, 0f);
        }
    }
}