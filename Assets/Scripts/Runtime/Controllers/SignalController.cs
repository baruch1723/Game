using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Controllers
{
    public class SignalController : MonoBehaviour
    {
        private Vector2 _closestTarget;
        private float _startDistance;
        private float _distance;
        private float _value;
        private float _timeSinceLastBeep;
        private float _beepInterval;

        public event Action<float> OnValueChanged;

        public AudioClip beepSound; // Assign your audio clip here
        private AudioSource _audioSource;

        public float thresholdDistance = 5f;  // The distance threshold where the sound starts playing
        public float maxBeepInterval = 1f;    // Maximum interval between beeps (farthest)
        public float minBeepInterval = 0.1f;  // Minimum interval between beeps (closest)

        public void Init()
        {
            SonarController.OnObjectsDetected.AddListener(SetClosestTargets);
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.clip = beepSound;
            _audioSource.loop = false; // We handle the repetition manually
            _audioSource.playOnAwake = false;
        }

        private void OnDisable()
        {
            SonarController.OnObjectsDetected.RemoveListener(SetClosestTargets);
        }

        private void Update()
        {
            _distance = GetDistance(_closestTarget);
            _value = MathExtensions.Remap(_distance, 0f, 10, 0f, 1f);
            OnValueChanged?.Invoke(_value);

            // Only play sound if the distance is below the threshold
            if (_distance < thresholdDistance)
            {
                // Calculate the beep interval based on distance
                _beepInterval = Mathf.Lerp(minBeepInterval, maxBeepInterval, _distance / thresholdDistance);

                // Check if it's time to play the next beep
                _timeSinceLastBeep += Time.deltaTime;
                if (_timeSinceLastBeep >= _beepInterval)
                {
                    PlayBeepSound();
                    _timeSinceLastBeep = 0f;
                }
            }
            else
            {
                // Optionally, stop the sound if player moves away
                _audioSource.Stop();
            }
        }

        private void PlayBeepSound()
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }

        private const float Tolerance = 0.01f;

        /// <summary>
        /// Set target for controller
        /// </summary>
        /// <param name="targets"></param>
        public void SetClosestTargets(List<Vector2> targets)
        {
            var closestTarget = GetClosestTarget(targets);
            if (!(Vector2.Distance(_closestTarget, closestTarget) > Tolerance)) return;
            
            _closestTarget = closestTarget;
            _startDistance = GetDistance(_closestTarget);
        }

        /// <summary>
        /// Get distance from target
        /// </summary>
        /// <returns></returns>
        private float GetDistance(Vector2 target)
        {
            return Vector2.Distance(transform.position, target);
        }

        /// <summary>
        /// Get closest vector
        /// </summary>
        /// <param name="targets"></param>
        /// <returns></returns>
        private Vector2 GetClosestTarget(List<Vector2> targets)
        {
            var closestTarget = targets.FirstOrDefault();
            var closestDistance = GetDistance(closestTarget);

            foreach (var target in targets)
            {
                var distance = GetDistance(target);
                if (!(distance < closestDistance)) continue;
                
                closestTarget = target;
                closestDistance = distance;
            }

            return closestTarget;
        }
    }
}



/*using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Controllers
{
    public class SignalController : MonoBehaviour
    {
        private Vector2 _closestTarget;
        private float _startDistance;
        private float _distance;
        private float _value;
        
        public event Action<float> OnValueChanged;

        public void Init()
        {
            SonarController.OnObjectsDetected.AddListener(SetClosestTargets);
        }

        private void OnDisable()
        {
            SonarController.OnObjectsDetected.RemoveListener(SetClosestTargets);
        }

        private void Update()
        {
            _distance = GetDistance(_closestTarget);
            _value = MathExtensions.Remap(_distance, 0f, 10, 0f, 1f);
            OnValueChanged?.Invoke(_value);
        }

        private const float Tolerance = 0.01f;
        
        /// <summary>
        /// Set target for controller
        /// </summary>
        /// <param name="target"></param>
        public void SetClosestTargets(List<Vector2> targets)
        {
            var closestTarget = GetClosestTarget(targets);
            if (!(Vector2.Distance(_closestTarget, closestTarget) > Tolerance)) return;
            
            _closestTarget = closestTarget;
            _startDistance = GetDistance(_closestTarget);
        }

        /// <summary>
        /// Get distance from target
        /// </summary>
        /// <returns></returns>
        private float GetDistance(Vector2 target)
        {
            return Vector2.Distance(transform.position, target);
        }

        /// <summary>
        /// Get closest vector
        /// </summary>
        /// <param name="targets"></param>
        /// <returns></returns>
        private Vector2 GetClosestTarget(List<Vector2> targets)
        {
            var closestTarget = targets.FirstOrDefault();
            var closestDistance = GetDistance(closestTarget);

            foreach (var target in targets)
            {
                var distance = GetDistance( target);
                if (!(distance < closestDistance)) continue;
                
                closestTarget = target;
                closestDistance = distance;
            }

            return closestTarget;
        }
    }
}*/