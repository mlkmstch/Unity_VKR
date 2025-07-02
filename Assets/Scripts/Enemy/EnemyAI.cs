using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public int enemyDamage;

    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax;
    [SerializeField] private float _roamimgDistanceMin;
    [SerializeField] private float _roamimgTimerMax;

    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _chasingDistance;
    [SerializeField] private float _roamingSpeed;
    [SerializeField] private float _chasingSpeed;

    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _attackingDistance;
    [SerializeField] private float _attackRate;

    [SerializeField] private float _meleeAttackDistance = 5f;
    [SerializeField] private float _rangedAttackDistance = 15f;
    [SerializeField] private float stoppingDistanceFromPlayer = 1.2f;
    private float _nextAttackTime = 0f;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;


    private float _nextCheckDirectionTime = 0f;
    private float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition;
    public static float _currentHealthasPercent;
    [SerializeField] private bool rangedEnemy = false;
    public event EventHandler Attack;

    public bool IsRunning
    {
        get
        {
            if (_navMeshAgent.velocity == Vector3.zero)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }

    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = _roamimgTimerMax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                break;
        }
    }

    private void ChasingTarget()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (distanceToPlayer > stoppingDistanceFromPlayer)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(Player.Instance.transform.position);
        }
        else
        {
            _navMeshAgent.isStopped = true;
        }
    }

    public float GetRoamingAnimationSpeed()
    {
        return _roamingSpeed;
    }

    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (_isChasingEnemy)
        {
            if (distanceToPlayer <= _chasingDistance)
            {
                newState = State.Chasing;
            }
        }

        if (_isAttackingEnemy)
        {
            float attackRange = rangedEnemy ? _rangedAttackDistance : _meleeAttackDistance;

            if (distanceToPlayer <= attackRange)
            {
                newState = State.Attacking;
            }
        }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                _roamingTimer = 0f;
                _navMeshAgent.speed = _roamingSpeed;
            }
            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
            }

            _currentState = newState;
        }
    }

    private void AttackingTarget()
    {
        if (rangedEnemy)
        {
            if (Time.time > _nextAttackTime)
            {
                Attack?.Invoke(this, EventArgs.Empty);

                GetComponent<EnemyRangedAttack>()?.FireAtPlayer();

                _nextAttackTime = Time.time + _attackRate;
            }
        }
        else
        {
            if (Time.time > _nextAttackTime)
            {
                Attack?.Invoke(this, EventArgs.Empty);
            }
        }
        
    }

    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRunning)
            {
                ChangeFacingDirection(_lastPosition, transform.position);
            }
            else if (_currentState == State.Attacking)
            {
                ChangeFacingDirection(transform.position, Player.Instance.transform.position);
            }

            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
    }

    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        Vector3 tempVector = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        return _startingPosition + tempVector * UnityEngine.Random.Range(_roamimgDistanceMin, _roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    
}

