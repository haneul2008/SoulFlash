using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGuadianRecoveryState : BossState
{
    private bool _canRecovery;
    private float _recoveryAmount;

    int _saveHp;
    public LandGuadianRecoveryState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _boss.NowAttackAction += Recovery;

        _boss.MovementCompo.StopImmediately();

        _boss.MovementCompo.canMove = false;
        _boss.MovementCompo.canKnockback = false;

        _boss.OnPattern3Event?.Invoke();

        _boss.dontFlip = true;

        _saveHp = _boss.HealthCompo.CurrentHealth;
    }

    public override void Exit()
    {
        base.Exit();

        _boss.NowAttackAction -= Recovery;

        _boss.lastAttackTime = Time.time;

        _boss.MovementCompo.canKnockback = true;
        _boss.MovementCompo.canMove = true;

        _boss.HealthCompo.CanTakeHp(true);

        _boss.SetPatternIndex(0);
        _boss.attackRadius = 4.5f;

        _boss.dontFlip = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!_canRecovery) return;

        _recoveryAmount += Time.deltaTime * 100f;

        if(_recoveryAmount >= 0.1f && _boss.HealthCompo.CurrentHealth < _saveHp + 100)
        {
            _recoveryAmount = 0;
            _boss.HealthCompo.ResetHealth(_boss.HealthCompo.CurrentHealth + 1, false);
            _boss.HealthCompo.OnHitAction?.Invoke();
        }

        if(_endTriggerCalled)
        {
            _endTriggerCalled = false;
            _canRecovery = false;

            _stateMachine.ChangeState(BossEnum.Chase);
        }
    }

    private void Recovery()
    {
        _canRecovery = true;

        _boss.OnPattern2Event?.Invoke();
    }
}
