using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent OnHitEvent;
    public UnityEvent OnDeadEvent;

    public Action OnHitAction;

    [SerializeField] private int _maxHealth = 150;

    public bool IsCanTakeHp { get; private set; } = true;

    public int CurrentHealth { get; private set; }
    private Agent _owner;
    
    public void Initialize(Agent owner)
    {
        _owner = owner;
        ResetHealth();
    }

    public void ResetHealth()
    {
        CurrentHealth = _maxHealth;
        IsCanTakeHp = true;
    }
    public void TakeDamage(int amount, Vector2 normal, Vector2 point, float knockbackPower, float hpRetakeTime)
    {
        if (!IsCanTakeHp) return;

        OnHitAction?.Invoke();

        IsCanTakeHp = false;
        StartCoroutine(CanTakeHpCoroutine(true, hpRetakeTime));

        CurrentHealth -= amount;
        OnHitEvent?.Invoke();

        if (knockbackPower > 0)
            _owner.MovementCompo.GetKnockback(normal * -1, knockbackPower);

        if (CurrentHealth <= 0)
        {
            OnDeadEvent?.Invoke();
        }
    }
    public void CanTakeHp(bool value, float time = 0)
    {
        StartCoroutine(CanTakeHpCoroutine(value, time));
    }
    private IEnumerator CanTakeHpCoroutine(bool value, float time)
    {
        yield return new WaitForSeconds(time);
        IsCanTakeHp = value;
    }
}
