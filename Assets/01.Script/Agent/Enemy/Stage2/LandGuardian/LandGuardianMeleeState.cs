using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGuardianMeleeState : BossState
{
    public LandGuardianMeleeState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _boss.NowAttackAction += Attack;

        _boss.MovementCompo.StopImmediately();

        _boss.MovementCompo.canMove = false;
        _boss.MovementCompo.canKnockback = false;
    }

    public override void Exit()
    {
        base.Exit();

        _boss.NowAttackAction -= Attack;

        _boss.lastAttackTime = Time.time;

        _boss.MovementCompo.canKnockback = true;
        _boss.MovementCompo.canMove = true;

        _boss.SetPatternIndex(1);
        _boss.attackRadius = 9;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            _endTriggerCalled = false;

            _stateMachine.ChangeState(BossEnum.Chase);
        }
    }

    private void Attack()
    {
        _boss.DamageCasterCompo.CastDamage(_boss.attackDamage, _boss.knokbackPower, 0.1f, false, true);
        _boss.OnPattern0Event?.Invoke();
    }
}
