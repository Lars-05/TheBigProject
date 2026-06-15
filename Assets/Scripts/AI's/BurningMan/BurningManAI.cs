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
    [SerializeField] private GameObject _spawnCenter;
    [SerializeField] private GameObject _target;
    [SerializeField] private WeepingAngleLogic _weepingAngleLogic;

    [Header("Stats")]
    [SerializeField] private float _teleportRadius = 25f;
    [SerializeField] private float _lookAwayCooldownRate = 1f;

    [Header("Stalk Stats")]
    [SerializeField] private float _stalkSpeed = 3.5f;
    [SerializeField] private float _minDistanceFromTarget = 5f;
    [SerializeField] private float _maxSecondLookedAt = 5f;

    [Header("Chase Stats")]
    [SerializeField] private float _chaseSpeed = 8f;
    [SerializeField] private float _maxChaseTime = 10f;
    [SerializeField] private float _minDistanceForAttackTarget = 1.5f;

    public enum States
    {
        STALKING,
        CHASING
    }

    [HideInInspector]
    public States _currentState;

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

            if (_navMeshAgent.enabled)
                _navMeshAgent.enabled = false;

            Debug.Log($"[BurningManAI] Looked at for {_timeLookedAt:F2} seconds");
        }
        else
        {
            _timeLookedAt = Mathf.Max(
                0f,
                _timeLookedAt - Time.deltaTime * _lookAwayCooldownRate
            );

            if (!_navMeshAgent.enabled)
                _navMeshAgent.enabled = true;

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

        if (!_navMeshAgent.enabled)
            _navMeshAgent.enabled = true;

        _navMeshAgent.speed = _chaseSpeed;

        GoToPosition(_target.transform.position);

        Debug.Log(
            $"[BurningManAI] Chasing. Time remaining: {(_maxChaseTime - _timeChasing):F2}"
        );

        float distanceToTarget =
            Vector3.Distance(transform.position, _target.transform.position);

        if (distanceToTarget <= _minDistanceForAttackTarget)
        {
            Debug.Log("[BurningManAI] Attacked target");
            // Attack logic here
        }

        if (_timeChasing >= _maxChaseTime)
        {
            StopChase();
        }
    }

    private void StartChase()
    {
        Debug.Log("[BurningManAI] Started chasing");

        _currentState = States.CHASING;
        _timeChasing = 0f;

        if (!_navMeshAgent.enabled)
            _navMeshAgent.enabled = true;

        _navMeshAgent.speed = _chaseSpeed;
    }

    private void StopChase()
    {
        Debug.Log("[BurningManAI] Stopped chasing");

        _currentState = States.STALKING;
        _timeChasing = 0f;
        _timeLookedAt = 0f;

        _navMeshAgent.speed = _stalkSpeed;

        TeleportToRandomPosition();
    }

    private void TeleportToRandomPosition()
    {
        if (_spawnCenter == null)
            return;

        Vector3 randomPoint =
            _spawnCenter.transform.position +
            Random.insideUnitSphere * _teleportRadius;

        if (NavMesh.SamplePosition(
                randomPoint,
                out NavMeshHit hit,
                _teleportRadius,
                NavMesh.AllAreas))
        {
            if (!_navMeshAgent.enabled)
                _navMeshAgent.enabled = true;

            _navMeshAgent.Warp(hit.position);

            Debug.Log("[BurningManAI] Teleported");
        }
    }

    private void GoToPosition(Vector3 position)
    {
        if (!_navMeshAgent.enabled)
            return;

        if (!_navMeshAgent.isOnNavMesh)
            return;

        _navMeshAgent.SetDestination(position);
    }
}