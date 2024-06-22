using System.Collections;
using UnityEngine;

public class LandGuardian : Boss
{
    public BossStateMachine stateMachine;

    [SerializeField] private Sound _deadSound;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new BossStateMachine();

        stateMachine.AddState(BossEnum.Appear, new LandGuardianAppearState(this, stateMachine, "Appear"));
        stateMachine.AddState(BossEnum.Chase, new LandGuardianChaseState(this, stateMachine, "Idle"));
        stateMachine.AddState(BossEnum.Pattern0, new LandGuardianMeleeState(this, stateMachine, "Melee"));
        stateMachine.AddState(BossEnum.Pattern1, new LandGuardianLaserCastState(this, stateMachine, "LaserCast"));
        stateMachine.AddState(BossEnum.Pattern2, new LandGuadianRecoveryState(this, stateMachine, "Recovery"));
        stateMachine.AddState(BossEnum.Dead, new LandGuardianDeadState(this, stateMachine, "Dead"));

        lastAttackTime = -999f;

        StartAction += Initialize;
    }

    private void Start()
    {
        HealthCompo.CanTakeHp(false);
    }
    private void OnDisable()
    {
        StartAction -= Initialize;
    }
    private void Initialize()
    {
        stateMachine.Initialize(BossEnum.Appear, this);
    }
    private void Update()
    {
        if (!isAppear) return;

        stateMachine.CurrentState.UpdateState();

        if (targetTrm != null && IsDead == false)
        {
            if (Vector2.Distance(new Vector2(targetTrm.position.x, 0), new Vector2(transform.position.x, 0)) > 0.05f
                && !dontFlip)
                HandleSpriteFlip(targetTrm.position, true);
        }
    }
    public override void AnimationEndTrigger()
    {
        stateMachine.CurrentState.AnimationEndTrigger();
    }

    public override void SetDeadState()
    {
        base.SetDeadState();

        gameObject.layer = _deadLayer;
        CanStateChageable = true;
        stateMachine.ChangeState(BossEnum.Dead);

        SoundManager.instance.AddAudioAndPlay(_deadSound);
    }
}
