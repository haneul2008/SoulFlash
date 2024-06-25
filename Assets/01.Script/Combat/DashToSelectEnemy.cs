using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System.Collections;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;

public class DashToSelectEnemy : MonoBehaviour
{
    public UnityEvent OnDashFinishEvent;
    public Action<bool> OnDashFinishAction;

    NotifyValue<Collider2D> EnemyCollider = new NotifyValue<Collider2D>();

    [Header("Setting")]
    [SerializeField] private Player _player;
    [SerializeField] private Sprite _defaultSprite;
    [field: SerializeField] public float DashTime { get; private set; }
    [SerializeField] private float _canTakeAttackTime; //대쉬가 끝난 후 몇초간 넉백과 체력 감소를 무시할지
    [SerializeField] private int _hpIncreaseAmout; //적에게 순간이동후 회복되는 양
    [SerializeField] private Sound _selectingSound;
    public Collider2D NowEnemyCollider { get; private set; }
    public bool IsSelecting { get; private set; }

    public float canDashTime;

    private CinemachineVirtualCamera _cam;
    private MouseDetecter _mouseDeteter;
    private bool _isFalseBlink;
    private Collider2D[] _enemyDetectCollider;
    private SpriteRenderer _enemyRenderer;
    private Material _enemyMat;
    private bool _actionTrigger;
    private Coroutine _coroutine;
    private readonly int _isHitHash = Shader.PropertyToID("_IsHit");
    private void Awake()
    {
        _cam = GameManager.instance.virtualCam;
    }
    private void OnEnable()
    {
        _enemyDetectCollider = new Collider2D[1];

        GameManager.instance.OnEnemyFinalDeadAction += HandleSelectEnemy;
        EnemyCollider.OnValueChanged += HandleGetRenderer;

        GameManager.instance.OnDestroySingleton += UnSubscribe;

        _player.PlayerInput.OnLeftMousePressed += DashToEnemy;

        _mouseDeteter = GameManager.instance.mouseDetecter;
    }
    private void UnSubscribe()
    {
        GameManager.instance.OnEnemyFinalDeadAction -= HandleSelectEnemy;
        EnemyCollider.OnValueChanged -= HandleGetRenderer;
        _player.PlayerInput.OnLeftMousePressed -= DashToEnemy;
    }

    private void Update()
    {
        if (!IsSelecting) return;

        ToDoEnemy();
        DecreaseCameraSize();
    }

    private void DecreaseCameraSize()
    {
        _cam.m_Lens.OrthographicSize = Mathf.Clamp(_cam.m_Lens.OrthographicSize -= 0.001f, 4.5f, 5);
    }

    private void HandleSelectEnemy()
    {
        if (_actionTrigger) return;

        _actionTrigger = true;

        NowEnemyCollider = _mouseDeteter.DetectEnemy();
        _enemyRenderer = NowEnemyCollider == null ? null : NowEnemyCollider.gameObject.transform.Find("Visual")
            .GetComponent<SpriteRenderer>();
        _enemyMat = _enemyRenderer == null ? null : _enemyRenderer.material;

        if (_enemyMat != null )
        {
            _enemyMat.SetInt(_isHitHash, 1);
        }

        Time.timeScale = 0.2f;

        IsSelecting = true;
        _player.CanStateChangable = false;

        _coroutine = StartCoroutine(CanDashTimeCoroutine());

        SoundManager.instance.AddAudioAndPlay(_selectingSound);
    }

    private IEnumerator CanDashTimeCoroutine()
    {
        yield return new WaitForSecondsRealtime(canDashTime);

        if (!IsSelecting) yield break;

        ResetValue(false, true, false);
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
        EnemyCollider.Value = NowEnemyCollider;

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
        if (NowEnemyCollider == null || !IsSelecting) return;

        ResetValue(false, false, true);

        _player.canAttack = false;

        float distance = Vector2.Distance(transform.position, NowEnemyCollider.gameObject.transform.position);
        transform.DOMove(NowEnemyCollider.gameObject.transform.position, Mathf.Clamp(DashTime / distance, 0, 0.3f))
            .OnComplete(() =>
            {
                int addValue = 0;

                if(GameManager.instance.soulRandomNum != 0)
                {
                    GetRandomValue randomValue = new GetRandomValue();
                    addValue = Mathf.CeilToInt(randomValue.GetRandom(1, 100)) <= GameManager.instance.soulRandomNum ? 1 : 0;
                }
                GameManager.instance.soulCount += 1 + addValue;
                GameManager.instance.soulCollectCount += 1 + addValue;


                _player.HealthCompo.ResetHealth(Mathf.RoundToInt(_player.HealthCompo.CurrentHealth 
                    + _hpIncreaseAmout + GameManager.instance.soulTpHpIncreaseAdder), false);

                _player.HealthCompo.OnHitAction?.Invoke();

                if(_coroutine != null)
                    StopCoroutine(_coroutine);

                StartCoroutine("CanTakeAttackCoroutine");
            });
    }
    private void ResetValue(bool isSelecting, bool canMove, bool isDash)
    {
        Time.timeScale = 1f;
        IsSelecting = isSelecting;
        _player.MovementCompo.canMove = canMove;

        _player.HealthCompo.CanTakeHp(false);
        _player.MovementCompo.canKnockback = false;

        _cam.m_Lens.OrthographicSize = 5f;

        OnDashFinishEvent?.Invoke();
        OnDashFinishAction?.Invoke(isDash);

        _actionTrigger = false;

        if (_enemyMat == null) return;
            _enemyMat.SetInt(_isHitHash, 0);
    }
    private IEnumerator CanTakeAttackCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        _player.CanStateChangable = true;

        _player.MovementCompo.canMove = true;

        _player.canAttack = true;

        yield return new WaitForSeconds(_canTakeAttackTime - 0.1f);

        _player.HealthCompo.CanTakeHp(true);
        _player.MovementCompo.canKnockback = true;
    }
}
