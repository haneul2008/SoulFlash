using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpUI : MonoBehaviour
{
    [SerializeField] private GameObject _hpBar;
    [SerializeField] private Health _hp;

    private int _maxHp;
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
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
