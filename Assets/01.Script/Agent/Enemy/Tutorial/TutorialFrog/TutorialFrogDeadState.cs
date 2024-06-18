using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFrogDeadState : BossState
{
    public TutorialFrogDeadState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _boss.MovementCompo.StopImmediately();
        _boss.SetDead(true);

        _boss.MovementCompo.canKnockback = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
        {
            _endTriggerCalled = false;
            _boss.FinalDeadEvent?.Invoke();
        }
    }
}