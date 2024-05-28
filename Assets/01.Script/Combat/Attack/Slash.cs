using System;
using System.Collections;
using UnityEngine;

public class Slash : MonoBehaviour, IPoolable
{
    [SerializeField] private float _slashSpeed;
    [SerializeField] private float _slashLifeTime;
    [SerializeField] private string _poolName;
    public string PoolName => _poolName;

    public GameObject ObjectPrefab => gameObject;
    public float Dir { get; set; }

    public void ResetItem()
    {
        StartCoroutine(PushSlash());
    }

    private IEnumerator PushSlash()
    {
        yield return new WaitForSeconds(_slashLifeTime);
        PoolManager.instance.Push(this);
    }

    private void Update()
    {
        transform.position += new Vector3(Dir, 0, 0) * _slashSpeed * Time.deltaTime;
    }
}
