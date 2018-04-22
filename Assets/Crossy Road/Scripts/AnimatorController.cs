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
        if (playerController.isDead) {
            animator.SetBool("dead", true);
        }
        if (playerController.jumpStart) {
            animator.SetBool("jumpStart", true);
        } else if (playerController.isJumping) {
            animator.SetBool("jump", true);
        } else {
            animator.SetBool("jumpStart", false);
            animator.SetBool("jump", false);
        }

        if (!playerController.isIdle) { return; }

        if (Input.GetKeyDown(KeyCode.UpArrow)) { this.transform.rotation = Quaternion.Euler(270f, 0, 0); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { this.transform.rotation = Quaternion.Euler(270f, 180f, 0); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { this.transform.rotation = Quaternion.Euler(270f, -90f, 0); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { this.transform.rotation = Quaternion.Euler(270f, 90f, 0); }
    }
}
