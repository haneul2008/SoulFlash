using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChaseState : EnemyState
{
    public GhostChaseState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Vector2 dir = new Vector2((_enemy.targetTrm.position - _enemy.transform.position).x, 0);
        float distance = dir.magnitude;
        if (distance > _enemy.detectRadius + 4f)
        {
            _stateMachine.ChangeState(EnemyEnum.Idle);
            return;
        }
       
        _enemy.MovementCompo.SetMovement(Mathf.Sign(dir.x));

        if (distance < _enemy.attackRadius &&
            _enemy.lastAttackTime + _enemy.attackCooldown < Time.time)
        {
            _stateMachine.ChangeState(EnemyEnum.Attack);
            return;
        }
    }
}
