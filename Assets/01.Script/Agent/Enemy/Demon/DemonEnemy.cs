using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonEnemy : Enemy
{
    public EnemyStateMachine stateMachine;
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        stateMachine.AddState(EnemyEnum.Idle, new DemonIdleState(this, stateMachine, "Idle"));

        stateMachine.Initialize(EnemyEnum.Idle, this);

        lastAttackTime = -999f;
    }
    private void Update()
    {
        stateMachine.CurrentState.UpdateState();

        if (targetTrm != null && IsDead == false)
        {
            if (Vector2.Distance(targetTrm.position, transform.position) > 0.8f)
                HandleSpriteFlip(targetTrm.position, true);
        }
    }
    public override void AnimationEndTrigger()
    {
    }

    public override void SetDeadState()
    {
    }
}
