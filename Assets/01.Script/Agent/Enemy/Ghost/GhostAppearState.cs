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
