using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HellKeeper : Boss
{
    public UnityEvent OnAttackEvent;
    public BossStateMachine stateMachine;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _appearDelay;
    [SerializeField] private BossHpUi _bossHpUi;
    [SerializeField] private SkillLockUi _skillLockUi;

    [SerializeField] private List<Color> _appearColors;
    [SerializeField] private List<float> _appearMoveSpeeds;
    [SerializeField] private List<int> _appearHealth;
    [SerializeField] private List<int> _appearDamages;
    [SerializeField] private List<float> _appearAttackSpeeds;
    [SerializeField] private Sound _deadSound;
    [SerializeField] private Sound _bossBgm;

    public float AttackSpeed { get; private set; } = 1;
    public Animator AnimationCompo { get; private set; }
    private int _appearCount;
    private bool _isAppearing;
    private Player _player;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new BossStateMachine();

        stateMachine.AddState(BossEnum.Appear, new HellKeeperAppearState(this, stateMachine, "Appear"));
        stateMachine.AddState(BossEnum.Chase, new HellKeeperChaseState(this, stateMachine, "Chase"));
        stateMachine.AddState(BossEnum.Pattern0, new HellKeeperAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(BossEnum.Dead, new HellKeeperDeadState(this, stateMachine, "Dead"));

        lastAttackTime = -999f;

        _bossHpUi.ResetUi(true);

        StartAction += Appear;

        AnimationCompo = transform.Find("Visual").GetComponent<Animator>();

        _player = GameManager.instance.Player.GetComponent<Player>();
    }

    private void Start()
    {
        HealthCompo.CanTakeHp(false);
    }

    private void Appear()
    {
        stateMachine.Initialize(BossEnum.Appear, this);

        _skillLockUi.SetUnlockUi(1, 4, false);
        _player.canBlock = false;

        SoundManager.instance.AddAudioAndPlay(_bossBgm);
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
    public override void AnimationEndTrigger()
    {
        stateMachine.CurrentState.AnimationEndTrigger();
    }

    public override void SetDeadState()
    {
        base.SetDeadState();

        gameObject.layer = _deadLayer;
        stateMachine.ChangeState(BossEnum.Dead);

        SoundManager.instance.AddAudioAndPlay(_deadSound);
        _skillLockUi.SetUnlockUi(1, 4, true);
        _player.canBlock = true;
    }
    public override void Attack(bool castDamage = true)
    {
        base.Attack();
        OnAttackEvent?.Invoke();

        float dir = transform.rotation.eulerAngles.y == 0 ? -1 : 1;

        _particle.transform.position = new Vector2(transform.position.x + 2.15f * dir,
            _particle.transform.position.y);

        _particle.Play();
    }

    public void ReAppear()
    {
        if(_isAppearing) return;

        _isAppearing = true;

        _appearCount++;

        if (_appearCount == 2)
        {
            _bossHpUi.ResetUi(false);
        }

        if (_appearCount == 3)
        {
            SetDeadState();
            return;
        }

        HealthCompo.SetMaxHealth(_appearHealth[_appearCount]);
        HealthCompo.ResetHealth(_appearHealth[_appearCount]);

        attackDamage = _appearDamages[_appearCount];
        SpriteRendererCompo.color = _appearColors[_appearCount];

        MovementCompo.moveSpeed = _appearMoveSpeeds[_appearCount];
        AttackSpeed = _appearAttackSpeeds[_appearCount];

        _bossHpUi.SetUI();

        stateMachine.ChangeState(BossEnum.Appear);

        _isAppearing = false;
    }
}
