using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHeavyAttack : AnimationPlayer
{
    public UnityEvent OnHeavyAttackEvent;
    public event Action<int> OnEndHeavyAttackAction;
    public event Action OnHeavyAttackAction;

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
    [SerializeField] private float _setParentDelay;

    [Header("AttackSetting")]
    [SerializeField] private int _damage;
    [SerializeField] private float _knockbackPower;
    [SerializeField] private float _hpRetakeTime;

    private Player _player;
    private float _currentTime;
    private bool _attack;
    private Coroutine _particleCorou;
    private Tween _particleTween;
    public override void Initialize(Agent agent)
    {
        base.Initialize(agent);

        _player = agent as Player;

        _player.PlayerInput.OnEKeyPressed += HandleHeavyAttack;
        _player.MovementCompo.OnKnockbackAction += EndAttack;

        _currentTime = _cooltime;
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnEKeyPressed -= HandleHeavyAttack;
        _player.MovementCompo.OnKnockbackAction -= EndAttack;

        if(_particleTween != null)
            _particleTween.Kill();
    }
    private void Update()
    {
        if (_attack && !_player.MovementCompo.isGround.Value) EndAttack();
        if(_attack) _player.MovementCompo.StopImmediately();
        _currentTime += Time.deltaTime;
    }
    private void HandleHeavyAttack()
    {
        if (!_player.MovementCompo.isGround.Value || !_player.canHeavyAttack) return;
        if (!_player.CanStateChageable || _currentTime < _cooltime * GameManager.instance.groundCooldownMutiplier) return;

        OnEndHeavyAttackAction?.Invoke(Mathf.RoundToInt(_cooltime * GameManager.instance.groundCooldownMutiplier));
        OnHeavyAttackAction?.Invoke();

        _attack = true;
        _currentTime = 0;

        _player.CanStateChageable = false;
        _player.MovementCompo.canMove = false;

        PlayAnimation();

        float dir = transform.rotation.eulerAngles.y == 0f ? 1 : -1;

        StartCoroutine(DamageCastCoroutine(dir));
        _particleCorou = StartCoroutine(ParticleCoroutine(dir));
    }
    private void EndAttack()
    {
        EndAnimation();
        _player.CanStateChageable = true;
        _player.MovementCompo.canMove = true;
        _damageCaster.damageRadius = 0;

        _attack = false;
        if(_particleCorou != null)
            StopCoroutine(_particleCorou);
    }
    private IEnumerator DamageCastCoroutine(float dir)
    {
        yield return new WaitForSeconds(_damageCastTime);

        if (!_attack) yield break;

        _damageCaster.gameObject.transform.position = new Vector3(transform.position.x + dir * _damageCasterPos.x, transform.position.y);
        _damageCaster.damageRadius = _damageCasterRadius;

        float multiplier = GameManager.instance.groundDamageMultiplier + GameManager.instance.passiveGroundDamage;

        _damageCaster.CastDamage(Mathf.RoundToInt(_damage * multiplier)
            , _knockbackPower, _hpRetakeTime, false);

        OnHeavyAttackEvent?.Invoke();

        yield return new WaitForSeconds(_attackTime - 0.1f);
        EndAttack();
    }
    private IEnumerator ParticleCoroutine(float dir)
    {
        yield return new WaitForSeconds(_particleStartTime);

        _particle.gameObject.transform.SetParent(null);

        _particle.gameObject.transform.position = new Vector3(transform.position.x + dir * _damageCasterPos.x, transform.position.y);

        _particleTween = DOTween.Sequence()
            .Append(DOTween.To(PlayParticle, 0, 1, _particle.main.duration))
            .OnComplete(() =>
            {
                _particleCorou = StartCoroutine("SetParticleparent");
            });
    }
    private void PlayParticle(float f)
    {
        if(!_particle.isPlaying)
            _particle.Play();
    }
    private IEnumerator SetParticleparent()
    {
        yield return new WaitForSeconds(_setParentDelay);
        _particle.gameObject.transform.SetParent(_player.transform);
    }
}
