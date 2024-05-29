using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyEnum
{
    Appear,
    Idle,
    Chase,
    Attack,
    Dead
}
public class GhostEnemy : Enemy, IPoolable
{
    [SerializeField] private string _poolName;
    public string PoolName => _poolName;

    public GameObject ObjectPrefab => gameObject;

    public EnemyStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        //시작 상태 추가
        stateMachine.AddState(EnemyEnum.Appear, new GhostAppearState(this, stateMachine, "Appear"));
        stateMachine.AddState(EnemyEnum.Idle, new GhostIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Chase, new GhostChaseState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Attack, new GhostAttackState(this, stateMachine, "Attack"));
        //stateMachine.AddState(EnemyEnum.Dead, new ZombieDeadState(this, stateMachine, "Dead"));

        //시작상태를 설정해서 준비
        stateMachine.Initialize(EnemyEnum.Appear, this);

        lastAttackTime = -9999f;
    }
    private void Update()
    {
        stateMachine.CurrentState.UpdateState();

        if (targetTrm != null && IsDead == false)
        {
            if(Vector2.Distance(targetTrm.position, transform.position) > 0.8f)
                HandleSpriteFlip(targetTrm.position, true);
        }
    }
    public override void AnimationEndTrigger()
    {
        stateMachine.CurrentState.AnimationEndTrigger();
    }

    public void ResetItem()
    {
        SpriteRendererCompo.color = new Color(1, 1, 1, 0);
    }

    public override void SetDeadState()
    {
    }
}
