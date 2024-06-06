using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAttackState : BossState
{
    public DeathBringerAttackState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _boss.MovementCompo.StopImmediately(false);

        _boss.CanStateChageable = false;

        _boss.MovementCompo.canMove = false;
        _boss.MovementCompo.canKnockback = false;
    }
    public override void Exit()
    {
        _boss.lastAttackTime = Time.time;
         
        _boss.MovementCompo.canKnockback = true;
        _boss.MovementCompo.canMove = true;

        _boss.SetPatternIndex(1);
        _boss.attackRadius = 10;

        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
        {
            _endTriggerCalled = false;
            _boss.CanStateChageable = true;
            _stateMachine.ChangeState(BossEnum.Chase);
        }
    }
}
