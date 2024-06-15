using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    #region Component
    public AgentMovement MovementCompo { get; protected set; }
    public Animator AnimatorCompo { get; protected set; }
    public Health HealthCompo { get; protected set; }
    public SpriteRenderer SpriteRendererCompo { get; protected set; }
    #endregion

    public bool IsDead { get; protected set; }

    public bool isSpawnAgent;
    protected float _timeInAir;

    [SerializeField] private bool _dontSetVisual;

    [HideInInspector] public bool dontFlip;
    protected virtual void Awake()
    {
        MovementCompo = GetComponent<AgentMovement>();
        MovementCompo.Initalize(this);

        if(!_dontSetVisual)
        {
            AnimatorCompo = transform.Find("Visual").GetComponent<Animator>();
            SpriteRendererCompo = transform.Find("Visual").GetComponent<SpriteRenderer>();
        }

        HealthCompo = GetComponent<Health>();
        HealthCompo.Initialize(this);
    }

    public abstract void SetDeadState();

    #region Flip Character
    public bool IsFacingRight()
    {
        return Mathf.Approximately(transform.eulerAngles.y, 0);
    }
    public void HandleSpriteFlip(Vector3 targetPosition, bool flip = false)
    {
        if (targetPosition.x < transform.position.x)
        {
            if(flip)
                transform.eulerAngles = Vector3.zero;
            else
                transform.eulerAngles = new Vector3(0, -180f, 0);
        }
        else if (targetPosition.x > transform.position.x)
        {
            if (!flip)
                transform.eulerAngles = Vector3.zero;
            else
                transform.eulerAngles = new Vector3(0, -180f, 0);
        }
    }
    #endregion
}
