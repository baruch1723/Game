using UnityEngine;

namespace ThirdPersonCharacter.Scripts
{
    public class ThirdPersonCharacterV2 : MonoBehaviour
    {
        [SerializeField] float m_MovingTurnSpeed = 360;
        [SerializeField] float m_StationaryTurnSpeed = 180;
        [SerializeField] float m_MoveSpeedMultiplier = 1f;
        [SerializeField] float m_AnimSpeedMultiplier = 1f;
        [SerializeField] private float m_Speed = 5f;

        [SerializeField] private Animator m_Animator;

        private Rigidbody m_Rigidbody;
        private Vector3 m_GroundNormal;
        private float m_TurnAmount;
        private float m_ForwardAmount;

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                                      RigidbodyConstraints.FreezeRotationZ;
        }

        public void Move(Vector3 move)
        {
            if (move.magnitude > 1f) move.Normalize();

            move = transform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            m_TurnAmount = Mathf.Atan2(move.x, move.z);
            m_ForwardAmount = move.z;

            ApplyExtraTurnRotation();
            UpdateAnimator(move);
        }

        private void UpdateAnimator(Vector3 move)
        {
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);

            if (move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier * m_Speed;
            }
            else
            {
                m_Animator.speed = 1;
            }
        }

        private void ApplyExtraTurnRotation()
        {
            var turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
        }

        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (!(Time.deltaTime > 0)) return;

            var v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier * m_Speed) / Time.deltaTime;
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }
}