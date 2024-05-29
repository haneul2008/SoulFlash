using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttack : AnimationPlayer
{
    [Header("Setting")]
    [SerializeField] private GameObject _damageCaster;
    [SerializeField] private Vector2 _damageCasterPos;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _damageCasterRadius;
    [SerializeField] private float _cooltime;

    [Header("Particle")]
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _particleStartTime;

    private Player _player;
    private float _currentTime;
    private bool _attack;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.PlayerInput.OnEKeyPressed += HandleHeavyAttack;
        _currentTime = _cooltime;
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnEKeyPressed -= HandleHeavyAttack;
    }
    private void Update()
    {
        if (_attack) return;
        _currentTime += Time.deltaTime;
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
    private void EndAttack(DamageCaster damageCaster)
    {
        PoolManager.instance.Push(damageCaster);
        EndAnimation();

        _player.CanStateChageable = true;
        _player.MovementCompo.canMove = true;

        _attack = false;
    }
    private IEnumerator DamageCastCoroutine(float dir)
    {
        yield return new WaitForSeconds(0.1f);

        DamageCaster damageCaster = PoolManager.instance.Pop("DamageCaster") as DamageCaster;

        damageCaster.gameObject.transform.position = new Vector3(transform.position.x + dir * _damageCasterPos.x, _damageCasterPos.y);
        damageCaster.damageRadius = _damageCasterRadius;

        yield return new WaitForSeconds(_attackTime - 0.1f);

        EndAttack(damageCaster);
    }
    private IEnumerator ParticleCoroutine(float dir)
    {
        yield return new WaitForSeconds(_particleStartTime);

        _particle.gameObject.transform.position = new Vector3(transform.position.x + dir * _damageCasterPos.x, _damageCasterPos.y);
        _particle.Play();
    }
}
