using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonEnemy : Enemy, IPoolable
{
    [SerializeField] private string _poolName;

    public EnemyStateMachine stateMachine;

    public string PoolName => _poolName;

    public GameObject ObjectPrefab => gameObject;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        stateMachine.AddState(EnemyEnum.Idle, new DemonIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Chase, new DemonChaseState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Attack, new DemonAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(EnemyEnum.Dead, new DemonDeadState(this, stateMachine, "Dead"));

        stateMachine.Initialize(EnemyEnum.Idle, this);

        lastAttackTime = -999f;
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

    public override void SetDeadState()
    {
        gameObject.layer = _deadLayer;
        stateMachine.ChangeState(EnemyEnum.Dead);
    }

    public void ResetItem()
    {
        MovementCompo.rbCompo.gravityScale = 1;
        HealthCompo.CanTakeHp(true);
        MovementCompo.canKnockback = true;

        HealthCompo.ResetHealth(HealthCompo.MaxHealth);
        SetDead(false);

        SpriteRendererCompo.color = new Color(1, 1, 1, 1);
        stateMachine.Initialize(EnemyEnum.Idle, this);

        lastAttackTime = -9999f;
        
        gameObject.layer = _enemyLayer;
    }
}
