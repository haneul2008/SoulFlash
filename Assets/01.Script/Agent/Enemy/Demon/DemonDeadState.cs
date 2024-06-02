using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDeadState : EnemyState
{
    public DemonDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Dead");
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
        Debug.Log(_enemy.gameObject.layer);
        _enemy.FinalDeadEvent?.Invoke();
        GameManager.instance.OnEnemyDeadAction?.Invoke();
    }
}
