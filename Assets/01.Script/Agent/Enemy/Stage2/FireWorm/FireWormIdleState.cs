using UnityEngine;

public class FireWormIdleState : EnemyState
{
    private Collider2D _player;
    public FireWormIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        _enemy.HealthCompo.OnHitAction += ChangeChaseState;
        _enemy.MovementCompo.StopImmediately(false);
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
        _enemy.OnAppearToIdleEvent?.Invoke();

        if (_enemy.dontCheckDetect) _enemy.detectRadius = 150;
        _enemy.targetTrm = GameManager.instance.Player.transform;
        _stateMachine.ChangeState(EnemyEnum.Chase);
    }
}
