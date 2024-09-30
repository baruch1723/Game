using Runtime.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Views
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private Image _image;

        private void UpdateView(float amount)
        {
            _amount.text = $"{amount}%";
            _image.fillAmount = amount / 100f;
        }

        public void Init(float initValue)
        {
            UpdateView(initValue);
            Health.OnHealthUpdate += UpdateView;
        }

        private void OnDisable()
        {
            Health.OnHealthUpdate -= UpdateView;
        }
    }
}
