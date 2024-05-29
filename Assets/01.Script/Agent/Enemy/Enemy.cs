using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : Agent
{
    public UnityEvent FinalDeadEvent;

    [Header("Attack Setting")]
    public float detectRadius;
    public float attackRadius, attackCooldown, knokbackPower;
    public int attackDamage;
    //public LayerMask whatIsPlayer;
    public ContactFilter2D contactFilter;

    [HideInInspector] public float lastAttackTime;
    [HideInInspector] public Transform targetTrm;

    protected int _enemyLayer;
    public bool CanStateChageable { get; protected set; } = true;
    public DamageCaster DamageCasterCompo { get; protected set; }

    private Collider2D[] _colliders;
    protected override void Awake()
    {
        base.Awake();
        DamageCasterCompo = transform.Find("DamageCaster").GetComponent<DamageCaster>();
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        _colliders = new Collider2D[1];
    }
    public Collider2D GetPlayerInRange()
    {
        int count = Physics2D.OverlapCircle(transform.position, detectRadius, contactFilter,
            _colliders);

        return count > 0 ? _colliders[0] : null;
    }

    public abstract void AnimationEndTrigger();

    public void SetDead(bool value)
    {
        IsDead = value;
        CanStateChageable = !value;
    }
    
    public virtual void Attack()
    {
        DamageCasterCompo.CastDamage(attackDamage, knokbackPower, 0.1f, false, true);
    }
#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.white;
    }
#endif
}