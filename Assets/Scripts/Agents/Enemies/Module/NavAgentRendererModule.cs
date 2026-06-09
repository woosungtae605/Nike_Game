using Agents.Module;
using Module;
using Systems.AnimationSystems;
using UnityEngine;
using UnityEngine.AI;

namespace Agents.Enemies.Module
{
    public class NavAgentRendererModule : RendererModule, IAfterInitModule
    { 
        [Header("NavAgent가 rotation과 position을 제어")]
        [SerializeField] private bool updateRotationByNavAgent;
        [SerializeField] private bool updatePositionByNavAgent;

        [Header("강제 회전 코드")] 
        [SerializeField] private bool forceRotation;
        [SerializeField] private float forceRotationSpeed;
        
        private INavMovement _navMovement;
        private NavMeshAgent _navAgent;
        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;

        public bool UpdateRotationByAnimator
        {
            get => !updateRotationByNavAgent;
            set
            {
                updateRotationByNavAgent = !value;
                if (_navAgent != null)
                {
                    _navAgent.updateRotation = updateRotationByNavAgent;
                }
            }
        }

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _navMovement = owner.GetModule<INavMovement>();
            Debug.Assert(_navMovement != null, $"NavMovement module required : {gameObject}");
        }

        public void AfterInit()
        {
            _navAgent = _navMovement.NavAgent;
            _navAgent.updatePosition = updatePositionByNavAgent;
            _navAgent.updateRotation = updateRotationByNavAgent;
        }

        private void OnAnimatorMove()
        {
            if (_navAgent == null) return;
            
            Vector3 rootPosition = Animator.rootPosition;
            rootPosition.y = _navAgent.nextPosition.y;
            //이걸 안해주면 땅바닥으로만 다녀.
            if (NavMesh.SamplePosition(rootPosition, out NavMeshHit hit, 0.3f, NavMesh.AllAreas))
            {
                _navAgent.nextPosition = hit.position; //애니메이션이 이동한 위치로 navAgent도 이동시킨다.
                _owner.transform.position = rootPosition; //_owner protected로 변경해라.
            }

            if (UpdateRotationByAnimator)
            {
                _owner.transform.rotation = Animator.rootRotation; //루트로테이션 반영.
            }
        }

        private void Update()
        {
            SynchronizeAnimatorAndNavAgent();
            ForceRotationControl();
        }


        private void SynchronizeAnimatorAndNavAgent()
        {
            if(_navAgent == null) return; 
            //애니메이터와 nav에이전트 사이의 차이값을 구한다.
            Vector3 worldDeltaPosition = _navAgent.nextPosition - _owner.transform.position;
            worldDeltaPosition.y = 0; //y는 어짜피 navAgent따라갈거니 무시한다.
            
            //오른쪽 방향으로 적용해야할 위치값
            float dx = Vector3.Dot(_owner.transform.right, worldDeltaPosition);
            //전방방향으로 적용해야할 위치 값을 구한다.
            float dy = Vector3.Dot(_owner.transform.forward, worldDeltaPosition);
            
            Vector2 deltaPosition = new Vector2(dx, dy);
            //이 델타를 바로 적용하지 않고 보간을 해서 부드럽게 이동시킨다.
            float smooth = Mathf.Min(1f, Time.deltaTime / 0.1f);
            
            //이전 스무스 값과 새 delta를 lerp로 섞어서 이동량을 부드럽게 유지시킨다.
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);
            _velocity = _smoothDeltaPosition / Time.deltaTime;

            //남은거리가 StopDistance보다 작다면 부드럽게 정지
            if (_navAgent.remainingDistance <= _navAgent.stoppingDistance)
            {
                _velocity = Vector2.Lerp(Vector2.zero, _velocity, _navAgent.remainingDistance / _navAgent.stoppingDistance);
            }
            
            float deltaMagnitude = worldDeltaPosition.magnitude;
            if (deltaMagnitude > _navAgent.radius * 0.5f)
            {
                _owner.transform.position = Vector3.Lerp(Animator.rootPosition, _navAgent.nextPosition, smooth);
            }
        }
        
        private void ForceRotationControl()
        {
            if (!forceRotation || _navAgent == null || _navMovement.IsArrived) return;

            Vector3 desiredDirection = _navAgent.steeringTarget - _owner.transform.position;
            if (desiredDirection.sqrMagnitude < 0.01f) return; //거의 정지상태면 회전하지 마라.
            
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            _owner.transform.rotation = Quaternion.RotateTowards(
                _owner.transform.rotation, 
                targetRotation, 
                forceRotationSpeed * Time.deltaTime);
        }

    }
}
