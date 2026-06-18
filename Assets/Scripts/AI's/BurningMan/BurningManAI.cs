using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(WeepingAngleLogic))]
public class BurningManAI : MonoBehaviour
{
    public static bool isHunting { private set; get; } = false;
    [Header("NavMesh")]
    [SerializeField] private NavMeshAgent _navMeshAgent;

    [Header("Dependencies")]
    [SerializeField] private GameObject _spawnCenter;
    [SerializeField] private GameObject _target;
    [SerializeField] private WeepingAngleLogic _weepingAngleLogic;

    [Header("Teleport")]
    [SerializeField] private float _minTeleportRadius = 10f;
    [SerializeField] private float _maxTeleportRadius = 25f;
    [SerializeField] private float _navMeshSampleRadius = 5f;

    [Header("Stalk Stats")]
    [SerializeField] private float _stalkSpeed = 3.5f;
    [SerializeField] private float _minDistanceFromTarget = 5f;
    [SerializeField] private float _maxSecondLookedAt = 5f;
    [SerializeField] private float _lookAwayCooldownRate = 1f;

    [Header("Chase Stats")]
    [SerializeField] private float _chaseSpeed = 8f;
    [SerializeField] private float _maxChaseTime = 10f;
    [SerializeField] private float _minDistanceForAttackTarget = 1.5f;
    [SerializeField] private int _sanityLossOnAttack = 20;
    public enum States
    {
        STALKING,
        CHASING
    }

    [HideInInspector] public States _currentState;

    private float _timeLookedAt;
    private float _timeChasing;
    
    private void Awake()
    {
        if (_navMeshAgent == null)
            _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_weepingAngleLogic == null)
            _weepingAngleLogic = GetComponent<WeepingAngleLogic>();
    }

    private void Start()
    {
        _currentState = States.STALKING;
        _navMeshAgent.speed = _stalkSpeed;

        TeleportToRandomPosition();
    }

    private void Update()
    {
        if (_target == null || _spawnCenter == null)
            return;

        switch (_currentState)
        {
            case States.STALKING:
                HandleStalking();
                break;

            case States.CHASING:
                HandleChasing();
                break;
        }
    }

    private void HandleStalking()
    {
        bool isLookedAt = _weepingAngleLogic.LookedAt();

        if (!isLookedAt)
        {
            _timeLookedAt += Time.deltaTime;
            _navMeshAgent.isStopped = true;

            Debug.Log("[BurningManAI]: Looked at for: " + _timeLookedAt + " seconds");
        }
        else
        {
            _timeLookedAt = Mathf.Max(
                0f,
                _timeLookedAt - Time.deltaTime * _lookAwayCooldownRate
            );

            _navMeshAgent.isStopped = false;
            GoToPosition(_target.transform.position);
        }

        float distanceToTarget =
            Vector3.Distance(transform.position, _target.transform.position);

        if (distanceToTarget <= _minDistanceFromTarget ||
            _timeLookedAt >= _maxSecondLookedAt)
        {
            StartChase();
        }
    }

    private void HandleChasing()
    {
        _timeChasing += Time.deltaTime;

        _navMeshAgent.speed = _chaseSpeed;
        _navMeshAgent.isStopped = false;

        GoToPosition(_target.transform.position);

        Debug.Log("[BurningManAI]: Is chasing you, seconds remaining: " + (_maxChaseTime - _timeChasing) + " seconds");

        float distanceToTarget =
            Vector3.Distance(transform.position, _target.transform.position);

        if (distanceToTarget <= _minDistanceForAttackTarget)
        {
            AudioManager.PlaySound("Jumpscare");
            SanityManager.LoseSanity?.Invoke(_sanityLossOnAttack);
            StopChase();
        }

        if (_timeChasing >= _maxChaseTime)
        {
            StopChase();
        }
    }

    private void StartChase()
    {
        _currentState = States.CHASING;
        _timeChasing = 0f;

        _navMeshAgent.speed = _chaseSpeed;
        _navMeshAgent.isStopped = false;
        isHunting = true;
        AudioManager.PlaySound("BurningManScream");
        VFXManager.StartVignettePulse();
        VFXManager.SetFov(90,1);
        VFXManager.SetInterlacingStrength(0.6f, 2);
        Debug.Log("[BurningManAI]: Started chasing");
    }

    private void StopChase()
    {
        
        VFXManager.StopVignettePulse();
        VFXManager.ResetInterlacingStrength();
        VFXManager.SetFov(60,1);
        _currentState = States.STALKING;
        _timeChasing = 0f;
        _timeLookedAt = 0f;

        _navMeshAgent.speed = _stalkSpeed;
        isHunting = false;
        TeleportToRandomPosition();
        
        Debug.Log("[BurningManAI]: Stopped chasing");
    }

    private void TeleportToRandomPosition()
    {
        if (_spawnCenter == null || _target == null)
            return;

        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0f;

        float distance = Random.Range(_minTeleportRadius, _maxTeleportRadius);

        Vector3 randomPoint =
            _spawnCenter.transform.position +
            randomDirection * distance;

        float distToPlayer =
            Vector3.Distance(randomPoint, _target.transform.position);

        if (distToPlayer < _minTeleportRadius)
        {
            Vector3 away = (randomPoint - _target.transform.position).normalized;
            randomPoint = _target.transform.position + away * _minTeleportRadius;
        }

        if (NavMesh.SamplePosition(
                randomPoint,
                out NavMeshHit hit,
                _navMeshSampleRadius,
                NavMesh.AllAreas))
        {
            _navMeshAgent.Warp(hit.position);
        }
    }

    private void GoToPosition(Vector3 position)
    {
        if (!_navMeshAgent.isOnNavMesh)
            return;

        _navMeshAgent.SetDestination(position);
    }

    private void OnDrawGizmos()
    {
        if (_spawnCenter != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_spawnCenter.transform.position, _maxTeleportRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_spawnCenter.transform.position, _minTeleportRadius);
        }

        if (_target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_target.transform.position, _minDistanceFromTarget);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_target.transform.position, _minDistanceForAttackTarget);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}