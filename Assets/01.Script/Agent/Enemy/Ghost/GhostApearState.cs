using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostApearState : EnemyState
{
    public GhostApearState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
