using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [SerializeField] private string _poolName;
    [SerializeField] private bool _useBox;
    [SerializeField] private Vector2 _boxSize;
    [SerializeField] private GameObject _parent;
    [SerializeField] private bool _isPlayer;

    public ContactFilter2D filter;
    public float damageRadius;
    public int detectCount = 1;
    public bool isDamageCast;

    private Collider2D[] _colliders;
    private Player _player;
    private PlayerBlock _playerBlock;

    private void Awake()
    {
        _colliders = new Collider2D[detectCount];
        _player = GameManager.instance.Player.GetComponent<Player>();
        _playerBlock = GameManager.instance.Player.GetComponent<PlayerBlock>();
    }
    public bool CastDamage(int damage, float knockbackPower, float hpRetakeTime, bool useHitDir = true, bool enemy = false)
    {
        int cnt = 0;
        if(_useBox)
        {
            cnt = Physics2D.OverlapBox(transform.position, _boxSize, _parent.transform.rotation.eulerAngles.z, filter, _colliders);
        }
        else
        {
            cnt = Physics2D.OverlapCircle(transform.position, damageRadius, filter, _colliders);
        }

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

                if(!_isPlayer && _playerBlock.IsBlock)
                {
                    _playerBlock.BlockedDamage();
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _boxSize);
        Gizmos.color = Color.white;
    }
#endif
}

