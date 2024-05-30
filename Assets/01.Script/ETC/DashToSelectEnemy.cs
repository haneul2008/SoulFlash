using UnityEngine;

public class DashToSelectEnemy : MonoBehaviour
{
    NotifyValue<Collider2D> EnemyCollider = new NotifyValue<Collider2D>();

    [SerializeField] private Player _player;

    [Header("DetectSetting")]
    [SerializeField] private Transform _detectPos;
    [SerializeField] private MouseDetecter _mouseDeteter;
    [SerializeField] private Vector3 _enemyDetectSize;
    [SerializeField] private ContactFilter2D _contact;

    private bool _isSeleting;
    private bool _isFalseBlink;
    private Collider2D[] _enemyDetectCollider;
    private Collider2D _enemyCollider;
    private SpriteRenderer _enemyRenderer;
    private Material _enemyMat;

    private readonly int _isHitHash = Shader.PropertyToID("_IsHit");
    private void Start()
    {
        _enemyDetectCollider = new Collider2D[1];

        GameManager.instance.OnEnemyDeadAction += HandleSelectEnemy;
        EnemyCollider.OnValueChanged += HandleGetRenderer;
    }
    private void OnDisable()
    {
        GameManager.instance.OnEnemyDeadAction -= HandleSelectEnemy;
        EnemyCollider.OnValueChanged -= HandleGetRenderer;
    }

    private void Update()
    {
        if (!_isSeleting) return;

        ToDoEnemy();
    }

    private void HandleSelectEnemy()
    {
        int enemyInDetect = Physics2D.OverlapBox(transform.position, _enemyDetectSize, 0, _contact, _enemyDetectCollider);
        if(enemyInDetect == 0) return;

        _enemyCollider = null;
        Time.timeScale = 0.2f;

        _isSeleting = true;
        _player.CanStateChageable = false;
    }
    private void ToDoEnemy()
    {
        _enemyCollider = _mouseDeteter.DetectEnemy();
        if (!_isFalseBlink && _enemyMat != null && _enemyCollider == null)
        {
            _isFalseBlink = true;
            _enemyMat.SetInt(_isHitHash, 0);
        }

        if (_enemyCollider == null) return;

        _isFalseBlink = false;
        EnemyCollider.Value = _mouseDeteter.DetectEnemy();
        _enemyMat.SetInt(_isHitHash, 1);
    }
    private void HandleGetRenderer(Collider2D prev, Collider2D next)
    {
        if (_enemyRenderer != null)
            _enemyMat.SetInt(_isHitHash, 0);

        _enemyRenderer = next.gameObject.transform.Find("Visual").GetComponent<SpriteRenderer>();
        _enemyMat = _enemyRenderer.material;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(_detectPos.position.x, 0), _enemyDetectSize);
        Gizmos.color = Color.white;
    }
#endif
}
