using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    #region Component
    public AgentMovement MovementCompo { get; protected set; }
    public Animator AnimatorCompo { get; protected set; }
    public Health HealthCompo { get; protected set; }
    #endregion

    public bool IsDead { get; protected set; }

    protected float _timeInAir;
    protected virtual void Awake()
    {
        MovementCompo = GetComponent<AgentMovement>();
        MovementCompo.Initalize(this);
        AnimatorCompo = transform.Find("Visual").GetComponent<Animator>();

        HealthCompo = GetComponent<Health>();
        HealthCompo.Initialize(this);
    }

    public abstract void SetDeadState();

    #region Flip Character
    public bool IsFacingRight()
    {
        return Mathf.Approximately(transform.eulerAngles.y, 0);
    }
    public void HandleSpriteFlip(Vector3 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, -180f, 0);
        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
    #endregion
}
