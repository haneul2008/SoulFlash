using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PlayerAirAttack : AnimationPlayer
{
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
    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.PlayerInput.OnLeftMousePressed += AirAttack;

        _currentTime = _cooltime;
    }
    private void Update()
    {
        if (_attack) return;
        _currentTime += Time.deltaTime;
    }

    private void AirAttack()
    {
        if (!_player.CanStateChageable) return;
        if (!InAir || _currentTime < _cooltime) return;

        _attack = true;
        _currentTime = 0;

        float dir;
        if (Mathf.Abs(_player.PlayerInput.Movement.x) > 0.1f)
        {
            dir = Mathf.Sign(_player.PlayerInput.Movement.x);
        }
        else
        {
            dir = _player.PlayerInput.MousePosition.x > transform.position.x ? 1f : -1f;
        }

        _player.MovementCompo.canMove = false;
        _player.CanStateChageable = false;
        _player.MovementCompo.rbCompo.velocity = Vector2.zero;
        _player.MovementCompo.rbCompo.gravityScale = 0;

        PlayAnimation();
        Invoke("EndAttack", _attackTime);

        StartCoroutine(SpawnSlash(dir));
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
        EndAnimation();

        _player.MovementCompo.canMove = true;
        _player.CanStateChageable = true;

        _attack = false;
    }

    public void SetAirState(bool isInAir)
    {
        InAir = isInAir;
    }
}
