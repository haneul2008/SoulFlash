using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : Enemy, IPoolable
{
    [SerializeField] private GameObject _attackObj;
    [SerializeField] private GameObject _damageCaster;
    [SerializeField] private string _poolName;
    public string PoolName => _poolName;

    public GameObject ObjectPrefab => gameObject;

    public EnemyStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        //시작 상태 추가
        stateMachine.AddState(EnemyEnum.Appear, new GhostAppearState(this, stateMachine, "Appear"));
        stateMachine.AddState(EnemyEnum.Idle, new GhostIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Chase, new GhostChaseState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Attack, new GhostAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(EnemyEnum.Dead, new GhostDeadState(this, stateMachine, "Dead"));

        //시작상태를 설정해서 준비
        stateMachine.Initialize(EnemyEnum.Appear, this);

        lastAttackTime = -9999f;
    }
    private void Update()
    {
        stateMachine.CurrentState.UpdateState();

        if (targetTrm != null && IsDead == false)
        {
            if (Vector2.Distance(new Vector2(targetTrm.position.x, 0), new Vector2(transform.position.x, 0)) > 0.1f)
                HandleSpriteFlip(targetTrm.position, true);
        }
    }
    public override void AnimationEndTrigger()
    {
        stateMachine.CurrentState.AnimationEndTrigger();
    }

    public void ResetItem()
    {
        HealthCompo.ResetHealth(HealthCompo.MaxHealth);
        SetDead(false);
        
        SpriteRendererCompo.color = new Color(1, 1, 1, 0);
        stateMachine.Initialize(EnemyEnum.Appear, this);
        MovementCompo.canKnockback = true;

        lastAttackTime = -9999f;

        gameObject.layer = _enemyLayer;
    }
    public override void Attack(bool castDamage = true)
    {
        _damageCaster.transform.position = new Vector2(targetTrm.position.x, transform.position.y);
        if (Vector2.Distance(transform.position, targetTrm.position) > attackRadius)
            _damageCaster.transform.localPosition = new Vector2(-2f, 0);
        base.Attack(castDamage);

        _attackObj.transform.SetParent(null);
        _attackObj.transform.position = new Vector3(_damageCaster.transform.position.x, -2f);
        _attackObj.SetActive(true);
    }
    public override void SetDeadState()
    {
        gameObject.layer = _deadLayer;
        stateMachine.ChangeState(EnemyEnum.Dead);
    }
}
