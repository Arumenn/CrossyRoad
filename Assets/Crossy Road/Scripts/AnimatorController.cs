using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public PlayerController playerController = null;

    private Animator animator = null;

    private void Start() {
        animator = this.GetComponent<Animator>();
    }

    private void Update() {

        animator.SetBool("dead", playerController.isDead);
        
        if (playerController.jumpStart) {
            animator.SetBool("jumpStart", true);
        } else if (playerController.isJumping) {
            animator.SetBool("jump", true);
        } else {
            animator.SetBool("jumpStart", false);
            animator.SetBool("jump", false);
        }
    }
}
