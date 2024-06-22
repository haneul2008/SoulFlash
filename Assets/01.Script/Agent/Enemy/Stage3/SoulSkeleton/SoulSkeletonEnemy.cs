using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSkeletonenemy : Enemy
{
    public EnemyStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        stateMachine.AddState(EnemyEnum.Idle, new SoulSkeletonIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Chase, new SoulSkeletonChaseState(this, stateMachine, "Chase"));
        stateMachine.AddState(EnemyEnum.Attack, new SoulSkeletonAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(EnemyEnum.Dead, new SoulSkeletonDeadState(this, stateMachine, "Dead"));

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

    public override void Attack(bool castDamage = true)
    {
        base.Attack(castDamage);

        SoundManager.instance.AddAudioAndPlay(attackSound);
    }

    public override void SetDeadState()
    {
        CanStateChageable = true;

        gameObject.layer = _deadLayer;
        stateMachine.ChangeState(EnemyEnum.Dead);
    }
}
