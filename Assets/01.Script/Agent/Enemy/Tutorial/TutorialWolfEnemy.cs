using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWolfEnemy : Enemy
{
    [SerializeField] private bool _playerStop;

    public EnemyStateMachine stateMachine;

    private bool _playerStoped;
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        stateMachine.AddState(EnemyEnum.Idle, new TutorialWolfIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Attack, new TutorialWolfAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(EnemyEnum.Dead, new BabyDemonDeadState(this, stateMachine, "Dead"));

        stateMachine.Initialize(EnemyEnum.Idle, this);

        lastAttackTime = -9999;
    }
    private void Update()
    {
        stateMachine.CurrentState.UpdateState();

        if (targetTrm != null && IsDead == false)
        {
            if (Vector2.Distance(new Vector2(targetTrm.position.x, 0), new Vector2(transform.position.x, 0)) > 0.1f)
                HandleSpriteFlip(targetTrm.position, true);
        }
    }
    public override void AnimationEndTrigger()
    {
        stateMachine.CurrentState.AnimationEndTrigger();
    }

    public override void SetDeadState()
    {
        gameObject.layer = _deadLayer;
        stateMachine.ChangeState(EnemyEnum.Dead);
    }

    public void StopPlayer(bool stop)
    {
        if (!_playerStop || _playerStoped) return;

        Player player = GameManager.instance.Player.GetComponent<Player>();

        player.CanStateChageable = !stop;
        player.AnimatorCompo.SetBool("run", !stop);

        player.MovementCompo.canMove = !stop;
        player.MovementCompo.StopImmediately();

        player.dontFlip = stop;

        _playerStoped = stop;

        _playerStoped = !stop;
    }
}
