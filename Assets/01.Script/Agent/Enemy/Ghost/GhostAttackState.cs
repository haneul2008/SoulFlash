using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAttackState : EnemyState
{
    public GhostAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.MovementCompo.StopImmediately(false);
        _enemy.MovementCompo.canKnockback = false;
    }

    public override void Exit()
    {
        _enemy.lastAttackTime = Time.time;
        _enemy.MovementCompo.canKnockback = true;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(EnemyEnum.Chase);
        }
    }
}
