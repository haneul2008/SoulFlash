using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareAttackState : EnemyState
{
    public NightmareAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (Vector2.Distance(_enemy.targetTrm.position, _enemy.transform.position) > _enemy.attackRadius)
        {
            _stateMachine.ChangeState(EnemyEnum.Chase);
        }
        else
        {
            if(_enemy.DamageCasterCompo.isDamageCast)
            {
                _enemy.DamageCasterCompo.isDamageCast = false;
                _enemy.Attack(false);
                _enemy.lastAttackTime = Time.time;
                _stateMachine.ChangeState(EnemyEnum.Chase);

                SoundManager.instance.AddAudioAndPlay(_enemy.attackSound);
            }
            else
            {
                _enemy.DamageCasterCompo.CastDamage(_enemy.attackDamage, _enemy.knokbackPower, 0.2f, false, true);
            }
        }
    }
}
