    using System;
using Runtime.Components;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Tools
{
    public class UserPlayerController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _movingTurnSpeed;
        [SerializeField] private float _stationaryTurnSpeed;

        private Camera _camera;
        private Rigidbody _rigidbody; 
        //private NavMeshAgent _agent;
        private DestinationTarget _destinationTarget;
        
        private float _turn;
        private float _forward;

        public event Action<Vector3,bool> OnHitPoint;
        
        private const string Animation_Forward = "Forward";
        private const string Animation_Attack = "Attack";
        private const string Animation_Turn = "Turn";
        private const string Layer_Move = "Walkable";
        private const string Layer_Treasure = "Treasure";

        private void Start()
        {
            _destinationTarget = null;
            //_agent = GetComponent<NavMeshAgent>();
            _rigidbody = GetComponent<Rigidbody>();
            
            ChangePlayerSpeed(30f);
            _camera = Camera.main;
        }

        public void ChangePlayerSpeed(float speed)
        {
            //_agent.speed = speed;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!Input.GetKeyDown(KeyCode.Space)) return;
            
            if (!TryGetComponent(out Enemy enemy)) return;
                
            Attack();
            enemy.Hit(100);
        }

        /*private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    var ray = _camera.ScreenPointToRay(touch.position);
                    if (!Physics.Raycast(ray, out var hit)) return;

                    if (ValidPosition(hit.collider.gameObject.layer))
                    {
                        SetMovePosition(hit);
                    }
                    else
                    {
                        OnHitPoint?.Invoke(hit.point, false);
                    }
                }
            }
        }*/
        
        private void FixedUpdate()
        {
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            //var move = v*Vector3.forward + h*Vector3.right;
            var camForward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            var move = v* camForward + h*_camera.transform.right;

            Move(move);
        }

        /*private void LateUpdate()
        {
            if (_destinationTarget != null)
            {
                _agent.SetDestination(_destinationTarget.Position);
            }

            if (_agent.remainingDistance > _agent.stoppingDistance)
            {
                Move(_agent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }*/

        /*private void OnMoveClicked()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;
            
            if (ValidPosition(hit.collider.gameObject.layer))
            {
                SetMovePosition(hit);
            }
            else
            {
                OnHitPoint?.Invoke(hit.point,false);
            }
        }*/

        /*private void SetMovePosition(RaycastHit hit)
        {
            _destinationTarget = new DestinationTarget()
            {
                Position = hit.point,
                Normal = hit.normal
            };
            
            OnHitPoint?.Invoke(hit.point,true);
        }*/
        
        private void Move(Vector3 move)
		{
			if (move.magnitude > 1f) move.Normalize();
            
			move = transform.InverseTransformDirection(move);
            if (_destinationTarget != null)
            {
                move = Vector3.ProjectOnPlane(move, _destinationTarget.Normal);
            }
            
			_turn = Mathf.Atan2(move.x, move.z);
			_forward = move.z;
            

            //ApplyExtraTurnRotation();
            
            UpdateAnimator();
		}

        private void UpdateAnimator()
		{
			_animator.SetFloat(Animation_Forward, _forward, 0.1f, Time.deltaTime);
			_animator.SetFloat(Animation_Turn, _turn, 0.5f, Time.deltaTime);
        }

        private void Attack()
        {
            _animator.SetTrigger(Animation_Attack);
        }
        
        /*private void ApplyExtraTurnRotation()
        {
            var turnSpeed = Mathf.Lerp(_stationaryTurnSpeed, _movingTurnSpeed, _forward);
            transform.Rotate(0, _turn * turnSpeed * Time.deltaTime, 0);
        }*/
        
        /*private static bool ValidPosition(int layer)
        {
            var isValid = layer == LayerMask.NameToLayer(Layer_Move) || layer == LayerMask.NameToLayer(Layer_Treasure);
            return isValid;
        }*/
    }
}