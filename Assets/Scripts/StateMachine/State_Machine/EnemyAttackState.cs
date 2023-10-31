using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public GameObject player;
    private float movementSpeed = 2f;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(!enemy.isWithinAttackRadius) {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }

        Vector2 moveDirection = (player.transform.position - enemy.transform.position).normalized;
        enemy.MoveEnemy(moveDirection * movementSpeed);
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

}
