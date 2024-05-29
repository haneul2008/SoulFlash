using UnityEngine;

public class AnimationPlayer: MonoBehaviour
{
    [SerializeField] protected string _parameterName;
    [SerializeField] protected bool _isTriggerAnim;
    [SerializeField] protected bool _isHaveMoveAnim = true;

    protected Agent _agent;
    protected Animator _anim;

    private int _hash;
    private float _saveAnimSpeed;
    public virtual void Initialize(Agent agent)
    {
        if (!_isHaveMoveAnim) return;

        _agent = agent;
        _anim = _agent.transform.Find("Visual").GetComponent<Animator>();
        _hash = Animator.StringToHash(_parameterName);
        _saveAnimSpeed = _anim.speed;
    }
    protected virtual void PlayAnimation(bool isStoped = false)
    {
        if (!_isHaveMoveAnim) return;

        if (isStoped)
        {
            _anim.speed = _saveAnimSpeed;
            return;
        }

        if (_isTriggerAnim)
            _anim.SetTrigger(_hash);
        else
            _anim.SetBool(_hash, true);
    }
    protected virtual void StopAnimation()
    {
        if (!_isHaveMoveAnim) return;

        _saveAnimSpeed = _anim.speed;
        _anim.speed = 0;
    }
    protected virtual void EndAnimation()
    {
        if (!_isHaveMoveAnim) return;

        _anim.SetBool(_hash, false);
    }
}
