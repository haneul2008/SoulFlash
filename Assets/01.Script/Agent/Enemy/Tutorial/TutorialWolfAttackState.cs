using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWolfAttackState : EnemyState
{
    private TutorialWolfEnemy _tutorialWolf;
    private float _attackJumpPower = 3f;

    public TutorialWolfAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        if (_tutorialWolf == null) _tutorialWolf = _enemy.GetComponent<TutorialWolfEnemy>();

        _enemy.MovementCompo.StopImmediately(false);

        Vector2 dir = _enemy.targetTrm.position - _enemy.transform.position;
        dir.y = _attackJumpPower;
        dir.x *= 0.5f;

        _enemy.MovementCompo.JumpTo(dir);
    }

    public override void Exit()
    {
        _enemy.lastAttackTime = Time.time;
        _tutorialWolf.StopPlayer(false);

        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(EnemyEnum.Idle);
        }
    }
}
