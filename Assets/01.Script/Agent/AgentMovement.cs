using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AgentMovement : AnimationPlayer
{
    public Action OnKnockbackAction;

    [Header("Reference")]
    [SerializeField] private Transform _groundCheckerTrm;

    [Header("Settings")]
    [SerializeField] private bool _isPlayer = false;
    public float moveSpeed = 6f;
    public float jumpPower = 6f;
    public float extraGravity = 30f;
    public float gravityDelay = 0.15f;
    public float knockbackTime = 0.2f;

    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Vector2 _groundCheckerSize;


    public NotifyValue<bool> isGround = new NotifyValue<bool>();
    public Rigidbody2D rbCompo { get; private set; }

    protected float _xMove;
    private float _timeInAir;
    public bool canMove = true;
    public bool canKnockback = true;
    protected Coroutine _kbCoroutine;

    private Agent _owner;
    private Player _player;
    public bool IsKnockback { get; private set; }
    public void Initalize(Agent agent)
    {
        _owner = agent;
        rbCompo = GetComponent<Rigidbody2D>();

        if(!_isPlayer) return;
        _player = _owner.GetComponent<Player>();
    }

    public void JumpTo(Vector2 force)
    {
        SetMovement(force.x);
        rbCompo.AddForce(force, ForceMode2D.Impulse);
    }

    public void SetMovement(float xMove)
    {
        if (_isPlayer && _agent.dontFlip) return;

        _xMove = xMove;

        if (Mathf.Abs(_xMove) > 0.1f)
            PlayAnimation();
        else
            EndAnimation();
    }

    public void StopImmediately(bool isYStop = false)
    {
        _xMove = 0;
        if (isYStop)
        {
            rbCompo.velocity = Vector2.zero;
        }
        else
        {
            rbCompo.velocity = new Vector2(_xMove, rbCompo.velocity.y);
        }
    }

    public void Jump(float multiplier = 1f)
    {
        _timeInAir = 0;
        rbCompo.velocity = Vector2.zero;
        rbCompo.AddForce(Vector2.up * jumpPower * multiplier, ForceMode2D.Impulse);
    }

    private void Update()
    {
        CalculateInAirTime();
        KnockBackCheck();
    }

    private void KnockBackCheck()
    {
        if (!IsKnockback) return;
        if (!_isPlayer) return;

        _player.CanStateChageable = false;
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        ApplyExtraGravity();
        ApplyXMove();
    }

    private void ApplyXMove()
    {
        if (!canMove) return;

        float finalMoveSpeed = _isPlayer ? moveSpeed * GameManager.instance.moveSpeedMutiplier : moveSpeed;
        rbCompo.velocity = new Vector2(_xMove * finalMoveSpeed, rbCompo.velocity.y);
    }

    public void CheckGrounded()
    {
        Collider2D collider = Physics2D.OverlapBox(_groundCheckerTrm.position, _groundCheckerSize, 0, _whatIsGround);

        isGround.Value = collider != null;
    }

    private void CalculateInAirTime()
    {
        if (!canMove) return;
        if (!isGround.Value)
        {
            _timeInAir += Time.deltaTime;
        }
        else
        {
            _timeInAir = 0;
        }
    }

    private void ApplyExtraGravity()
    {
        if(!canMove) return;
        if (_timeInAir > gravityDelay)
            rbCompo.AddForce(new Vector2(0, -extraGravity));
    }

    #region knockback region
    public void GetKnockback(Vector3 direction, float power)
    {
        if (!canKnockback) return;

        IsKnockback = true;

        OnKnockbackAction?.Invoke();
        canMove = false;

        StopImmediately();

        Vector3 difference = direction * power * rbCompo.mass;
        rbCompo.AddForce(difference, ForceMode2D.Impulse);

        if (_kbCoroutine != null)
            StopCoroutine(_kbCoroutine);

        _kbCoroutine = StartCoroutine(knockbackCoroutine());
    }

    private IEnumerator knockbackCoroutine()
    {
        yield return new WaitForSeconds(knockbackTime);
        rbCompo.velocity = Vector2.zero;
        canMove = true;

        IsKnockback = false;

        if (!_isPlayer) yield break;
        _player.CanStateChageable = true;
    }
    public void ClearKnockback()
    {
        rbCompo.velocity = Vector2.zero;
        canMove = true;
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_groundCheckerTrm == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheckerTrm.position, _groundCheckerSize);
        Gizmos.color = Color.white;
    }
#endif
}
