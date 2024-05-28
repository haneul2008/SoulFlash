using UnityEngine;

public class PlayerRolls : AnimationPlayer
{
    [Header("Setting")]
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _rollTime;
    [SerializeField] private float _coolTime;

    private Player _player;
    private bool _roll;
    private float _dir;
    private float _currentTime = 2f;
    private CapsuleCollider2D _collider;
    private SizeChanger _sizeChanger;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.PlayerInput.OnRollEvent += Roll;
        _collider = GetComponent<CapsuleCollider2D>();
        _sizeChanger = new();
        //0.78, 0.78
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnRollEvent -= Roll;
    }

    private void Roll()
    {
        if (!_player.MovementCompo.isGround.Value) return;
        if (_currentTime < _coolTime) return;
        if (!_player.CanStateChageable) return;

        _player.CanStateChageable = false;

        _collider.size = _sizeChanger.ChangeSize(_collider.size, new Vector2(0.76f, 0.76f));

        _dir = _player.PlayerInput.Movement.x;
        _player.MovementCompo.canMove = false;
        _roll = true;

        PlayAnimation();
        Invoke("RollEnd", _rollTime);
    }
    private void Update()
    {
        _currentTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!_roll) return;
        transform.position += new Vector3(_dir, 0, 0) * _rollSpeed * Time.deltaTime;
    }
    private void RollEnd()
    {
        _roll = false;

        EndAnimation();

        _sizeChanger.GetSaveSize();
        _player.MovementCompo.canMove = true;
        _currentTime = 0;
        _player.CanStateChageable = true;
    }
}
