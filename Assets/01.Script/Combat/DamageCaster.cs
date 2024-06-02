using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [SerializeField] private string _poolName;

    public ContactFilter2D filter;
    public float damageRadius;
    public int detectCount = 1;
    public bool isDamageCast;

    private Collider2D[] _colliders;

    private void Awake()
    {
        _colliders = new Collider2D[detectCount];
    }
    public bool CastDamage(int damage, float knockbackPower, float hpRetakeTime, bool useHitDir = true, bool enemy = false)
    {
        int cnt = Physics2D.OverlapCircle(transform.position, damageRadius, filter, _colliders);

        for (int i = 0; i < cnt; i++)
        {
            if (_colliders[i].TryGetComponent(out Health health))
            {
                Vector2 direction;
                RaycastHit2D hit;

                Vector2 knockbackDir;
                Vector3 hitPoint;
                if (useHitDir)
                {
                    direction = _colliders[i].transform.position - transform.position;

                    hit = Physics2D.Raycast(transform.position, direction.normalized,
                        direction.magnitude, filter.layerMask);

                    knockbackDir = hit.normal;
                    hitPoint = hit.point;
                }
                else
                {
                    float x = transform.rotation.eulerAngles.y == 0f ? -1 : 1;

                    if (enemy) x = -x;

                    direction = new Vector2(x, 0);

                    knockbackDir = direction;
                    hitPoint = Vector3.zero;
                }

                isDamageCast = true;
                health.TakeDamage(damage, knockbackDir, hitPoint, knockbackPower, hpRetakeTime);
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
#endif
}

