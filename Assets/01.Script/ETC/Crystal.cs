using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Crystal : MonoBehaviour, IPoolable
{
    [SerializeField] private string _poolName;
    [SerializeField] private float _pushTime;

    private DashToSelectEnemy _dashToSelectEnemy;
    private Collider2D _collider;

    public string PoolName => _poolName;

    public GameObject ObjectPrefab => gameObject;

    private SpriteRenderer _renderer;
    private Coroutine _corou;
    public void ResetItem()
    {
    }

    private void Awake()
    {
        _dashToSelectEnemy = GameManager.instance.Player.GetComponent<DashToSelectEnemy>();
        _dashToSelectEnemy.OnDashFinishAction += DestroyCrystal;

        _renderer = transform.Find("Visual").GetComponent<SpriteRenderer>();

        _collider = GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        _renderer.DOFade(1, 0.5f);
        _corou = StartCoroutine(PushCoroutine());
    }
    private void OnDestroy()
    {
        _dashToSelectEnemy.OnDashFinishAction -= DestroyCrystal;
        StopCoroutine(_corou);
    }
    private void DestroyCrystal()
    {
        if (_dashToSelectEnemy.NowEnemyCollider == _collider)
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
}
