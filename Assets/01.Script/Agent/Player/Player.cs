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

    [field: SerializeField] public InputReader PlayerInput { get; private set; }

    private bool _canDoubleJump;
    public bool CanStateChageable { get; set; } = true;
    [HideInInspector] public bool animationEndTrigger;

    private CameraConfiner _cameraConfiner;
    private GameObject _light;
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

        HealthCompo.ResetHealth(Mathf.RoundToInt(HealthCompo.MaxHealth * GameManager.instance.hpMultiplier));

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
        if (!MovementCompo.canMove) return;
        if (Mathf.Abs(PlayerInput.Movement.x) > 0.1f)
        {
            float rotationY = PlayerInput.Movement.x > 0.1f ? 0 : -180f;
            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }
        else
            HandleSpriteFlip(PlayerInput.MousePosition);
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

        Flip();
        _airAttack.SetAirState(!MovementCompo.isGround.Value);
    }

    private void HandleJumpKeyEvent()
    {
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
        if (!CanStateChageable) return;
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

        MovementCompo.StopImmediately();

        MovementCompo.OnKnockbackAction?.Invoke();

        IsDead = value;
        CanStateChageable = !value;

        HealthCompo.CanTakeHp(!value);
        MovementCompo.canMove = !value;
        MovementCompo.canKnockback = !value;

        if (value)
            Time.timeScale = 0.5f;

        AnimatorCompo.SetBool("death", true);

        SizeChanger sizeChanger = new SizeChanger();

        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        if (value)
        {
            collider.size = sizeChanger.ChangeSize(collider.size, _deadColliderSize);
        }
        else if (!value && collider.size == _deadColliderSize)
        {
            collider.size = sizeChanger.GetSaveSize();
        }

        OnPlayerDeadAction?.Invoke();
    }
}
