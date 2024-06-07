using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathBringer : Boss
{
    public UnityEvent OnAttackEvent;
    public BossStateMachine stateMachine;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _appearDelay;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new BossStateMachine();

        stateMachine.AddState(BossEnum.Appear, new DeathBringerAppearState(this, stateMachine, "Idle"));
        stateMachine.AddState(BossEnum.Idle, new DeathBringerIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(BossEnum.Chase, new DeathBringerChaseState(this, stateMachine, "Chase"));
        stateMachine.AddState(BossEnum.Pattern0, new DeathBringerAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(BossEnum.Pattern1, new DeathBringerSpellCastState(this, stateMachine, "SpellCast"));
        stateMachine.AddState(BossEnum.Dead, new DeathBringerDeadState(this, stateMachine, "Dead"));

        stateMachine.Initialize(BossEnum.Appear, this);

        lastAttackTime = -999f;
    }
    private void Update()
    {
        stateMachine.CurrentState.UpdateState();

        if(isAppear)
        {
            isAppear = false;
            StartCoroutine("AppearToIdle");
            OnAppearEvent?.Invoke();
        }

        if (targetTrm != null && IsDead == false)
        {
            if (Vector2.Distance(new Vector2(targetTrm.position.x, 0), new Vector2(transform.position.x, 0)) > 0.1f)
                HandleSpriteFlip(targetTrm.position, true);
        }
    }
    private IEnumerator AppearToIdle()
    {
        yield return new WaitForSeconds(_appearDelay);
        stateMachine.ChangeState(BossEnum.Idle);
    }
    public override void AnimationEndTrigger()
    {
        stateMachine.CurrentState.AnimationEndTrigger();
    }

    public override void SetDeadState()
    {
        base.SetDeadState();

        gameObject.layer = _deadLayer;
        _particle.Play();
        stateMachine.ChangeState(BossEnum.Dead);
    }
    public override void Attack(bool castDamage = true)
    {
        base.Attack();
        OnAttackEvent?.Invoke();
    }
}