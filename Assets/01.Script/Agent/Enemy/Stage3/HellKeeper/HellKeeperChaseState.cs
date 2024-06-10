using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HellKeeperChaseState : BossState
{
    public HellKeeperChaseState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _boss.MovementCompo.StopImmediately();
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
            _stateMachine.ChangeState(BossEnum.Pattern0);
        }
    }
}
