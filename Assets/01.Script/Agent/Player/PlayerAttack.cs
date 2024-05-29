using System.Collections;
using UnityEngine;

public class PlayerAttack : AnimationPlayer
{
    [Header("Setting")]
    [SerializeField] private DamageCaster _damageCaster;
    [SerializeField] private Vector2 _damageCasterPos;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _damageCasterRadius;

    [Header("AttackSetting")]
    [SerializeField] private int _damage;
    [SerializeField] private float _knockbackPower;
    [SerializeField] private float _hpReTakeTime;

    private Player _player;
    private Collider2D[] _colliders;
    private bool _isAttack;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.PlayerInput.OnLeftMousePressed += HandleAttack;
        _colliders = new Collider2D[1];
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnLeftMousePressed -= HandleAttack;
    }
    private void Update()
    {
        if (!_isAttack) return;
        _damageCaster.CastDamage(_damage, _knockbackPower, _hpReTakeTime);
    }
    private void HandleAttack()
    {
        if (!_player.MovementCompo.isGround.Value) return;
        if (!_player.CanStateChageable) return;

        _isAttack = true;

        _player.CanStateChageable = false;
        _player.MovementCompo.canMove = false;
        _player.MovementCompo.rbCompo.velocity = Vector2.zero;

        PlayAnimation();

        float dir;
        if (Mathf.Abs(_player.PlayerInput.Movement.x) > 0.1f)
        {
            dir = Mathf.Sign(_player.PlayerInput.Movement.x);
        }
        else
        {
            dir = _player.PlayerInput.MousePosition.x > transform.position.x ? 1f : -1f;
        }
        StartCoroutine(DamageCastCoroutine(dir));
    }
    private void EndAttack()
    {
        EndAnimation();

        _player.CanStateChageable = true;
        _player.MovementCompo.canMove = true;

        _isAttack = false;
    }
    private IEnumerator DamageCastCoroutine(float dir)
    {
        yield return new WaitForSeconds(0.1f);

        _damageCaster.gameObject.transform.position = new Vector3(transform.position.x + dir * _damageCasterPos.x, _damageCasterPos.y);
        _damageCaster.damageRadius = _damageCasterRadius;

        yield return new WaitForSeconds(_attackTime - 0.1f);

        EndAttack();
    }
}
