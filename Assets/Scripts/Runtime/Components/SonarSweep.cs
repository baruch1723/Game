using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Components
{
    public class SonarSweep : MonoBehaviour
    {
        [SerializeField] private float _sweepSpeed = 50f;
        
        private RectTransform _sweepTransform;

        private float _currentAngle = 0f;
        //public static UnityEvent OnSweepCompleted = new UnityEvent();
        
        private void Start()
        {
            _sweepTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            float deltaAngle = _sweepSpeed * Time.deltaTime;
            _sweepTransform.Rotate(0, 0, -deltaAngle);
            _currentAngle += deltaAngle;

            // Trigger event when a full rotation (360 degrees) is completed
            if (_currentAngle >= 360f)
            {
                _currentAngle = 0f;
                //OnSweepCompleted.Invoke();
            }
        }
    }
}

/*using UnityEngine;

namespace Runtime.Components
{
    public class SonarSweep : MonoBehaviour
    {
        [SerializeField] private float _sweepSpeed = 50f;
        
        private RectTransform _sweepTransform;

        private void Start()
        {
            _sweepTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _sweepTransform.Rotate(0, 0, -_sweepSpeed * Time.deltaTime);
        }
    }
}*/