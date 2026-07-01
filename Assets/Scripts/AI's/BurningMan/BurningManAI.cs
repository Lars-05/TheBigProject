using System.Collections;
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
    [SerializeField] private GameObject _model;
    [SerializeField] private WeepingAngleLogic _weepingAngleLogic;

    [Header("Teleport")]
    [SerializeField] private float _minTeleportRadius = 10f;
    [SerializeField] private float _maxTeleportRadius = 25f;
    [SerializeField] private float _navMeshSampleRadius = 5f;

    [Header("Animator")]
    [SerializeField] private Animator _animator;
    
    [Header("Animation Clips")]
    [SerializeField] private AnimationClip _walkingAnimation;
    [SerializeField] private AnimationClip _runningAnimation;
    [SerializeField] private AnimationClip _attackingAnimation;
    [SerializeField] private AnimationClip _idleAnimation;
    
    [Header("FX")]
    [SerializeField] private GameObject _fxHolder;
    
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
    [SerializeField] private float _cooldown = 10;

    [Header("VFX")] 
    [SerializeField] private float _cameraShakeRoughness;
    [SerializeField] private float _cameraShakeMagnitude;
    [SerializeField] private float _cameraShakeFadeOut;
    [SerializeField] private float _cameraShakeFadeIn;

    private AudioSource _fireAudioSource;
    private AudioSource _chainAudioSource;
    public enum States
    {
        STALKING,
        CHASING,
        IDLE,
    }

    [HideInInspector] public States _currentState;

    private float _timeLookedAt;
    private float _timeChasing;
    
    private void Awake()
    {
        _fxHolder.SetActive(false);
        if (_navMeshAgent == null)
            _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_weepingAngleLogic == null)
            _weepingAngleLogic = GetComponent<WeepingAngleLogic>();
    }

    private void Start()
    {
        _currentState = States.STALKING;
        _navMeshAgent.speed = _stalkSpeed;
        _fireAudioSource = AudioManager.InterceptSource(this.gameObject);
        _fireAudioSource.loop = true;
        _fireAudioSource.clip = AudioManager.GetAudioClip("OnFire");
        
        _chainAudioSource = AudioManager.InterceptSource(this.gameObject);
        _chainAudioSource.loop = true;
        _chainAudioSource.clip = AudioManager.GetAudioClip("Chains");
        
        TeleportToRandomPosition();
    }

    private void Update()
    {
        if(SanityManager.isDead || GameManager.gameEnded)
            return;
        
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

    public void SetStalkSpeed(float newSpeed)
    {
        _stalkSpeed = newSpeed;
        _navMeshAgent.speed = newSpeed;
    }
    
    private void HandleStalking()
    {
        bool isLookedAt = _weepingAngleLogic.LookedAt();

        if (!isLookedAt)
        {
            _chainAudioSource.Stop();
            _timeLookedAt += Time.deltaTime;
            _navMeshAgent.isStopped = true;
            _animator.Play(_idleAnimation.name);
            Debug.Log("[BurningManAI]: Looked at for: " + _timeLookedAt + " seconds");
        }
        else
        {
            if(!_chainAudioSource.isPlaying)
                _chainAudioSource.Play();
            _animator.Play(_walkingAnimation.name);
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
        _animator.Play(_runningAnimation.name);
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
            AudioManager.PlaySound("Breath");
            StopChase();
        }
    }

    private void StartChase()
    {
        _fireAudioSource.Play();
        _currentState = States.CHASING;
        _timeChasing = 0f;
        
   
        _navMeshAgent.speed = _chaseSpeed;
        _navMeshAgent.isStopped = false;
        isHunting = true;

        StartCoroutine(ChaseFX());
        VFXManager.SetInterlacingStrength(0.6f, 2);
        VFXManager.OneShotScreenShake(_cameraShakeMagnitude, _cameraShakeRoughness, _cameraShakeFadeIn, _cameraShakeFadeOut);
        Debug.Log("[BurningManAI]: Started chasing");
    }

    IEnumerator ChaseFX()
    {
        
        _fxHolder.SetActive(true);
   
        VFXManager.StartScreenShake(_cameraShakeMagnitude,_cameraShakeRoughness,_cameraShakeFadeIn);
        AudioManager.PlaySound("BurningManScream");
        VFXManager.SetFov(90,1);
        VFXManager.StartVignettePulse();
        VFXManager.SetInterlacingStrength(0.6f, 2);
        yield return new WaitForSeconds(AudioManager.GetAudioClip("BurningManScream").length);
        VFXManager.StopScreenShake(_cameraShakeFadeOut);

    }

    private void StopChase()
    {
        _fireAudioSource.Stop();
        _fxHolder.SetActive(false);
        _animator.Play(_idleAnimation.name);
        VFXManager.StopVignettePulse();
        VFXManager.ResetInterlacingStrength();
        VFXManager.SetFov(60,1);
        _currentState = States.STALKING;
        _timeChasing = 0f;
        _timeLookedAt = 0f;

        _navMeshAgent.speed = _stalkSpeed;
        isHunting = false;

        StartCoroutine(OnChaseStopped());
        Debug.Log("[BurningManAI]: Stopped chasing");
    }

    private IEnumerator OnChaseStopped()
    {
        _model.SetActive(false);
        _currentState = States.IDLE;
        yield return new WaitForSeconds(_cooldown);
        _currentState = States.STALKING;
        _model.SetActive(true);
        TeleportToRandomPosition();
        
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
        AudioManager.PlaySound("Bells");
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