using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AttackSkillUnlockTrigger : MonoBehaviour
{
    [SerializeField] private UnlockType _unlockType;
    [SerializeField] private float _delay;
    private AttackSkillUnlocker _unlocker;
    private Player _player;
    private Health _dummyHealth;
    private void OnDisable()
    {
        if(_unlockType == UnlockType.AirAttack)
        {
            AttackClamp(true, true, true);
        }

        _dummyHealth.OnHitAction -= Nextunlock;
    }
    private void AttackClamp(bool attck, bool heavyAttack, bool airAttack)
    {
        _player.canAttack = attck;
        _player.canHeavyAttack = heavyAttack;
        _player.canAirAttack = airAttack;
    }
    public void Init(AttackSkillUnlocker unlocker, Health dummyHealth)
    {
        _unlocker = unlocker;
        _dummyHealth = dummyHealth;

        _player = GameManager.instance.Player.GetComponent<Player>();

        switch (_unlockType)
        {
            case UnlockType.Attack:
                AttackClamp(true, false, false);
                break;

            case UnlockType.HeavyAttack:
                AttackClamp(false, true, false);
                break;

            case UnlockType.AirAttack:
                AttackClamp(false, false, true);
                break;
        }

        _dummyHealth.OnHitAction += Nextunlock;
    }
    private void Nextunlock()
    {
        AttackClamp(false, false, false);

        StartCoroutine("WaitDelay");
    }
    private IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(_delay);

        _unlocker.Setting();
    }
}
