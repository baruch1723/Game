using System.Collections;
using UnityEngine;

namespace Runtime.Views
{
    public class MousePositionView : MonoBehaviour
    {
        [SerializeField] private GameObject _indicator;
        [SerializeField] private float _animationDuration = 1f;

        private Coroutine _coroutine;
        
        private void Start()
        {
            _indicator.SetActive(false);
        }

        private void OnSetTarget(Vector3 pos)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _indicator.transform.position = new Vector3(pos.x, pos.y + 0.1f, pos.z);
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