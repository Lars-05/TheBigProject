using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(WeepingAngleLogic))]
public class BurningManAI : MonoBehaviour
{
    [Header("NavMesh")]
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [Header("Dependencies")]
    [SerializeField] private Transform _mapCenter;
    [SerializeField] private Transform _target;
    [SerializeField] private WeepingAngleLogic _weepingAngleLogic;
    
    [Header("Stats")]
    [SerializeField] private float _teleportRadius;
    [SerializeField] private float _maxLookDuration;
    [SerializeField] private float _lookAwayCooldownRate;
        
    [Header("Stalk Stats")]
    [SerializeField] private float _stalkSpeed;
    [SerializeField] private float _minDistanceFromTarget;
    [SerializeField] private float _maxSecondLookedAt;

    [Header("Chase Stats")]
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _maxChaseTime;
    [SerializeField] private float _minDistanceForAttackTarget;

    public enum States
    {
        STALKING,
        CHASING
    }

    [HideInInspector] public States _currentState;
    
    private float timeLookedAt = 0;

    void Start()
    {
        _currentState = States.STALKING;
        _navMeshAgent.speed = _stalkSpeed;
              
        
    }

    private float _timeChasing;
    private void Update()
    {
        if (_currentState == States.STALKING)
        {
            if (!_weepingAngleLogic.LookedAt())
            {
                Debug.Log("[BurningManAI]: Looked at for: " + timeLookedAt + " seconds");
                timeLookedAt += Time.deltaTime;
                _navMeshAgent.enabled = false;
            }
            else
            {
                timeLookedAt -= Time.deltaTime * _lookAwayCooldownRate;
                _navMeshAgent.enabled = true;
                GoToPosition(_target.transform.position);
            }
            
            if (Vector3.Distance(transform.position, _target.position) <= _minDistanceFromTarget || timeLookedAt > _maxSecondLookedAt)
            {
                StartChase();
            }
        }
        
        else if(_currentState == States.CHASING)
        {
            _timeChasing += Time.deltaTime;
            _navMeshAgent.speed = _chaseSpeed;
            Debug.Log("[BurningManAI]: Is chasing you, seconds remaining: " + (_maxChaseTime - _timeChasing)  + " seconds");
            GoToPosition(_target.transform.position);
            if (Vector3.Distance(transform.position, _target.position) <=  _minDistanceForAttackTarget)
            {
                Debug.Log("[BurningManAI]: Attacked");
            }
            if (_timeChasing < _maxChaseTime)
                return;
            
            _navMeshAgent.speed = _stalkSpeed;
            _timeChasing = 0;
            Debug.Log("[BurningManAI]: Has stopped chasing");
            _currentState = States.STALKING;
            TeleportToRandomPosition();
        }
    }

    private void TeleportToRandomPosition()
    {
        Debug.Log("[BurningManAI]: Teleported to random position");
        Vector3 randomDirection = GetRandomPositionOnNavMesh();
        
        randomDirection += _mapCenter.position;

        NavMeshHit hit;
    
        if (NavMesh.SamplePosition(randomDirection, out hit, _teleportRadius, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }
    
    
    private void StartChase()
    {
        _navMeshAgent.enabled = true;
        _currentState = States.CHASING;
    }
    private Vector3 GetRandomPositionOnNavMesh()
    {
        return Random.insideUnitSphere * _teleportRadius;
    }
    
    private void GoToPosition(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_mapCenter.position, _teleportRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_target.transform.position, _minDistanceFromTarget);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_target.transform.position, _minDistanceForAttackTarget);
    }
}
