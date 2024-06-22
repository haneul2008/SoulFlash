using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpUI : MonoBehaviour
{
    [SerializeField] private GameObject _hpBar;
    [SerializeField] private Health _hp;

    private Enemy _enemy;
    private int _maxHp;
    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
        _enemy.OnFlipEvent += HandleOnFlipEvent;
    }
    private void OnDestroy()
    {
        _enemy.OnFlipEvent -= HandleOnFlipEvent;
    }
    private void HandleOnFlipEvent()
    {
        if (_enemy.IsFacingRight())
        {
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, -180f, 0);
        }
    }

    private void OnEnable()
    {
        _hpBar.transform.localScale = Vector3.one;

        _maxHp = _hp.CurrentHealth;
        if(_maxHp == 0) _maxHp = _hp.MaxHealth;
    }
    public void SetUi()
    {
        _hpBar.transform.localScale = new Vector3(Mathf.Clamp(_hp.CurrentHealth / (float)_maxHp, 0, 1), 1, 1);
    }
}
