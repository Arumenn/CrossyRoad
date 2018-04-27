using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject chick = null;
    [Header("Movement")]
    public float moveDistance = 1f;
    public float moveTime = 0.4f;
    public float colliderDistCheck = 1;
    [Header("States")]
    public bool isDead = false;
    public bool isIdle = true;
    public bool isMoving = false;
    public bool jumpStart = false;
    public bool isJumping = false;
    public bool parentedToObject = false;
    [Header("Particles")]
    public ParticleSystem particleDeath = null;
    public ParticleSystem particleSplash = null;
    [Header("Audio")]
    public AudioClip audioIdle1 = null;
    public AudioClip audioIdle2 = null;
    public AudioClip audioHop = null;
    public AudioClip audioHit = null;
    public AudioClip audioSplash = null;
    

    private Renderer _renderer = null;
    private bool isVisible = false;

    private void Start() {
        _renderer = chick.GetComponent<Renderer>();
    }

    private void Update() {
        if (!Manager.GetInstance.CanPlay()) { return; }
        if (isDead) { return; }

        CanIdle();
        CanMove();

        IsVisible();
    }

    private void CanIdle() {
        if (isIdle) {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
                CheckIfCanMove();
                PlayAudio(audioIdle1);
            }
        }
    }

    private void CheckIfCanMove() {
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

    private void SetMove() {
        isIdle = false;
        isMoving = true;
        jumpStart = true;
    }

    private void CanMove() {
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

    private void Move(Vector3 pos) {
        isIdle = false;
        isMoving = false;
        isJumping = true;
        jumpStart = false;
        PlayAudio(audioHop);
        LeanTween.move(this.gameObject, pos, moveTime).setOnComplete(MoveComplete);
    }

    private void MoveComplete() {
        isIdle = true;
        isMoving = false;
        isJumping = false;
        jumpStart = false;
        PlayAudio(audioIdle2);
    }

    private void SetMoveForwardState() {
        Manager.GetInstance.UpdateDistanceCount();
    }

    private void IsVisible() {
        if (_renderer.isVisible) { isVisible = true; }

        if (!_renderer.isVisible && isVisible) {
            Debug.Log("Player is off screen. Dies");
            GotHit();
        }
    }

    public void GotHit() {
        isDead = true;
        isIdle = false;
        ParticleSystem.EmissionModule em = particleDeath.emission;
        em.enabled = true;
        PlayAudio(audioHit);
        Manager.GetInstance.GameOver();
    }

    public void GotSoaked()
    {
        isDead = true;
        isIdle = false;
        ParticleSystem.EmissionModule em = particleSplash.emission;
        em.enabled = true;
        PlayAudio(audioSplash);
        chick.SetActive(false);
        Manager.GetInstance.GameOver();
    }

    private void PlayAudio(AudioClip clip)
    {
        this.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
