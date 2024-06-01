using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDeadState : EnemyState
{
    private readonly int _deadLayer = LayerMask.NameToLayer("DeathEnemy");
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
        _enemy.gameObject.layer = _deadLayer;
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
        GameManager.instance.OnEnemyDeadAction?.Invoke();
    }
}
