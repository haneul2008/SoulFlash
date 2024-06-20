using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAirAttack : AnimationPlayer
{
    public event Action<int> OnEndAirAttackAction;
    public event Action OnAirAttackAction;

    [Header("Setting")]
    [SerializeField] private float _slashSpawnX;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _cooltime;
    [SerializeField] private float[] _slashSpawnTime;
    [SerializeField] private float[] _slashRotationX;

    public bool InAir { get; private set; }
    private Player _player;
    private float _currentTime;
    private bool _attack;
    private Coroutine _slashSpawnCorou;
    public override void Initialize(Agent agent)
    {
        base.Initialize(agent);

        _player = agent as Player;

        _player.PlayerInput.OnEKeyPressed += AirAttack;
        _player.MovementCompo.OnKnockbackAction += EndAttack;

        _currentTime = _cooltime;
    }
    private void OnDisable()
    {
        _player.MovementCompo.OnKnockbackAction -= EndAttack;
    }
    private void Update()
    {
        if (_attack)
        {
            _player.MovementCompo.StopImmediately(true);
            return;
        }
        _currentTime += Time.deltaTime;
    }

    private void AirAttack()
    {
        if (!_player.CanStateChageable || !_player.canAirAttack) return;
        if (!InAir || _currentTime < _cooltime * GameManager.instance.airCooldownMutiplier) return;

        _currentTime = 0;

        _attack = true;

        OnAirAttackAction?.Invoke();

        float dir = transform.rotation.eulerAngles.y == 0f ? 1 : -1;

        _player.MovementCompo.canMove = false;
        _player.CanStateChageable = false;

        _player.MovementCompo.rbCompo.velocity = Vector2.zero;
        _player.MovementCompo.rbCompo.gravityScale = 0;

        PlayAnimation();
        Invoke("EndAttack", _attackTime);

        _slashSpawnCorou = StartCoroutine(SpawnSlash(dir));
    }
    private IEnumerator SpawnSlash(float dir)
    {
        for(int i = 0; i < _slashSpawnTime.Length; i++)
        {
            yield return new WaitForSeconds(_slashSpawnTime[i]);

            Slash slash = PoolManager.instance.Pop("Slash") as Slash;
            slash.gameObject.transform.position = new Vector3(dir * _slashSpawnX + transform.position.x, transform.position.y, 0);

            float rotationY = dir == 1f ? 0 : 180;
            slash.gameObject.transform.rotation = Quaternion.Euler(_slashRotationX[i], rotationY, 0);
            slash.Dir = dir;
        }
    }

    private void EndAttack()
    {
        if (!_attack) return;

        EndAnimation();

        _player.MovementCompo.canMove = true;
        _player.CanStateChageable = true;
        _player.MovementCompo.rbCompo.gravityScale = 1;

        OnEndAirAttackAction?.Invoke(Mathf.RoundToInt(_cooltime * GameManager.instance.airCooldownMutiplier));

        _attack = false;

        if (_slashSpawnCorou != null)
            StopCoroutine(_slashSpawnCorou);
    }

    public void SetAirState(bool isInAir)
    {
        InAir = isInAir;
    }
}
