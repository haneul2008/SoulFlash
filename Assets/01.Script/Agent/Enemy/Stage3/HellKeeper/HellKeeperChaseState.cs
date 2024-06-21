using UnityEngine;

public class HellKeeperChaseState : BossState
{
    private CameraConfiner _confiner;
    public HellKeeperChaseState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _boss.MovementCompo.StopImmediately();

        if (_confiner == null)
            _confiner = GameManager.instance.virtualCam.GetComponent<CameraConfiner>();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Vector2 dir = new Vector2((_boss.targetTrm.position - _boss.transform.position).x, 0);
        float distance = dir.magnitude;

        _boss.MovementCompo.SetMovement(Mathf.Sign(dir.x));

        _boss.transform.position = new Vector2(Mathf.Clamp(_boss.transform.position.x, Camera.main.transform.position.x -
         _confiner.PlayerClamp, Camera.main.transform.position.x + _confiner.PlayerClamp), _boss.transform.position.y);

        if (distance < _boss.attackRadius &&
            _boss.lastAttackTime + _boss.PatternCooltime[_boss.NowPattern] < Time.time)
        {
            _stateMachine.ChangeState(BossEnum.Pattern0);
        }
    }
}
