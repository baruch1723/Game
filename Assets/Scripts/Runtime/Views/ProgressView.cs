using Runtime.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Views
{
    public class ProgressView : MonoBehaviour
    {
        [SerializeField] private Image _areaProgressImage;
        [SerializeField] private TextMeshProUGUI _areaProgressAmount;

        private void UpdateView(float amount)
        {
            _areaProgressAmount.text = $"{amount}%";
            _areaProgressImage.fillAmount = amount / 100f;
        }

        public void Init(float initValue)
        {
            UpdateView(initValue);
            GameController.OnProgressUpdate += UpdateView;
        }

        private void OnDisable()
        {
            GameController.OnProgressUpdate -= UpdateView;
        }
    }
}
