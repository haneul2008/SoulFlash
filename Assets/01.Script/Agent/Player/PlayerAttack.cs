using System.Collections;
using UnityEngine;

public class PlayerAttack : AnimationPlayer
{
    [Header("Setting")]
    [SerializeField] private GameObject _damageCaster;
    [SerializeField] private Vector2 _damageCasterPos;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _damageCasterRadius;

    private Player _player;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.PlayerInput.OnLeftMousePressed += HandleAttack;
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnLeftMousePressed -= HandleAttack;
    }
    private void HandleAttack()
    {
        if (!_player.MovementCompo.isGround.Value) return;
        if (!_player.CanStateChageable) return;

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
    private void EndAttack(DamageCaster damageCaster)
    {
        PoolManager.instance.Push(damageCaster);
        EndAnimation();

        _player.CanStateChageable = true;
        _player.MovementCompo.canMove = true;
    }
    private IEnumerator DamageCastCoroutine(float dir)
    {
        yield return new WaitForSeconds(0.1f);

        DamageCaster damageCaster = PoolManager.instance.Pop("DamageCaster") as DamageCaster;

        damageCaster.gameObject.transform.position =new Vector3(transform.position.x + dir * _damageCasterPos.x, _damageCasterPos.y);
        damageCaster.damageRadius = _damageCasterRadius;

        yield return new WaitForSeconds(_attackTime - 0.1f);

        EndAttack(damageCaster);
    }
}
