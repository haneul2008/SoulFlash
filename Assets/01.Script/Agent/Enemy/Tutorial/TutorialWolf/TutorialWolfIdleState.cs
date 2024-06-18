using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialWolfIdleState : EnemyState
{
    private Collider2D _player;
    private TutorialWolfEnemy _tutorialWolf;
    public TutorialWolfIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        
        if(_tutorialWolf == null) _tutorialWolf = _enemy.GetComponent<TutorialWolfEnemy>();

        _enemy.HealthCompo.OnHitAction += ChangeAttackState;
        _enemy.MovementCompo.StopImmediately(false);
    }

    public override void Exit()
    {
        base.Exit();

        _tutorialWolf.StopPlayer(true);
        _enemy.HealthCompo.OnHitAction -= ChangeAttackState;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_enemy.dontCheckDetect) return;
        
        _player = _enemy.GetPlayerInRange();
        if (_player != null)
        {
            ChangeAttackState();
        }
    }
    private void ChangeAttackState()
    {
        _enemy.targetTrm = GameManager.instance.Player.transform;

        Vector2 dir = new Vector2((_enemy.targetTrm.position - _enemy.transform.position).x, 0);
        float distance = dir.magnitude;

        if (distance < _enemy.attackRadius &&
            _enemy.lastAttackTime + _enemy.attackCooldown < Time.time && _enemy.MovementCompo.isGround.Value)
        _stateMachine.ChangeState(EnemyEnum.Attack);
    }
}
