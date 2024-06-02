using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareIdleState : EnemyState
{
    private Collider2D _player;
    public NightmareIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player = null;
        _enemy.HealthCompo.OnHitAction += ChangeChaseState;
        _enemy.MovementCompo.StopImmediately(false);
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.HealthCompo.OnHitAction -= ChangeChaseState;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _player = _enemy.GetPlayerInRange();
        if (_player != null)
        {
            ChangeChaseState();
        }
    }
    private void ChangeChaseState()
    {
        _enemy.targetTrm = GameManager.instance.Player.transform;
        _stateMachine.ChangeState(EnemyEnum.Chase);
    }
}
