using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWizardAttackState : EnemyState
{
    public EvilWizardAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        _enemy.MovementCompo.StopImmediately(false);

        _enemy.MovementCompo.canMove = false;
        _enemy.MovementCompo.canKnockback = false;

        _enemy.dontFlip = true;
    }
    public override void Exit()
    {
        _enemy.lastAttackTime = Time.time;

        _enemy.MovementCompo.canKnockback = true;
        _enemy.MovementCompo.canMove = true;

        _enemy.dontFlip = false;

        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            _endTriggerCalled = false;
            _stateMachine.ChangeState(EnemyEnum.Chase);
        }
    }
}
