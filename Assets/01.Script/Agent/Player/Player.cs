using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;
public class Player : Agent
{
    public UnityEvent JumpEvent;
    [SerializeField] private DamageCaster _damageCaster;
    [SerializeField] private Vector2 _damageCasterPos;
    [SerializeField] private int _damage;
    [SerializeField] private float _knockbackPower;
    [SerializeField] private float _hpRetakeTime;
    [SerializeField] private float _damageCasterRadius;
    [SerializeField] private CameraConfiner _cameraConfiner;

    [field: SerializeField] public InputReader PlayerInput { get; private set; }

    private bool _canDoubleJump;
    public bool CanStateChageable { get; set; } = true;
    [HideInInspector] public bool animationEndTrigger;
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
    }
    private void OnDisable()
    {
        PlayerInput.JumpEvent -= HandleJumpKeyEvent;
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
        MovementCompo.SetMovement(PlayerInput.Movement.x);

        if(_cameraConfiner.PlayerClamp != 0)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, Camera.main.transform.position.x - _cameraConfiner.PlayerClamp,
                Camera.main.transform.position.x + _cameraConfiner.PlayerClamp), transform.position.y, 0);
        }

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

        _damageCaster.gameObject.transform.position = new Vector3(transform.position.x + dir * _damageCasterPos.x, _damageCasterPos.y);
        _damageCaster.damageRadius = _damageCasterRadius;
        _damageCaster.CastDamage(_damage, _knockbackPower, _hpRetakeTime, false); 
    }
    public void AnimationEndTrigger()
    {
        animationEndTrigger = true;
    }
}
