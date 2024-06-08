using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGuardianChaseState : BossState
{
    public LandGuardianChaseState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }
    public override void UpdateState()
    {
        base.UpdateState();

        Vector2 dir = new Vector2((_boss.targetTrm.position - _boss.transform.position).x, 0);
        float distance = dir.magnitude;

        _boss.MovementCompo.SetMovement(Mathf.Sign(dir.x));

        if (distance < _boss.attackRadius &&
            _boss.lastAttackTime + _boss.PatternCooltime[_boss.NowPattern] < Time.time)
        {

            switch(_boss.NowPattern)
            {
                case 0: _stateMachine.ChangeState(BossEnum.Pattern0);
                    break;

                case 1: _stateMachine.ChangeState(BossEnum.Pattern1);
                    break;

                case 2: _stateMachine.ChangeState(BossEnum.Pattern2);
                    break;
            }
        }
    }
}
