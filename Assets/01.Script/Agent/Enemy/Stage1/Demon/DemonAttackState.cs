using UnityEngine;

public class DemonAttackState : EnemyState
{
    public DemonAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.MovementCompo.StopImmediately(false);
        _enemy.HandleSpriteFlip(_enemy.targetTrm.position, true);

        _enemy.MovementCompo.canMove = false;
        _enemy.MovementCompo.canKnockback = false;
        _enemy.dontFlip = true;

        SoundManager.instance.AddAudioAndPlay(_enemy.attackSound);
    }
    public override void Exit()
    {
        _enemy.lastAttackTime = Time.time;

        _enemy.MovementCompo.canKnockback = true;
        _enemy.MovementCompo.canMove = true;
        _enemy.dontFlip = false;

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
