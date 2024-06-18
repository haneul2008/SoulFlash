using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWolfDeadState : EnemyState
{
    public TutorialWolfDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _enemy.MovementCompo.StopImmediately();
        _enemy.SetDead(true);

        _enemy.MovementCompo.canKnockback = false;

        GameManager.instance.OnEnemyFinalDeadAction?.Invoke();
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
    }
}
