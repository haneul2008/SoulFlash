using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGuardianLaser : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _hpRetakeTime;
    [SerializeField] private float _laserRadius;
    [SerializeField] private float _rotateTime;
    [SerializeField] private Transform _bossTrm;
    [SerializeField] private float _angleLimit;

    private Transform _targetTrm;
    private DamageCaster _damageCaster;
    private bool _canCastDamage;
    private void Awake()
    {
        _targetTrm = GameManager.instance.Player.transform;
        _damageCaster = transform.Find("DamageCaster").GetComponent<DamageCaster>();
    }
    private void OnEnable()
    {
        transform.position = new Vector3(_bossTrm.position.x, transform.position.y, _bossTrm.position.z);

        _canCastDamage = false;

        Vector2 dir = _targetTrm.position - transform.position;
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, z) * Quaternion.Euler(0, 0, 180);
    }
    private void Update()
    {
        Rotate();
        CastDamage();
    }

    private void Rotate()
    {   
        if (_targetTrm == null) return;

        Vector2 dir = _targetTrm.position - transform.position;
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        z = Mathf.Clamp(-Mathf.Abs(z), -180 + _angleLimit, -_angleLimit);

        transform.rotation = Quaternion.Lerp(transform.rotation, 
            Quaternion.Euler(0, 0, z) * Quaternion.Euler(0, 0, 180), Time.deltaTime * _rotateTime);
    }

    private void CastDamage()
    {
        if (!_canCastDamage) return;

        _damageCaster.CastDamage(_damage, 0, _hpRetakeTime, false, true);
    }

    public void CastTrigger()
    {
        _canCastDamage = true;
    }

    public void EndAttack()
    {
        gameObject.SetActive(false);
        _canCastDamage = false;
    }
}
