using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void AnimationEndTrigger()
    {
        _enemy.AnimationEndTrigger();
    }
    private void AnimationAttackTrigger()
    {
        _enemy.Attack();
    }
}
