using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCombo : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAttack", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            animator.SetBool("isAttack", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAttack", false);
    }
}
