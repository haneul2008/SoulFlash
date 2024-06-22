using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellKeeperAppearState : BossState
{
    public HellKeeperAppearState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _boss.MovementCompo.canKnockback = false;
        _boss.HealthCompo.CanTakeHp(false);

        _boss.isAppear = true;
        _boss.dontFlip = true;

        _boss.OnAppearToIdleEvent?.Invoke();
    }

    public override void Exit()
    {
        base.Exit();

        _boss.MovementCompo.canKnockback = true;
        _boss.HealthCompo.CanTakeHp(true);

        _boss.dontFlip = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            _endTriggerCalled = false;

            _boss.targetTrm = GameManager.instance.Player.transform;
            _stateMachine.ChangeState(BossEnum.Chase);
        }
    }
}
