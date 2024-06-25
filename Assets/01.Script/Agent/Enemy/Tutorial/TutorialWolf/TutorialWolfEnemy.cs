using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWolfEnemy : Enemy
{
    [SerializeField] private EnemyGuideEvent _enemyGuideEvent;
    [SerializeField] private bool _playerStop;
    [field: SerializeField] public bool IsBlockGuide { get; private set; }

    public EnemyStateMachine stateMachine;

    private bool _playerStoped;
    public bool Attacked { get; private set; }
    private Vector2 _originPos;
    private Player _player;
    private bool _blocked;
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        stateMachine.AddState(EnemyEnum.Idle, new TutorialWolfIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(EnemyEnum.Attack, new TutorialWolfAttackState(this, stateMachine, "Attack"));
        stateMachine.AddState(EnemyEnum.Dead, new TutorialWolfDeadState(this, stateMachine, "Dead"));

        stateMachine.Initialize(EnemyEnum.Idle, this);

        lastAttackTime = -9999;

        _originPos = transform.position;

        _player = GameManager.instance.Player.GetComponent<Player>();
    }
    private void Update()
    {
        stateMachine.CurrentState.UpdateState();

        if (targetTrm != null && IsDead == false)
        {
            if (Vector2.Distance(new Vector2(targetTrm.position.x, 0), new Vector2(transform.position.x, 0)) > 0.1f && !dontFlip)
                HandleSpriteFlip(targetTrm.position, true);
        }
    }
    public override void AnimationEndTrigger()
    {
        stateMachine.CurrentState.AnimationEndTrigger();

        if (Attacked)
        {
            Attacked = false;
            if(IsBlockGuide) transform.position = _originPos;
            if (_blocked)
            {
                stateMachine.ChangeState(EnemyEnum.Idle);

                MovementCompo.StopImmediately();

                CanStateChageable = false;
                SpriteRendererCompo.DOFade(0, 0.5f)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            }
        }
    }
    public override void Attack(bool castDamage = true)
    {
        base.Attack(castDamage);

        Attacked = true;
    }

    public override void SetDeadState()
    {
        CanStateChageable = true;

        gameObject.layer = _deadLayer;
        stateMachine.ChangeState(EnemyEnum.Dead);

        if(_enemyGuideEvent != null) _enemyGuideEvent.SetGuide();
    }

    public void StopPlayer(bool stop)
    {
        if (!IsBlockGuide && (!_playerStop || _playerStoped)) return;

        if(!IsBlockGuide) _player.CanStateChangable = !stop;
        _player.AnimatorCompo.SetBool("run", !stop);

        _player.MovementCompo.canMove = !stop;
        _player.MovementCompo.StopImmediately();

        _player.dontFlip = stop;

        _playerStoped = !stop;
    }

    public void Blocked()
    {
        _player.CanStateChangable = true;

        _player.MovementCompo.canMove = true;

        _player.dontFlip = false;

        _blocked = true;
    }
}
