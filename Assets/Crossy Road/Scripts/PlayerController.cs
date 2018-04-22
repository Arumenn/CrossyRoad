using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveTime = 0.4f;
    public float colliderDistCheck = 1;
    public bool isDead = false;
    public bool isIdle = true;
    public bool isMoving = false;
    public bool jumpStart = false;
    public bool isJumping = false;
    public ParticleSystem particle = null;
    public GameObject chick = null;

    private Renderer renderer = null;
    private bool isVisible = false;

    void Start() {
        renderer = chick.GetComponent<Renderer>();
    }

    void Update() {
        //TODO Manager -> CanPlay()
        if (isDead) { return; }

        CanIdle();
        CanMove();

        IsVisible();
    }

    void CanIdle() {
        if (isIdle) {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
                CheckIfCanMove();
            }
        }
    }

    void CheckIfCanMove() {
        //raycast - find if there's any collider box in front of player
        RaycastHit hit;
        Physics.Raycast(this.transform.position, -chick.transform.up, out hit, colliderDistCheck);
        Debug.DrawRay(this.transform.position, -chick.transform.up * colliderDistCheck, Color.red, 2f);

        if (hit.collider == null) {
            SetMove();
        }else {
            if (hit.collider.tag == "collider") {
                Debug.Log("Hit something with collider tag");
            } else {
                SetMove();
            }
        }
    }

    void SetMove() {
        Debug.Log("Hit nothing. Keep moving.");
        isIdle = false;
        isMoving = true;
        jumpStart = true;
    }

    void CanMove() {
        if (isMoving) {
            if (Input.GetKeyUp(KeyCode.UpArrow)) {
                Move(new Vector3(transform.position.x, transform.position.y, transform.position.z + moveDistance));
                SetMoveForwardState();
            } else if (Input.GetKeyUp(KeyCode.DownArrow)) {
                Move(new Vector3(transform.position.x, transform.position.y, transform.position.z - moveDistance));
            } else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                Move(new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z));
            } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
                Move(new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z));
            }
        }
    }

    void Move(Vector3 pos) {
        isIdle = false;
        isMoving = false;
        isJumping = true;
        jumpStart = false;
        LeanTween.move(this.gameObject, pos, moveTime).setOnComplete(MoveComplete);
    }

    void MoveComplete() {
        isIdle = true;
        isMoving = false;
        isJumping = false;
        jumpStart = false;
    }

    void SetMoveForwardState() {

    }

    void IsVisible() {
        if (renderer.isVisible) { isVisible = true; }

        if (!renderer.isVisible && isVisible) {
            Debug.Log("Player is off screen. Dies");
            GotHit();
        }
    }

    public void GotHit() {
        isDead = true;
        isIdle = false;
        ParticleSystem.EmissionModule em = particle.emission;
        em.enabled = true;

        //TODO Manager => GameOver()
    }
}
