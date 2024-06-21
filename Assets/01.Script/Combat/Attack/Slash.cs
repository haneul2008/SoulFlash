using System;
using System.Collections;
using UnityEngine;

public class Slash : MonoBehaviour, IPoolable
{
    [Header("Setting")]
    [SerializeField] private DamageCaster _damageCaster;
    [SerializeField] private float _slashSpeed;
    [SerializeField] private float _slashLifeTime;
    [SerializeField] private string _poolName;

    [Header("AttackSetting")]
    [SerializeField] private int _damage;
    [SerializeField] private float _knockbackPower;
    [SerializeField] private float _hpRetakeTime;

    public string PoolName => _poolName;

    public GameObject ObjectPrefab => gameObject;
    public float Dir { get; set; }

    private Coroutine _corou;

    public void ResetItem()
    {
        _corou = StartCoroutine(PushSlash(_slashLifeTime));
    }

    private IEnumerator PushSlash(float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.instance.Push(this);
    }

    private void OnDisable()
    {
        if(_corou != null)
            StopCoroutine(_corou);
    }

    private void Update()
    {
        transform.position += new Vector3(Dir, 0, 0) * _slashSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float multiplier = GameManager.instance.airDamageMultiplier + GameManager.instance.passiveAirDamage;

        _damageCaster.CastDamage(Mathf.RoundToInt(_damage * multiplier)
            , _knockbackPower, _hpRetakeTime, false);

        _corou = StartCoroutine(PushSlash(0));
    }
}
