using UnityEngine;

namespace ThirdPersonCharacter.Scripts
{
    public class ThirdPersonUserControlV2 : MonoBehaviour
    {
        private ThirdPersonCharacterV2 m_Character;
        private Transform m_Cam;
        private Vector3 m_CamForward;
        private Vector3 m_Move;
        private FloatingJoystick _joystick;

        private void Start()
        {
            _joystick = FindObjectOfType<FloatingJoystick>(true);
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.",
                    gameObject);
            }

            m_Character = GetComponent<ThirdPersonCharacterV2>();
        }

        private void FixedUpdate()
        {
            #if !UNITY_EDITOR
            HandleMobileInput();
            #else
            HandleDesktopInput();
            #endif
        }

        private void HandleMobileInput()
        {
            // Only allow movement if exactly one finger is touching the screen
            if (Input.touchCount == 1)
            {
                var h = _joystick.Horizontal;
                var v = _joystick.Vertical;

                if (m_Cam != null)
                {
                    m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                    m_Move = v * m_CamForward + h * m_Cam.right;
                }
                else
                {
                    m_Move = v * Vector3.forward + h * Vector3.right;
                }

                m_Character.Move(m_Move);
            }
            else
            {
                // Stop movement if no fingers or more than one finger are touching the screen
                m_Character.Move(Vector3.zero);
            }
        }

        private void HandleDesktopInput()
        {
            // Get input from WASD or arrow keys
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (m_Cam != null)
            {
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                m_Move = v * Vector3.forward + h * Vector3.right;
            }

            m_Character.Move(m_Move);

            // Stop movement if no input is detected
            if (h == 0 && v == 0)
            {
                m_Character.Move(Vector3.zero);
            }
        }
    }
}







/*using UnityEngine;
using UnityEngine.Serialization;

namespace ThirdPersonCharacter.Scripts
{
    public class ThirdPersonUserControlV2 : MonoBehaviour
    {
        private ThirdPersonCharacterV2 m_Character;
        private Transform m_Cam;
        private Vector3 m_CamForward;
        private Vector3 m_Move;
        private FloatingJoystick _joystick;

        private void Start()
        {
            _joystick = FindObjectOfType<FloatingJoystick>(true);
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.",
                    gameObject);
            }

            m_Character = GetComponent<ThirdPersonCharacterV2>();
        }

        private void FixedUpdate()
        {
            // Only allow movement if exactly one finger is touching the screen
            if (Input.touchCount == 1)
            {
                var h = _joystick.Horizontal;
                var v = _joystick.Vertical;

                if (m_Cam != null)
                {
                    m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                    m_Move = v * m_CamForward + h * m_Cam.right;
                }
                else
                {
                    m_Move = v * Vector3.forward + h * Vector3.right;
                }

                m_Character.Move(m_Move);
            }
            else
            {
                // Stop movement if no fingers or more than one finger are touching the screen
                m_Character.Move(Vector3.zero);
            }
        }
    }
}*/