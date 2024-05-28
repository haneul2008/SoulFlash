using UnityEngine;

public class DamageCaster : MonoBehaviour, IPoolable
{
    [SerializeField] private string _poolName;

    public ContactFilter2D filter;
    public float damageRadius;
    public int detectCount = 1;

    private Collider2D[] _colliders;

    public string PoolName => _poolName;

    public GameObject ObjectPrefab => gameObject;

    private void Awake()
    {
        _colliders = new Collider2D[detectCount];
    }
    public bool CastDamage(int damage, float knockbackPower)
    {
        int cnt = Physics2D.OverlapCircle(transform.position, damageRadius, filter, _colliders);

        for (int i = 0; i < cnt; i++)
        {
            if (_colliders[i].TryGetComponent(out Health health))
            {
                Vector2 direction = _colliders[i].transform.position - transform.position;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized,
                    direction.magnitude, filter.layerMask);

                health.TakeDamage(damage, hit.normal, hit.point, knockbackPower);
            }
        }

        return cnt > 0;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
        Gizmos.color = Color.white;
    }

    public void ResetItem()
    {
        
    }
#endif
}

