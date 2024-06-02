using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState
{
    protected Boss _boss;
    protected BossStateMachine _stateMachine;

    protected int _animBoolHash;
    protected bool _endTriggerCalled;

    public BossState(Boss boss, BossStateMachine stateMachine, string animBoolName)
    {
        _boss = boss;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animBoolName);
    }
    public virtual void UpdateState()
    {

    }
    public virtual void Enter()
    {
        _boss.AnimatorCompo.SetBool(_animBoolHash, true);
        _endTriggerCalled = false;
    }
    public virtual void Exit()
    {
        _boss.AnimatorCompo.SetBool(_animBoolHash, false);
    }
    public void AnimationEndTrigger()
    {
        _endTriggerCalled = true;
    }
}
