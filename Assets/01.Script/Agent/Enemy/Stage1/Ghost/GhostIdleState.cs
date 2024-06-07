using UnityEngine;
public class GhostIdleState : EnemyState
{
    private Collider2D _player;
    public GhostIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        if (_enemy.isSpawnAgent)
        {
            ChangeChaseState();
            return;
        }

        _enemy.HealthCompo.OnHitAction += ChangeChaseState;
        _enemy.MovementCompo.StopImmediately(false);
        _player = null;
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.HealthCompo.OnHitAction -= ChangeChaseState;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_enemy.dontCheckDetect) return;

        _player = _enemy.GetPlayerInRange();
        if (_player != null)
        {
            ChangeChaseState();
        }
    }
    private void ChangeChaseState()
    {
        _enemy.targetTrm = GameManager.instance.Player.transform;
        _stateMachine.ChangeState(EnemyEnum.Chase);
    }
}
