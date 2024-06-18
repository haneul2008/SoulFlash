using DG.Tweening;
using UnityEngine;
public class GhostAppearState : EnemyState
{
    public GhostAppearState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        _endTriggerCalled = false;
        base.Enter();
        _enemy.SpriteRendererCompo.DOFade(1, 0.2f);

        _enemy.HealthCompo.CanTakeHp(false);
        _enemy.MovementCompo.canKnockback = false;
    }

    public override void Exit()
    {
        base.Exit();

        _enemy.HealthCompo.CanTakeHp(true);
        _enemy.MovementCompo.canKnockback = true;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
        {
            _endTriggerCalled = false;

            _enemy.OnAppearToIdleEvent?.Invoke();
            _stateMachine.ChangeState(EnemyEnum.Idle);
        }
    }
}
