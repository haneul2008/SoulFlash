using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDeadState : EnemyState
{
    private bool _isSpawnAgent;
    public GhostDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.MovementCompo.StopImmediately();
        _enemy.SetDead(true);

        _isSpawnAgent = _enemy.isSpawnAgent;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            PlayFinalDead();
        }
    }

    private void PlayFinalDead()
    {
        _enemy.FinalDeadEvent?.Invoke();

        IPoolable iPoolable = _enemy.GetComponent<IPoolable>();
        if (iPoolable != null)
        {
            PoolManager.instance.Push(iPoolable);
        }
        else
        {
            GameObject.Destroy(_enemy.gameObject);
        }

        if (_isSpawnAgent) return;
        GameManager.instance.OnEnemyDeadAction?.Invoke();
    }
}
