using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public GameObject player;
    private float movementSpeed = 1.75f;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy.animator.SetBool("ChaseState",true);
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.animator.SetBool("ChaseState",false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(!enemy.isAggro) {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }

        if(enemy.isWithinAttackRadius) {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }

        Vector2 moveDirection = (player.transform.position - enemy.transform.position).normalized;
        enemy.MoveEnemy(moveDirection * movementSpeed);
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
