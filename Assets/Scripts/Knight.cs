using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum UnitState
{
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack
}

public class Knight : Unit
{
    public UnitState CurrentUnitState;
    public Vector3 TargetPoint;
    public Enemy EnemyTarget;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;
    public float AttackPeriod = 1f;

    private float _timer;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (CurrentUnitState == UnitState.Idle)
        {
            FindClosestEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToPoint) 
        {
            FindClosestEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {
            if (EnemyTarget)
            {
                NavMeshAgent.SetDestination(EnemyTarget.transform.position);
                float distance = Vector3.Distance(transform.position, EnemyTarget.transform.position);
                if (distance > DistanceToFollow)
                {
                    SetState(UnitState.WalkToPoint);
                }

                if (distance < DistanceToAttack)
                {
                    SetState(UnitState.Attack);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }
        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            if (EnemyTarget)
            {
                float distance = Vector3.Distance(transform.position, EnemyTarget.transform.position);
                if (distance > DistanceToAttack)
                {
                    SetState(UnitState.WalkToEnemy);
                }

                _timer += Time.deltaTime;
                if (_timer > AttackPeriod)
                {
                    _timer = 0f;
                    //hit 
                    EnemyTarget.TakeDamage(1);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }
        }
    }

    public void SetState(UnitState unitState)
    {
        CurrentUnitState = unitState;
        if (CurrentUnitState == UnitState.Idle)
        {
            
        }
        else if (CurrentUnitState == UnitState.WalkToPoint) 
        {

        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {
            
        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            _timer = 0;
        }
    }

    public void FindClosestEnemy()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        float minDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        
        for (int i = 0; i < allEnemies.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allEnemies[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = allEnemies[i];
            }
        }
        if (minDistance < DistanceToFollow)
        {
            EnemyTarget = closestEnemy;
            SetState(UnitState.WalkToEnemy);
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
