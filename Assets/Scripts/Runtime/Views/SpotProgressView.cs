/*using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Views
{
    public class SpotProgressView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _parent;

        public void UpdateView(float f,bool value)
        {
            _parent.SetActive(value);
            _image.fillAmount = f / 100f;
        }
    }
}*/