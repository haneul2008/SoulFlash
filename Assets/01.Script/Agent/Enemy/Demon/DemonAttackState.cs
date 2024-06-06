using UnityEngine;

public class DemonAttackState : EnemyState
{
    public DemonAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.MovementCompo.rbCompo.gravityScale = 0;
        _enemy.transform.position = new Vector2(_enemy.transform.position.x, _enemy.transform.position.y + 0.4f);

        _enemy.MovementCompo.StopImmediately(false);

        _enemy.MovementCompo.canMove = false;
        _enemy.MovementCompo.canKnockback = false;
    }
    public override void Exit()
    {
        _enemy.MovementCompo.rbCompo.gravityScale = 1;

        _enemy.lastAttackTime = Time.time;

        _enemy.MovementCompo.canKnockback = true;
        _enemy.MovementCompo.canMove = true;

        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(EnemyEnum.Chase);
        }
    }
}
