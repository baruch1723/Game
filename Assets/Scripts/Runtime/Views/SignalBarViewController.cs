using Runtime.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Views
{
    public class SignalBarViewController : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private SignalController _signalController;

        public void Init(SignalController signalController)
        {
            _signalController = signalController;
            _signalController.OnValueChanged += ChangeValue;
        }

        public void OnDisable()
        {
            _signalController.OnValueChanged += ChangeValue;
        }

        private void ChangeValue(float value)
        {
            _image.fillAmount = 1 - value;
        }
    }
}