using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareDeadState : EnemyState
{
    public NightmareDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.MovementCompo.StopImmediately();
        _enemy.SetDead(true);

        _enemy.MovementCompo.canKnockback = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
        {
            _endTriggerCalled = false;
            PlayFinalDead();
        }
    }
    private void PlayFinalDead()
    {
        _enemy.FinalDeadEvent?.Invoke();
        GameManager.instance.OnEnemyFinalDeadAction?.Invoke();
    }
}
