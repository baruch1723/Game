/*using System.Collections.Generic;
using Runtime.Components;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Controllers
{
    public class SonarController : MonoBehaviour
    {
        public static UnityEvent<List<Vector2>> OnObjectsDetected = new();
        public float DetectionRadius = 10f;

        [SerializeField] private LayerMask _detectableLayer;

        private readonly List<Vector2> _positions = new();

        public void Init()
        {
            
        }
        
        private void OnEnable()
        {
            SonarSweep.OnSweepCompleted.AddListener(DetectObjects);
        }

        private void OnDisable()
        {
            SonarSweep.OnSweepCompleted.RemoveListener(DetectObjects);
        }

        private void DetectObjects()
        {
            _positions.Clear();

            var detectedObjects = Physics.OverlapSphere(transform.position, DetectionRadius, _detectableLayer);
            int count = detectedObjects.Length;

            if (_positions.Capacity < count)
            {
                _positions.Capacity = count;
            }

            for (int i = 0; i < count; i++)
            {
                var pos = GetSonarDot(detectedObjects[i].transform.position);
                _positions.Add(pos);
            }

            OnObjectsDetected.Invoke(_positions);
        }

        private Vector2 GetSonarDot(Vector3 objectPos)
        {
            var direction = objectPos - transform.position;
            var distance = direction.magnitude;
            var relativeDirection = transform.InverseTransformDirection(direction);
            return new Vector2(relativeDirection.x, relativeDirection.z).normalized * Mathf.Min(distance, DetectionRadius);
        }
    }
}*/


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Runtime.Controllers
{
    public class SonarController : MonoBehaviour
    {
        public static UnityEvent<List<Vector2>> OnObjectsDetected = new();
        public float DetectionRadius = 10f;
        
        [SerializeField] private LayerMask _detectableLayer;
        [SerializeField] private float _detectionInterval = 0.25f;

        private float _timeSinceLastDetection;
        
        private readonly List<Vector2> _positions = new();

        public void Init()
        {
            
        }
        
        private void Update()
        {
            _timeSinceLastDetection += Time.deltaTime;

            if (_timeSinceLastDetection >= _detectionInterval)
            {
                _timeSinceLastDetection = 0f;
                DetectObjects();
            }
        }

        private void DetectObjects()
        {
            _positions.Clear();
            
            var detectedObjects = Physics.OverlapSphere(transform.position, DetectionRadius, _detectableLayer);
            int count = detectedObjects.Length;

            if (_positions.Capacity < count)
            {
                _positions.Capacity = count;
            }

            for (int i = 0; i < count; i++)
            {
                var pos = GetSonarDot(detectedObjects[i].transform.position);
                _positions.Add(pos);
            }

            OnObjectsDetected.Invoke(_positions);
        }

        private Vector2 GetSonarDot(Vector3 objectPos)
        {
            var direction = objectPos - transform.position;
            var distance = direction.magnitude;
            //var relativeDirection = transform.InverseTransformDirection(direction);
            var relativeDirection = Camera.main.transform.InverseTransformDirection(direction);
            return new Vector2(relativeDirection.x, relativeDirection.z).normalized * Mathf.Min(distance, DetectionRadius);
        }
    }
}