using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    public NavMeshAgent NavMeshAgent;
    public int Price;
    public int Health;
    public GameObject HealthBarPrefab;
    
    private HealthBar _healthbar;
    private int _maxHealth;

    protected override void Start()
    {
        base.Start();
        _maxHealth = Health;
        GameObject healthBar = Instantiate(HealthBarPrefab);
        _healthbar = healthBar.GetComponent<HealthBar>();
        _healthbar.Setup(transform);
    }

    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        NavMeshAgent.SetDestination(point);
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
            FindObjectOfType<Managment>().UnSelect(this);
        }
    }
}
