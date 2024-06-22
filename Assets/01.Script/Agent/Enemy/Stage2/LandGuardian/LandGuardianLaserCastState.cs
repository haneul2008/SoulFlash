using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGuardianLaserCastState : BossState
{
    private int _recoveryCount;

    public LandGuardianLaserCastState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _boss.NowAttackAction += Attack;

        _boss.MovementCompo.StopImmediately();

        _boss.HealthCompo.CanTakeHp(false);

        _boss.MovementCompo.canMove = false;
        _boss.MovementCompo.canKnockback = false;

        _recoveryCount++;

        SoundManager.instance.AddAudioAndPlay(_boss.PatternSound[1]);
    }

    public override void Exit()
    {
        base.Exit();

        _boss.NowAttackAction -= Attack;

        _boss.lastAttackTime = Time.time;

        _boss.MovementCompo.canKnockback = true;
        _boss.MovementCompo.canMove = true;

        _boss.HealthCompo.CanTakeHp(true);

        if (_recoveryCount >= 2)
        {
            _boss.SetPatternIndex(2);
            _boss.attackRadius = 100;

            _recoveryCount = 0;
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

            _stateMachine.ChangeState(BossEnum.Chase);
        }
    }

    private void Attack()
    {
        _boss.OnPattern1Event?.Invoke();
    }
}
