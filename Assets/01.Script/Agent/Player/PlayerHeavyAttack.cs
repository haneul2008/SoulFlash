using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttack : AnimationPlayer
{
    [Header("Setting")]
    [SerializeField] private DamageCaster _damageCaster;
    [SerializeField] private Vector2 _damageCasterPos;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _damageCasterRadius;
    [SerializeField] private float _damageCastTime;
    [SerializeField] private float _cooltime;

    [Header("Particle")]
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _particleStartTime;

    [Header("AttackSetting")]
    [SerializeField] private int _damage;
    [SerializeField] private float _knockbackPower;
    [SerializeField] private float _hpRetakeTime;

    private Player _player;
    private float _currentTime;
    private bool _attack;
    private bool _isDamageCast;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.PlayerInput.OnEKeyPressed += HandleHeavyAttack;
        _player.MovementCompo.OnKnockbackAction += EndAttack;
        _currentTime = _cooltime;
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnEKeyPressed -= HandleHeavyAttack;
        _player.MovementCompo.OnKnockbackAction -= EndAttack;
    }
    private void Update()
    {
        if (!_attack)
        {
            _currentTime += Time.deltaTime;
        }
        else if(_isDamageCast)
            _damageCaster.CastDamage(_damage, _knockbackPower, _hpRetakeTime, false);
    }
    private void HandleHeavyAttack()
    {
        if (!_player.MovementCompo.isGround.Value) return;
        if (!_player.CanStateChageable || _currentTime < _cooltime) return;

        _attack = true;
        _currentTime = 0;

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
        StartCoroutine(ParticleCoroutine(dir));
    }
    private void EndAttack()
    {
        EndAnimation();

        _player.CanStateChageable = true;
        _player.MovementCompo.canMove = true;
        _damageCaster.damageRadius = 0;

        _attack = false;
    }
    private IEnumerator DamageCastCoroutine(float dir)
    {
        yield return new WaitForSeconds(_damageCastTime);

        _isDamageCast = true;

        _damageCaster.gameObject.transform.position = new Vector3(transform.position.x + dir * _damageCasterPos.x, _damageCasterPos.y);
        _damageCaster.damageRadius = _damageCasterRadius;

        yield return new WaitForSeconds(_attackTime - 0.1f);
        
        _isDamageCast = false;
        EndAttack();
    }
    private IEnumerator ParticleCoroutine(float dir)
    {
        yield return new WaitForSeconds(_particleStartTime);

        _particle.gameObject.transform.position = new Vector3(transform.position.x + dir * _damageCasterPos.x, _damageCasterPos.y);
        _particle.Play();
    }
}