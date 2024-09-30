using Runtime.Controllers;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Components
{
    public class Spot : MonoBehaviour
    {
        public float RequiredHoldTime;
        public bool IsInTrigger { get; private set; }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsInTrigger = true;

                // Notify the UI to show the button and start tracking progress
                UIManager.Instance.ClaimButton.SetCurrentSpot(this);
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
                UIManager.Instance.ClaimButton.SetButtonPosition(screenPosition);
                UIManager.Instance.ClaimButton.ShowButton();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsInTrigger = false;

                // Notify the UI to stop tracking and hide the button
                UIManager.Instance.ClaimButton.ResetProgress();
                UIManager.Instance.ClaimButton.HideButton();
            }
        }
    }
}