using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}

public class Enemy : MonoBehaviour
{
    public EnemyState CurrentEnemyState;
    public int Health;
    public Building TargetBuilding;
    public Unit TargetUnit;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;
    public NavMeshAgent NavMeshAgent;
    public float AttackPeriod = 1f;
    public GameObject HealthBarPrefab;
    
    private HealthBar _healthbar;
    private int _maxHealth;
    private float _timer;

    private void Start()
    {
        SetState(EnemyState.WalkToBuilding);
        _maxHealth = Health;
        GameObject healthBar = Instantiate(HealthBarPrefab);
        _healthbar = healthBar.GetComponent<HealthBar>();
        _healthbar.Setup(transform);
    }
    
    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthbar.SetHealth(Health, _maxHealth);
        if (Health <= 0)
        {
            //dead
            Destroy(gameObject);
            _healthbar.OnDeath();
        }
    }

    private void Update()
    {
        if (CurrentEnemyState == EnemyState.Idle)
        {
            FindClosestBuildings();
            if (TargetBuilding)
            {
                SetState(EnemyState.WalkToBuilding);
            }
            FindClosestUnit();
        }
        else if (CurrentEnemyState == EnemyState.WalkToBuilding) 
        {
            FindClosestUnit();
            if (!TargetBuilding)
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {
            if (TargetUnit)
            {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToFollow)
                {
                    SetState(EnemyState.WalkToBuilding);
                }

                if (distance < DistanceToAttack)
                {
                    SetState(EnemyState.Attack);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            if (TargetUnit)
            {
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToAttack)
                {
                    SetState(EnemyState.WalkToUnit);
                }

                _timer += Time.deltaTime;
                if (_timer > AttackPeriod)
                {
                    _timer = 0f;
                    //hit 
                    TargetUnit.TakeDamage(1);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
    }

    public void SetState(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;
        if (CurrentEnemyState == EnemyState.Idle)
        {
            
        }
        else if (CurrentEnemyState == EnemyState.WalkToBuilding) 
        {
            FindClosestBuildings();
            if (TargetBuilding)
            {
                NavMeshAgent.SetDestination(TargetBuilding.transform.position);
            }
            else
            {
                SetState(EnemyState.Idle);
            }
            
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {
            
        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            _timer = 0;
        }
    }

    public void FindClosestBuildings()
    {
        Building[] allBuilding = FindObjectsOfType<Building>();

        float minDistance = Mathf.Infinity;
        Building closestBuilding = null;
        
        for (int i = 0; i < allBuilding.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allBuilding[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = allBuilding[i];
            }
        }

        TargetBuilding = closestBuilding;
    }
    
    public void FindClosestUnit()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();

        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;
        
        for (int i = 0; i < allUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestUnit = allUnits[i];
            }
        }
        if (minDistance < DistanceToFollow)
        {
            TargetUnit = closestUnit;
            SetState(EnemyState.WalkToUnit);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToFollow);
    }
#endif
}
