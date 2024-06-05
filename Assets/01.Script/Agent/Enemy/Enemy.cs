using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyEnum
{
    Appear,
    Idle,
    Chase,
    Attack,
    Dead
}
public abstract class Enemy : Agent
{
    public UnityEvent OnAppearToIdleEvent;
    public UnityEvent FinalDeadEvent;

    [SerializeField] private bool _crystalSpawn = true;
    [SerializeField] private float _crystalSpawnTime;
    public bool dontCheckDetect;

    [Header("Attack Setting")]
    public float detectRadius;
    public float attackRadius, attackCooldown, knokbackPower;
    public int attackDamage;
    public ContactFilter2D contactFilter;

    [HideInInspector] public float lastAttackTime;
    [HideInInspector] public Transform targetTrm;

    protected int _enemyLayer;
    protected int _deadLayer;
    public bool CanStateChageable { get; set; } = true;
    public DamageCaster DamageCasterCompo { get; protected set; }

    private Collider2D[] _colliders;
    protected override void Awake()
    {
        base.Awake();
        DamageCasterCompo = transform.Find("DamageCaster").GetComponent<DamageCaster>();
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        _deadLayer = LayerMask.NameToLayer("DeathEnemy");
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
        dontCheckDetect = false;
    }
    public void SpawnCrystal()
    {
        if(!_crystalSpawn) return;

        if (isSpawnAgent)
        {
            isSpawnAgent = false;
            return;
        }

            Crystal crystal = PoolManager.instance.Pop("Crystal") as Crystal;
        crystal.gameObject.transform.position = transform.position;
        crystal.SetCrystalSpawnTime(_crystalSpawnTime);
    }
    
    public virtual void Attack(bool castDamage = true)
    {
        if (!castDamage) return;
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