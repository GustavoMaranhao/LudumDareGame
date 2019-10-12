using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackResetBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var playerControls = animator.GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.canDamage = true;
            playerControls.bIsAttacking = false;
            playerControls.bShouldDash = false;
            playerControls.baseWeaponObjLeft.ToggleWeaponState(false);
            playerControls.baseWeaponObjRight.ToggleWeaponState(false);
			
			playerControls.rigidBody.simulated = true;
			playerControls.rigidBody.isKinematic = false;
			playerControls.boxCollider.enabled = true;

            animator.ResetTrigger("AttackWithSword");
            animator.ResetTrigger("Dash");
        }

        var enemyControls = animator.GetComponent<BaseEnemy>();
        if (enemyControls)
        {
            enemyControls.canDamage = true;
            enemyControls.isAttacking = false;
            enemyControls.baseWeaponObjLeft.ToggleWeaponState(false);
            enemyControls.baseWeaponObjRight.ToggleWeaponState(false);
        }

        var scytheControls = animator.GetComponent<ScytheEnemy>();
        if (scytheControls)
        {
            scytheControls.canDamage = true;
            scytheControls.isAttacking = false;
            scytheControls.baseWeaponObjLeft.ToggleWeaponState(false);
            scytheControls.baseWeaponObjRight.ToggleWeaponState(false);
        }

        animator.ResetTrigger("Attack");
        animator.ResetTrigger("GoToIdle");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
