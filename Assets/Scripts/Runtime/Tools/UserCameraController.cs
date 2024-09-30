using Runtime.Components;
using UnityEngine;

namespace Runtime.Tools
{
    public class UserCameraController : MonoBehaviour
    {
        [SerializeField] private float _distance = 30f;
        [SerializeField] private float _xSpeed = 120f;
        [SerializeField] private float _ySpeed = 120f;
        [SerializeField] private float _yMinLimit = 30f;
        [SerializeField] private float _yMaxLimit = 80f;

        private Transform _player;
        private float _x;
        private float _y;

        private void Start()
        {
            _player = FindObjectOfType<Player>().transform;

            var angles = transform.eulerAngles;
            _x = angles.y;
            _y = angles.x;
        }

        private void LateUpdate()
        {
            // Mouse input for desktop
            if (Input.GetMouseButton(1))
            {
                _x += Input.GetAxis("Mouse X") * _xSpeed * 0.02f;
                _y -= Input.GetAxis("Mouse Y") * _ySpeed * 0.02f;
            }

            // Touch input for mobile (two-finger rotation)
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                // Only move the camera if both touches are moving
                if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
                {
                    // Calculate average delta movement between the two touches
                    Vector2 averageDelta = (touch1.deltaPosition + touch2.deltaPosition) / 2;

                    _x += averageDelta.x * _xSpeed * 0.01f;
                    _y -= averageDelta.y * _ySpeed * 0.01f;
                }
            }

            // Clamp the vertical angle
            _y = Mathf.Clamp(_y, _yMinLimit, _yMaxLimit);

            // Calculate rotation and position based on updated angles
            var rotation = Quaternion.Euler(_y, _x, 0f);
            var position = rotation * new Vector3(0.0f, 0.0f, -_distance) + _player.position;

            // Apply position and rotation to the camera
            SetTransform(position, rotation);
        }

        private void SetTransform(Vector3 pos, Quaternion rot)
        {
            transform.position = pos;
            transform.rotation = rot;
        }
    }
}
