using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent OnHitEvent;
    public UnityEvent OnDeadEvent;

    public Action OnHitAction;

    [field: SerializeField] public int MaxHealth { get; private set; }
    [SerializeField] private bool _isPlayer = false;

    public bool IsCanTakeHp { get; private set; } = true;

    public int CurrentHealth { get; private set; }
    private Agent _owner;
    
    public void Initialize(Agent owner)
    {
        _owner = owner;
        ResetHealth(MaxHealth);
    }

    public void ResetHealth(int hp, bool isCanTakegHp = true)
    {
        float multiplier = _isPlayer ? GameManager.instance.hpMultiplier : 1;
        CurrentHealth = Mathf.Clamp(hp, 0, Mathf.RoundToInt(MaxHealth * multiplier));

        IsCanTakeHp = isCanTakegHp;
    }
    public void TakeDamage(int amount, Vector2 normal, Vector2 point, float knockbackPower, float hpRetakeTime)
    {
        if (!IsCanTakeHp) return;

        CurrentHealth -= amount;

        OnHitAction?.Invoke();

        IsCanTakeHp = false;
        StartCoroutine(CanTakeHpCoroutine(true, hpRetakeTime));

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
    public void SetMaxHealth(int hp)
    {
        MaxHealth = hp;
    }

}
