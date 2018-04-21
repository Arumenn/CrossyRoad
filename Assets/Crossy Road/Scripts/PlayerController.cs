using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveTime = 0.4f;
    public float colliderDistCheck = 1;
    public bool isIdle = true;
    public bool isDead = false;
    public bool isMoving = false;
    public bool isJumping = false;
    public bool jumpStart = false;
    public ParticleSystem particle = null;
    public GameObject chick = null;

    private Renderer renderer = null;
    private bool isVisible = false;

    void Start() {
        
    }

    void Update() {
        CanMove();

    }

    void CanIdle() {

    }

    void CheckIfCanMove() {

    }

    void SetMove() {

    }

    void CanMove() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Move(new Vector3(transform.position.x, transform.position.y, transform.position.z + moveDistance));
        }else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            Move(new Vector3(transform.position.x, transform.position.y, transform.position.z - moveDistance));
        }else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Move(new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z));
        }else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Move(new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z));
        }
    }

    void Move(Vector3 pos) {
        LeanTween.move(this.gameObject, pos, moveTime);
    }

    void MoveComplete() {

    }

    void SetMoveForwardState() {

    }

    void IsVisible() {

    }

    public void GotHit() {

    }
}
