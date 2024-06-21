using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutEnemy : Enemy
{
    public EnemyStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        stateMachine.AddState(EnemyEnum.Idle, new SproutIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Chase, new SproutChaseState(this, stateMachine, "Chase"));
        stateMachine.AddState(EnemyEnum.Attack, new SproutAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(EnemyEnum.Dead, new SproutDeadState(this, stateMachine, "Dead"));

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
        CanStateChageable = true;

        gameObject.layer = _deadLayer;
        stateMachine.ChangeState(EnemyEnum.Dead);
    }
}
