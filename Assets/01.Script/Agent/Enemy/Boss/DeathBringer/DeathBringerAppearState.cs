using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAppearState : BossState
{
    private bool _appearTrigger;
    public DeathBringerAppearState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(_boss.MovementCompo.isGround.Value && !_appearTrigger)
        {
            _boss.isAppear = true;
            _appearTrigger = true;
        }
    }
}
