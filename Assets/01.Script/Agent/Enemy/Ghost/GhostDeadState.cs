using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDeadState : EnemyState
{
    public GhostDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}
