using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWormFireball : MonoBehaviour, IPoolable
{
    [SerializeField] private string _poolName;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private string _defaultSoltingLayer;
    [SerializeField] private string _explosionSoltingLayer;

    private float _dir;
    private int _damage;
    private float _knockbackPower;


    private DamageCaster _damageCaster;
    private Animator _anim;
    private SpriteRenderer _renderer;
    private int _explosionHash = Animator.StringToHash("Explosion");
    private Rigidbody2D _rigid;
    private bool _isDamageCast;

    public string PoolName => _poolName;

    public GameObject ObjectPrefab => gameObject;

    private void Awake()
    {
        _damageCaster = transform.Find("DamageCaster").GetComponent<DamageCaster>();

        _anim = GetComponent<Animator>();

        _renderer = GetComponent<SpriteRenderer>();

        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Initalize(float dir, int damage, float knockbackPower)
    {
        _dir = dir;
        _damage = damage;
        _knockbackPower = knockbackPower;

        float y = dir == 1? 180 : 0f;
        transform.rotation = Quaternion.Euler(0f, y, 0f);
    }

    private void Update()
    {
        if (_isDamageCast) return;
        _rigid.velocity = new Vector2(_dir, 0) * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explosion();
    }

    private void Explosion()
    {
        if (_isDamageCast) return;

        _isDamageCast = true;

        _rigid.velocity = Vector2.zero;

        _damageCaster.CastDamage(_damage, _knockbackPower, 0.1f, false, true);

        _renderer.sortingLayerName = _explosionSoltingLayer;
        _anim.SetBool(_explosionHash, true);
    }

    public void AnimationEnd()
    {
        _anim.SetBool(_explosionHash, false);
        PoolManager.instance.Push(this);
    }

    public void ResetItem()
    {
        _isDamageCast = false;
        _renderer.sortingLayerName = _defaultSoltingLayer;
    }
}
