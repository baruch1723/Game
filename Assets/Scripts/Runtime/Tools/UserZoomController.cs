using UnityEngine;

namespace Runtime.Tools
{
    public class UserZoomController : MonoBehaviour
    {
        private Camera _camera;

        private float _startFOV = 60f;
        private float _minFOV = 10f;
        private float _maxFOV = 150f;

        private float _targetFOV;
        private float _currentVelocity;

        [SerializeField] private float _zoomSpeed = 25f;
        [SerializeField] private float _smoothTime = 0.2f;

        private float _previousTouchDistance;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _camera.fieldOfView = _startFOV;
            _targetFOV = _camera.fieldOfView;
        }

        private void Update()
        {
            // Handle mouse scroll wheel input for zooming (for desktop)
            var scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                _targetFOV = Mathf.Clamp(_targetFOV - scrollInput * _zoomSpeed, _minFOV, _maxFOV);
            }

            // Handle mobile pinch-to-zoom
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                // Calculate current distance between the two touches
                float currentTouchDistance = Vector2.Distance(touch1.position, touch2.position);

                // On the first frame of the pinch, initialize _previousTouchDistance
                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    _previousTouchDistance = currentTouchDistance;
                }

                // Calculate the difference in touch distance between frames
                float touchDistanceDelta = _previousTouchDistance - currentTouchDistance;

                // Adjust the field of view based on the pinch gesture
                _targetFOV = Mathf.Clamp(_targetFOV + touchDistanceDelta * _zoomSpeed * 0.01f, _minFOV, _maxFOV);

                // Update the previous touch distance for the next frame
                _previousTouchDistance = currentTouchDistance;
            }

            // Smoothly transition to the target field of view
            _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, _targetFOV, ref _currentVelocity, _smoothTime);
        }
    }
}
