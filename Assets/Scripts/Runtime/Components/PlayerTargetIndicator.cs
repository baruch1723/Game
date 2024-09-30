using System.Collections;
using Runtime.Tools;
using UnityEngine;

namespace Runtime.Components
{
    public class PlayerTargetIndicator : MonoBehaviour
    {
        
        [SerializeField] private GameObject _indicatorPrefab;
        [SerializeField] private float _animationDuration = 1f;
        [SerializeField] private float _indicatorHeight = 0.01f;

        private GameObject _indicator;
        private SpriteRenderer _spriteRenderer;
        private UserPlayerController _player;
        private Coroutine _coroutine;

        public void Init(UserPlayerController playerController)
        {
            _player = playerController;
            _player.OnHitPoint += OnSetTarget;
        }

        private void OnDisable()
        {
            _player.OnHitPoint -= OnSetTarget;
            
            if (_indicator != null)
            {
                Destroy(_indicator);
            }
            
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        private void OnSetTarget(Vector3 pos, bool valid)
        {
            if (_indicator != null)
            {
                Destroy(_indicator);
            }
            
            var color = Color.green;
            if (!valid)
            {
                color = Color.red;
            }
            
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _indicator = Instantiate(_indicatorPrefab);
            _indicator.GetComponentInChildren<SpriteRenderer>().material.color = color;
            _indicator.transform.position = new Vector3(pos.x, pos.y + _indicatorHeight, pos.z);
            _coroutine = StartCoroutine(Animate());
        }
        
        private IEnumerator Animate()
        {
            _indicator.SetActive(true);
            yield return new WaitForSeconds(_animationDuration);
            _indicator.SetActive(false);
        }
    }
}