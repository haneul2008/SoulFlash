using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGuardianLaserCastState : BossState
{
    public LandGuardianLaserCastState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        _boss.NowAttackAction += Attack;

        _boss.MovementCompo.StopImmediately();

        _boss.CanStateChageable = false;

        _boss.MovementCompo.canMove = false;
        _boss.MovementCompo.canKnockback = false;

        _boss.dontFlip = true;
    }

    public override void Exit()
    {
        base.Exit();

        _boss.NowAttackAction -= Attack;

        _boss.lastAttackTime = Time.time;

        _boss.MovementCompo.canKnockback = true;
        _boss.MovementCompo.canMove = true;

        _boss.dontFlip = false;

        if (Random.Range(1, 4) == 1)
        {
            _boss.SetPatternIndex(2);
            _boss.attackRadius = 100;
        }
        else
        {
            _boss.SetPatternIndex(0);
            _boss.attackRadius = 4.5f;
        }
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

    private void Attack()
    {
        _boss.OnPattern1Event?.Invoke();
    }
}
