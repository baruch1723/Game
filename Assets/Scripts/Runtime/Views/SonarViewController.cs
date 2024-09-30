using System.Collections.Generic;
using Runtime.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Views
{
    public class SonarViewController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _sonarDotPrefab;
        [SerializeField] private Transform _sonarUIParent;
        [SerializeField] private float _sonarUIRadius = 200f;

        private SonarController _sonarController;
        
        
        [SerializeField] private RectTransform _imageTransform;
        [SerializeField] private Vector2 _enlargedSize = new Vector2(2.5f, 2.5f); // Enlarged size for the image
        [SerializeField] private float _doubleTapThreshold = 0.3f; // Maximum time interval for double tap detection

        
        private Vector2 _originalSize;
        private bool _isImageEnlarged = false;
        private float _lastTapTime = 0;


        public void Init(SonarController sonarController)
        {
            _sonarController = sonarController;
            SonarController.OnObjectsDetected.AddListener(OnDetectedObjects);
        }

        private void OnDisable()
        {
            SonarController.OnObjectsDetected.RemoveListener(OnDetectedObjects);
        }

        private void OnDetectedObjects(List<Vector2> positions)
        {
            ClearSonarDots();
            foreach (var pos in positions)
            {
                CreateSonarDot(pos);
            }
        }

        private void CreateSonarDot(Vector2 sonarPosition)
        {
            var sonarDot = Instantiate(_sonarDotPrefab, _sonarUIParent);
            var dotRectTransform = sonarDot.GetComponent<RectTransform>();
            dotRectTransform.anchoredPosition = sonarPosition / _sonarController.DetectionRadius * _sonarUIRadius;
        }
        
        private void ClearSonarDots()
        {
            foreach (Transform child in _sonarUIParent)
            {
                Destroy(child.gameObject);
            }
        }
        
        
        
        
        
        
        
        private void Start()
        {
            // Cache the original size of the image
            _originalSize = _imageTransform.localScale;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            // Detect double tap by checking time interval between taps
            if (Time.time - _lastTapTime < _doubleTapThreshold)
            {
                ToggleImageSize();
            }
            _lastTapTime = Time.time;
        }

        private void Update()
        {
            // Detect touch or click outside of the image to return to original size
            if (_isImageEnlarged && Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                if (!RectTransformUtility.RectangleContainsScreenPoint(_imageTransform, mousePosition, Camera.main))
                {
                    ResetImageSize();
                }
            }
        }

        private void ToggleImageSize()
        {
            if (_isImageEnlarged)
            {
                ResetImageSize();
            }
            else
            {
                // Enlarge the image
                _imageTransform.localScale = _enlargedSize;
                _isImageEnlarged = true;
            }
        }

        private void ResetImageSize()
        {
            // Return the image to its original size
            _imageTransform.localScale = _originalSize;
            _isImageEnlarged = false;
        }
    }
}