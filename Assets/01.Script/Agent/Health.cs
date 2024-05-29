using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent OnHitEvent;
    public UnityEvent OnDeadEvent;

    [SerializeField] private int _maxHealth = 150;

    public bool IsCanTakeHp { get; private set; } = true;

    private int _currentHealth;
    private Agent _owner;

    public void Initialize(Agent owner)
    {
        _owner = owner;
        ResetHealth();
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
    }
    public void TakeDamage(int amount, Vector2 normal, Vector2 point, float knockbackPower)
    {
        if (!IsCanTakeHp) return;

        _currentHealth -= amount;
        OnHitEvent?.Invoke();
        //normal과 point, 넉백 등은 차후에 여기서 사용합니다.

        if (knockbackPower > 0)
            _owner.MovementCompo.GetKnockback(normal * -1, knockbackPower);

        if (_currentHealth <= 0)
        {
            OnDeadEvent?.Invoke();
        }
    }
    public void ChangeCanTakeHp(bool value)
    {
        IsCanTakeHp = value;
    }
}
