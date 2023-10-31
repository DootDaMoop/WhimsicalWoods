using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 targetPos;
    private Vector3 direction;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.animator.Play("slime_idle");
        targetPos = GetRandomPointInCircle();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(enemy.isAggro) {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
        
        if(enemy.collisionHit) {
            targetPos = GetRandomPointInCircle();
            enemy.collisionHit = false;
        }

        direction = (targetPos - enemy.transform.position).normalized;
        enemy.MoveEnemy(direction * enemy.randomMovementSpeed);
        if((enemy.transform.position - targetPos).sqrMagnitude < 0.01f) {
            targetPos = GetRandomPointInCircle();
        }
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.AnimationTriggerEvent(Enemy.AnimationTriggerType.EnemyIdle);
    }

    private Vector3 GetRandomPointInCircle() {
        return enemy.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * enemy.randomMovementRange;
    }
}
