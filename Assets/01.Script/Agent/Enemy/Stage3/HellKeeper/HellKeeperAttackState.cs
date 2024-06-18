using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellKeeperAttackState : BossState
{
    private Animator _anim;
    private HellKeeper _hellKeeper;
    public HellKeeperAttackState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        _boss.MovementCompo.StopImmediately(false);

        _boss.CanStateChageable = false;

        _boss.MovementCompo.canMove = false;
        _boss.MovementCompo.canKnockback = false;

        _boss.HealthCompo.CanTakeHp(false);

        _boss.dontFlip = true;

        if(_anim == null)
        {
            _hellKeeper = _boss as HellKeeper;
            _anim = _hellKeeper.AnimationCompo;
        }

        _anim.speed = _hellKeeper.AttackSpeed;
    }
    public override void Exit()
    {
        _boss.lastAttackTime = Time.time;

        _boss.MovementCompo.canKnockback = true;
        _boss.MovementCompo.canMove = true;

        _boss.HealthCompo.CanTakeHp(true);

        _boss.SetPatternIndex(0);

        _boss.dontFlip = false;

        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
        {
            _endTriggerCalled = false;
            _boss.CanStateChageable = true;

            _anim.speed = 1;

            _stateMachine.ChangeState(BossEnum.Chase);
        }
    }
}
