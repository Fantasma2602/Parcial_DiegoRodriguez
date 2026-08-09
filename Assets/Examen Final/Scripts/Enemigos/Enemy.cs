using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Datos del enemigo")]
    [SerializeField] private EnemyProfile enemyProfile;

    [Header("Objetivo")]
    [SerializeField] private Transform target;

    [Header("Estado")]
    [SerializeField] private EnemyState currentState = EnemyState.Idle;

    private int currentHealth;

    public event Action<Enemy> OnEnemyDied;
    public event Action<Enemy> OnEnemyReachedTarget;

    private void Start()
    {
        InitializeEnemy();
    }

    private void Update()
    {
        UpdateState();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    public void Initialize(EnemyProfile profile, Transform enemyTarget)
    {
        enemyProfile = profile;
        target = enemyTarget;

        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        if (enemyProfile == null)
        {
            Debug.LogError("El enemigo no tiene un EnemyProfile asignado.", this);
            return;
        }

        currentHealth = enemyProfile.health;

        ChangeState(EnemyState.Walking);
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;

            case EnemyState.Walking:
                HandleWalking();
                break;

            case EnemyState.Attacking:
                HandleAttacking();
                break;

            case EnemyState.Dead:
                HandleDead();
                break;
        }
    }

    private void HandleIdle()
    {
        // El enemigo permanece quieto.
    }

    private void HandleWalking()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= 1.5f)
        {
            ReachTarget();
            return;
        }

        transform.position += direction.normalized *
                              enemyProfile.speed *
                              Time.deltaTime;
    }

    private void HandleAttacking()
    {
        // Posteriormente agregaremos aquí el ataque.
    }

    private void HandleDead()
    {
        // El enemigo ya murió.
    }

    public void TakeDamage(int damage)
    {
        if (currentState == EnemyState.Dead)
            return;

        currentHealth -= damage;

        Debug.Log(
            $"{enemyProfile.enemyName} recibió {damage} de daño. Vida restante: {currentHealth}"
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        currentHealth = 0;

        ChangeState(EnemyState.Dead);

        OnEnemyDied?.Invoke(this);

        Destroy(gameObject);
    }

    private void ReachTarget()
    {
        ChangeState(EnemyState.Attacking);

        OnEnemyReachedTarget?.Invoke(this);
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        if (enemyProfile != null)
        {
            Debug.Log(
                $"{enemyProfile.enemyName} cambió al estado: {currentState}"
            );
        }
    }

    public EnemyState GetCurrentState()
    {
        return currentState;
    }
}