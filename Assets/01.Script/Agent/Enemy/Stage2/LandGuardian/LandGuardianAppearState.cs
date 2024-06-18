using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGuardianAppearState : BossState
{
    public LandGuardianAppearState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _boss.isAppear = true;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            _endTriggerCalled = false;

            _boss.OnAppearToIdleEvent?.Invoke();

            _boss.targetTrm = GameManager.instance.Player.transform;
            _stateMachine.ChangeState(BossEnum.Chase);

            _boss.HealthCompo.CanTakeHp(true);
        }
    }
}
