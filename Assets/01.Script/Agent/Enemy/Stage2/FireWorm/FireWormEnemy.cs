using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWormEnemy : Enemy
{
    [SerializeField] private Vector2 _fireballPos;

    public EnemyStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        stateMachine.AddState(EnemyEnum.Idle, new FireWormIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Chase, new FireWormChaseState(this, stateMachine, "Chase"));
        stateMachine.AddState(EnemyEnum.Attack, new FireWormAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(EnemyEnum.Dead, new FireWormDeadState(this, stateMachine, "Dead"));

        stateMachine.Initialize(EnemyEnum.Idle, this);

        lastAttackTime = -9999;
    }
    private void Update()
    {
        stateMachine.CurrentState.UpdateState();

        if (targetTrm != null && IsDead == false)
        {
            if (Vector2.Distance(new Vector2(targetTrm.position.x, 0), new Vector2(transform.position.x, 0)) > 0.1f && !dontFlip)
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
    public override void Attack(bool castDamage = true)
    {
        base.Attack(castDamage);

        FireWormFireball fireball = PoolManager.instance.Pop("Fireball") as FireWormFireball;

        float dir = transform.rotation.eulerAngles.y == 0 ? -1 : 1;

        fireball.transform.position = new Vector2(transform.position.x + _fireballPos.x * -dir, transform.position.y + _fireballPos.y);
        fireball.Initalize(dir, attackDamage, knokbackPower);
    }
}
