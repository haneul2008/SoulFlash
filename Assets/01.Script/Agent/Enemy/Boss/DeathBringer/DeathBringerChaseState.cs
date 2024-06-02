using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DeathBringerChaseState : BossState
{
    public DeathBringerChaseState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Vector2 dir = new Vector2((_boss.targetTrm.position - _boss.transform.position).x, 0);
        float distance = dir.magnitude;
        if (distance > _boss.detectRadius + 4f)
        {
            _stateMachine.ChangeState(BossEnum.Idle);
            return;
        }

        _boss.MovementCompo.SetMovement(Mathf.Sign(dir.x));

        if (distance < _boss.attackRadius &&
            _boss.lastAttackTime + _boss.PatternCooltime[_boss.NowPattern] < Time.time)
        { 
            if(_boss.NowPattern == 0)
                _stateMachine.ChangeState(BossEnum.Pattern0);

            else if (_boss.NowPattern == 1)
                _stateMachine.ChangeState(BossEnum.Pattern1);

            return;
        }
    }
}
