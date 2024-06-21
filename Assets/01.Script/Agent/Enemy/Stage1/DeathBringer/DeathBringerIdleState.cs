using UnityEngine;

public class DeathBringerIdleState : BossState
{
    private Collider2D _player;

    public DeathBringerIdleState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player = null;
        _boss.HealthCompo.OnHitAction += ChangeChaseState;
        _boss.MovementCompo.StopImmediately(false);
    }

    public override void Exit()
    {
        base.Exit();
        _boss.HealthCompo.OnHitAction -= ChangeChaseState;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _player = _boss.GetPlayerInRange();
        if (_player != null)
        {
            ChangeChaseState();
        }
    }
    private void ChangeChaseState()
    {
        _boss.targetTrm = GameManager.instance.Player.transform;
        _stateMachine.ChangeState(BossEnum.Chase);
    }
}
