using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System.Collections;
using UnityEngine.Events;
using System;

public class DashToSelectEnemy : MonoBehaviour
{
    public UnityEvent OnDashFinishEvent;
    public Action OnDashFinishAction;

    NotifyValue<Collider2D> EnemyCollider = new NotifyValue<Collider2D>();

    [Header("Setting")]
    [SerializeField] private Player _player;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _canDashTime;
    [SerializeField] private float _canTakeAttackTime; //대쉬가 끝난 후 몇초간 넉백과 체력을 무시할지
    [SerializeField] private MouseDetecter _mouseDeteter;
    [SerializeField] private CinemachineVirtualCamera _cam;
    public Collider2D NowEnemyCollider { get; private set; }

    private bool _isSeleting;
    private bool _isFalseBlink;
    private Collider2D[] _enemyDetectCollider;
    private SpriteRenderer _enemyRenderer;
    private Material _enemyMat;

    private readonly int _isHitHash = Shader.PropertyToID("_IsHit");
    private void OnEnable()
    {
        _enemyDetectCollider = new Collider2D[1];

        GameManager.instance.OnEnemyDeadAction += HandleSelectEnemy;
        EnemyCollider.OnValueChanged += HandleGetRenderer;

        GameManager.instance.OnDestroySingleton += UnSubscribe;

        _player.PlayerInput.OnLeftMousePressed += DashToEnemy;
    }
    private void UnSubscribe()
    {
        GameManager.instance.OnEnemyDeadAction -= HandleSelectEnemy;
        EnemyCollider.OnValueChanged -= HandleGetRenderer;
        _player.PlayerInput.OnLeftMousePressed -= DashToEnemy;
    }

    private void Update()
    {
        if (!_isSeleting) return;

        ToDoEnemy();
        DecreaseCameraSize();
    }

    private void DecreaseCameraSize()
    {
        _cam.m_Lens.OrthographicSize -= 0.002f;
    }

    private void HandleSelectEnemy()
    {
        NowEnemyCollider = null;
        _enemyRenderer = null;
        _enemyMat = null;
        Time.timeScale = 0.2f;

        _isSeleting = true;
        _player.CanStateChageable = false;

        StartCoroutine(CanDashTimeCoroutine());
    }

    private IEnumerator CanDashTimeCoroutine()
    {
        yield return new WaitForSecondsRealtime(_canDashTime);

        if (!_isSeleting) yield break;

        ResetValue(false, true);
        _player.CanStateChageable = true;
        StartCoroutine("CanTakeAttackCoroutine");
    }

    private void ToDoEnemy()
    {
        NowEnemyCollider = _mouseDeteter.DetectEnemy();
        if (!_isFalseBlink && _enemyMat != null && NowEnemyCollider == null)
        {
            _isFalseBlink = true;
            _enemyMat.SetInt(_isHitHash, 0);
        }

        if (NowEnemyCollider == null) return;

        _isFalseBlink = false;
        EnemyCollider.Value = _mouseDeteter.DetectEnemy();

        if(_enemyMat == null) return;
        _enemyMat.SetInt(_isHitHash, 1);
    }
    private void HandleGetRenderer(Collider2D prev, Collider2D next)
    {
        if (_enemyRenderer != null)
            _enemyMat.SetInt(_isHitHash, 0);

        _enemyRenderer = next.gameObject.transform.Find("Visual").GetComponent<SpriteRenderer>();
        _enemyMat = _enemyRenderer.material;
    }
    private void DashToEnemy()
    {
        if (NowEnemyCollider == null || !_isSeleting) return;

        ResetValue(false, false);

        float distance = Vector2.Distance(transform.position, NowEnemyCollider.gameObject.transform.position);
        transform.DOMove(new Vector2(NowEnemyCollider.gameObject.transform.position.x, transform.position.y), _dashTime / distance)
            .OnComplete(() =>
            {
                _player.MovementCompo.canMove = true;

                _player.CanStateChageable = true;

                StartCoroutine("CanTakeAttackCoroutine");
            });
    }
    private void ResetValue(bool isSelecting, bool canMove)
    {
        Time.timeScale = 1f;
        _isSeleting = isSelecting;
        _player.MovementCompo.canMove = canMove;

        StartCoroutine(_player.HealthCompo.CanTakeHpCoroutine(false));
        _player.MovementCompo.canKnockback = false;

        _cam.m_Lens.OrthographicSize = 5f;

        OnDashFinishEvent?.Invoke();
        OnDashFinishAction?.Invoke();

        if (_enemyMat == null) return;
            _enemyMat.SetInt(_isHitHash, 0);
    }
    private IEnumerator CanTakeAttackCoroutine()
    {
        yield return new WaitForSeconds(_canTakeAttackTime);

        StartCoroutine(_player.HealthCompo.CanTakeHpCoroutine(true));
        _player.MovementCompo.canKnockback = true;
    }
}
