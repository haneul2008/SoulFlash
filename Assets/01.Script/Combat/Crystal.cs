using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class Crystal : MonoBehaviour, IPoolable, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _poolName;
    [SerializeField] private float _pushTime;
    [SerializeField] private bool _noPush;

    private DashToSelectEnemy _dashToSelectEnemy;
    private Collider2D _collider;

    public string PoolName => _poolName;

    public GameObject ObjectPrefab => gameObject;

    private SpriteRenderer _renderer;
    private Coroutine _corou;

    public bool _init;
    private int _enemyDeadLayer;
    private int _enemyLayer;
    private bool _isClick;
    private Material _material;

    private readonly int _isHitHash = Shader.PropertyToID("_IsHit");

    public void ResetItem()
    {
        gameObject.layer = _enemyDeadLayer;
        _init = false;
    }

    private void Awake()
    {
        _dashToSelectEnemy = GameManager.instance.Player.GetComponent<DashToSelectEnemy>();
        _dashToSelectEnemy.OnDashFinishAction += DestroyCrystal;

        _renderer = transform.Find("Visual").GetComponent<SpriteRenderer>();

        _collider = GetComponent<Collider2D>();

        _enemyDeadLayer = LayerMask.NameToLayer("DeathEnemy");
        _enemyLayer = LayerMask.NameToLayer("Enemy");

        _material = _renderer.material;
    }
    public void SetCrystalSpawnTime(float delay)
    {
        _corou = StartCoroutine(SpawnCrystalCoroutine(delay));
    }
    private IEnumerator SpawnCrystalCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Init();
    }
    private void Init()
    {
        _init = true;
        gameObject.layer = _enemyLayer;

        _renderer.DOFade(1, 0.5f);
        if(!_noPush) _corou = StartCoroutine(PushCoroutine());
    }
    private void OnDestroy()
    {
        _dashToSelectEnemy.OnDashFinishAction -= DestroyCrystal;
        if (_corou != null)
        {
            StopCoroutine(_corou);
        }
    }
    private void DestroyCrystal(bool isSelecting)
    {
        if (!_init) return;
        if (_dashToSelectEnemy.NowEnemyCollider == _collider && isSelecting)
            PushCrystal();
    }
    private IEnumerator PushCoroutine()
    {
        yield return new WaitForSeconds(_pushTime);
        PushCrystal();
    }
    private void PushCrystal()
    {
        _renderer.DOFade(0, 0.5f);
        PoolManager.instance.Push(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_init) return;

        DashToSelectEnemy dashToSelectEnemy = GameManager.instance.Player.GetComponent<DashToSelectEnemy>();
        
        float distance = Vector2.Distance(GameManager.instance.Player.transform.position, transform.position);
        GameManager.instance.Player.transform.DOMove(transform.position, Mathf.Clamp(dashToSelectEnemy.DashTime / distance, 0, 0.5f))
            .OnComplete(()=>
            {
                if(_noPush) Destroy(gameObject);
                else PoolManager.instance.Push(this);
            });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_init) return;
        _material.SetInt(_isHitHash, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_init) return;
        _material.SetInt(_isHitHash, 0);
    }
}
