using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DeathBringerChaseState : BossState
{
    private CameraConfiner _confiner;
    public DeathBringerChaseState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(_confiner == null)
            _confiner = GameManager.instance.virtualCam.GetComponent<CameraConfiner>();
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

        _boss.transform.position = new Vector2(Mathf.Clamp(_boss.transform.position.x, Camera.main.transform.position.x - 
            _confiner.PlayerClamp, Camera.main.transform.position.x + _confiner.PlayerClamp), _boss.transform.position.y);

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
