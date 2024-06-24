using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class Player : Agent
{
    public event Action OnPlayerDeadAction;

    public UnityEvent JumpEvent;
    [SerializeField] private DamageCaster _damageCaster;
    [SerializeField] private Vector2 _damageCasterPos;
    [SerializeField] private int _damage;
    [SerializeField] private float _knockbackPower;
    [SerializeField] private float _hpRetakeTime;
    [SerializeField] private float _damageCasterRadius;
    [SerializeField] private PlayerSmoke _smoke;
    [SerializeField] private Vector2 _deadColliderSize;
    [SerializeField] private Sound _deadSound;

    [field: SerializeField] public InputReader PlayerInput { get; private set; }

    private bool _canDoubleJump;
    public bool CanStateChageable { get; set; } = true;
    public float PlayerDir { get; private set; } = 1;
    [HideInInspector] public bool animationEndTrigger;

    #region ClampSkill
    //x[HideInInspector] 
    public bool canJump = true;
    [HideInInspector] public bool canAirDash = true;
    [HideInInspector] public bool canRoll = true;
    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public bool canHeavyAttack = true;
    [HideInInspector] public bool canAirAttack = true;
    [HideInInspector] public bool canBlock = true;
    #endregion

    private CameraConfiner _cameraConfiner;
    private GameObject _light;
    private SizeChanger _deadSizeChanger = new SizeChanger();
    #region Component
    private SpriteMask _spriteNask;
    private SpriteRenderer _spriteRenderer;
    private PlayerJumpAnimation _jumpAnimation;
    private PlayerAirAttack _airAttack;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        PlayerInput.JumpEvent += HandleJumpKeyEvent;

        _spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();

        _jumpAnimation = GetComponent<PlayerJumpAnimation>();
        _airAttack = GetComponent<PlayerAirAttack>();

        InitPlayerActions();

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += HandleSetPlayer;

        _cameraConfiner = GameManager.instance.virtualCam.GetComponent<CameraConfiner>();
        _light = transform.Find("PlayerLight").gameObject;
    }

    private void HandleSetPlayer(Scene scene, LoadSceneMode mode)
    {
        transform.position = new Vector3(0, -3.5f, 0);

        _spriteRenderer.color = new Color(1, 1, 1, 0);
        _smoke.PlayAnimation(true);

        float hpMultiplier = GameManager.instance.hpMultiplier + GameManager.instance.passiveHpInc;

        int resetHp = scene.name == "Lobby" ? HealthCompo.MaxHealth : Mathf.RoundToInt(HealthCompo.MaxHealth * hpMultiplier);
        HealthCompo.ResetHealth(resetHp);

        _light.SetActive(true);

        MovementCompo.rbCompo.gravityScale = 1;

        _cameraConfiner.SetConfiner(false);
    }

    private void OnDisable()
    {
        PlayerInput.JumpEvent -= HandleJumpKeyEvent;

        if (_smoke.Tween != null)
            _smoke.Tween.Kill();
    }
    private void Flip()
    {
        if (!MovementCompo.canMove || dontFlip) return;

        if (Mathf.Abs(PlayerInput.Movement.x) > 0.1f)
        {
            SetDir(AttackMode.Mix, PlayerInput.Movement.x > 0.1f);
            SetDir(AttackMode.Keyboard, PlayerInput.Movement.x > 0.1f);

            float rotationY = PlayerInput.Movement.x > 0.1f ? 0 : -180f;
            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }
        else
        {
            Vector2 mousePos = Input.mousePosition;

            SetDir(AttackMode.Mix, Camera.main.ScreenToWorldPoint(mousePos).x > transform.position.x);

            if(GameManager.instance.AttackMode != AttackMode.Keyboard) HandleSpriteFlip(Camera.main.ScreenToWorldPoint(mousePos));
        }
    }

    private void InitPlayerActions()
    {
        List<AnimationPlayer> actionList = GetComponents<AnimationPlayer>().ToList();
        actionList.ForEach(action => action.Initialize(this));
    }

    private void Update()
    {
        if (IsDead)
        {
            if (CanStateChageable) CanStateChageable = false;
            return;
        }

        MovementCompo.SetMovement(PlayerInput.Movement.x);
        SetDir(AttackMode.Mouse, Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x);

        Flip();
        _airAttack.SetAirState(!MovementCompo.isGround.Value);
    }

    private void HandleJumpKeyEvent()
    {
        if (!canJump) return;

        if (MovementCompo.isGround.Value)
        {
            JumpProcess(true);
        }
        else if (_canDoubleJump)
        {
            JumpProcess(false);
        }
    }
    private void JumpProcess(bool canDoubleJump)
    {
        if (!CanStateChageable || !canJump) return;
        _canDoubleJump = canDoubleJump;
        JumpEvent?.Invoke();
        MovementCompo.Jump();
        StartCoroutine(_jumpAnimation.Jump());
    }
    public override void SetDeadState()
    {
    }
    public void Attack()
    {
        float dir;
        dir = transform.rotation.eulerAngles.y == 0 ? 1 : -1;

        _damageCaster.gameObject.transform.position = new Vector3(transform.position.x + dir * _damageCasterPos.x, transform.position.y);
        _damageCaster.damageRadius = _damageCasterRadius;

        _damageCaster.CastDamage(Mathf.RoundToInt(_damage * GameManager.instance.normalDamageMultiplier)
            , _knockbackPower, _hpRetakeTime, false);
    }
    public void AnimationEndTrigger()
    {
        animationEndTrigger = true;
    }

    public void PlayerDead(bool value)
    {
        if(IsDead && value) return;
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            HealthCompo.ResetHealth(HealthCompo.MaxHealth);
            HealthCompo.OnHitAction?.Invoke();
            return;
        }

        MovementCompo.StopImmediately();

        MovementCompo.OnKnockbackAction?.Invoke();

        IsDead = value;
        CanStateChageable = !value;

        HealthCompo.CanTakeHp(!value);
        MovementCompo.canMove = !value;
        MovementCompo.canKnockback = !value;

        if (value)
        {
            Time.timeScale = 0.5f;
            SoundManager.instance.AddAudioAndPlay(_deadSound);
        }

        AnimatorCompo.SetBool("death", value);

        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        if (value)
        {
            collider.size = _deadSizeChanger.ChangeSize(collider.size, _deadColliderSize);
        }
        else if (!value && collider.size == _deadColliderSize)
        {
            collider.size = _deadSizeChanger.GetSaveSize();
        }

        OnPlayerDeadAction?.Invoke();
    }

    public void SetCanUseSkill(bool attack, bool airDash, bool roll, bool jump, bool heavyAttack, bool airAttack, bool block)
    {
        canAttack = attack;
        canAirDash = airDash;
        canRoll = roll;
        canJump = jump;
        canHeavyAttack = heavyAttack;
        canAirAttack = airAttack;
        canBlock = block;
    }

    private void SetDir(AttackMode attackMode, bool standard)
    {
        if (GameManager.instance.AttackMode != attackMode) return;

        PlayerDir = standard ? 1 : -1;
    }
}
