using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyEnum
{
    Apear,
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
        /*stateMachine.AddState(EnemyEnum.Apear, new ZombieApearState(this, stateMachine, "Air"));
        stateMachine.AddState(EnemyEnum.Idle, new ZombieIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Chase, new ZombieChaseState(this, stateMachine, "Chase"));
        stateMachine.AddState(EnemyEnum.Attack, new ZombieAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(EnemyEnum.Dead, new ZombieDeadState(this, stateMachine, "Dead"));*/

        //시작상태를 설정해서 준비
        stateMachine.Initialize(EnemyEnum.Apear, this);
    }
    private void Update()
    {
        stateMachine.CurrentState.UpdateState();

        if (targetTrm != null && IsDead == false)
            HandleSpriteFlip(targetTrm.position);
    }
    public override void AnimationEndTrigger()
    {
    }

    public void ResetItem()
    {
    }

    public override void SetDeadState()
    {
    }
}
