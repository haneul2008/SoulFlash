using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialFrog : Boss
{
    public BossStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new BossStateMachine();

        stateMachine.AddState(BossEnum.Chase, new TutorialFrogChaseState(this, stateMachine, "Chase"));
        stateMachine.AddState(BossEnum.Pattern0, new TutorialFrogAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(BossEnum.Dead, new TutorialFrogDeadState(this, stateMachine, "Dead"));

        lastAttackTime = -999f;

        StartAction += Detect;
    }
    private void Start()
    {
        HealthCompo.CanTakeHp(false);
        AnimatorCompo.SetBool("Idle", true);
    }

    private void OnDisable()
    {
        StartAction -= Detect;
    }
    private void Update()
    {
        if (!isAppear) return;

        stateMachine.CurrentState.UpdateState();

        if (targetTrm != null && IsDead == false)
        {
            if (Vector2.Distance(new Vector2(targetTrm.position.x, 0), new Vector2(transform.position.x, 0)) > 0.1f && !dontFlip)
                HandleSpriteFlip(targetTrm.position, true);
        }
    }
    private void Detect()
    {
        isAppear = true;

        AnimatorCompo.SetBool("Idle", false);

        targetTrm = GameManager.instance.Player.transform;
        stateMachine.Initialize(BossEnum.Chase, this);

        HealthCompo.CanTakeHp(true);
    }
    public override void AnimationEndTrigger()
    {
        stateMachine.CurrentState.AnimationEndTrigger();
    }

    public override void SetDeadState()
    {
        gameObject.layer = _deadLayer;
        stateMachine.ChangeState(BossEnum.Dead);
    }
}
