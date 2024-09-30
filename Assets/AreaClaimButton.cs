using Runtime.Components;
using Runtime.Controllers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AreaClaimButton : MonoBehaviour
{
    [SerializeField] private Button _button; // Reference to the child button
    [SerializeField] private Image _progressImage;
    [SerializeField] private GameObject _parent; // Parent object containing the button and progress bar

    private Spot _currentSpot;
    private float _progress;
    private float _holdTime;
    private bool _isHoldingButton = false;

    private void Start()
    {
        // Ensure that the button reference is assigned
        if (_button == null)
        {
            _button = GetComponentInChildren<Button>();
        }

        // Add EventTrigger programmatically to the button
        AddEventTriggerToButton();

        // Initialize UI state
        ResetProgress();
        HideButton();
    }

    private void Update()
    {
        // Only handle the button hold if the current spot is set, the button is visible, and the button is being held
        if (_currentSpot != null && _isHoldingButton)
        {
            _holdTime += Time.deltaTime;
            _progress = Mathf.Clamp(_holdTime / _currentSpot.RequiredHoldTime * 100f, 0f, 100f);
            UpdateClaimProgress(_progress);

            if (_holdTime >= _currentSpot.RequiredHoldTime)
            {
                // Successfully held, claim the spot
                ClaimSpot();
            }
        }
    }

    public void SetCurrentSpot(Spot spot)
    {
        if(_currentSpot == spot) return;
        
        _currentSpot = spot;
        ResetProgress();
    }

    public void SetButtonPosition(Vector3 screenPosition)
    {
        _parent.transform.position = screenPosition;
    }

    public void UpdateClaimProgress(float progress)
    {
        Debug.Log($"{progress}");
        _progressImage.fillAmount = progress / 100f;
    }

    public void ShowButton()
    {
        _parent.SetActive(true);
    }

    public void HideButton()
    {
        _parent.SetActive(false);
    }

    public void ResetProgress()
    {
        Debug.Log("reset");
        _holdTime = 0f;
        _progressImage.fillAmount = 0f;
    }

    private void ClaimSpot()
    {
        if (_currentSpot != null)
        {
            FindObjectOfType<CollectSpots>(true).RemoveSpot(_currentSpot.gameObject);
            ResetProgress();
            Destroy(_currentSpot.gameObject);
            HideButton();
        }
    }

    private void AddEventTriggerToButton()
    {
        EventTrigger trigger = _button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = _button.gameObject.AddComponent<EventTrigger>();
        }

        // Add OnPointerDown event
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(pointerDownEntry);

        // Add OnPointerUp event
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(pointerUpEntry);
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        _isHoldingButton = true;
        Debug.Log("Button Pressed Down");
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        _isHoldingButton = false;
        ResetProgress();
        Debug.Log("Button Released");
    }
}
