using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject chick = null;
    [Header("Movement")]
    public string controllerPrefix = "P1";
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
    public bool isSoaked = false;
    [Header("Particles")]
    public ParticleSystem particleDeath = null;
    public ParticleSystem particleSplash = null;
    public ParticleSystem particleSpeed = null;
    [Header("Audio")]
    public AudioClip audioIdle1 = null;
    public AudioClip audioIdle2 = null;
    public AudioClip audioHop = null;
    public AudioClip audioHit = null;
    public AudioClip audioSplash = null;
    public AudioClip audioCheckpoint = null;

    private Vector3 lastCheckPointPos = Vector3.zero;

    private Renderer _renderer = null;
    private bool isVisible = false;
    private bool isBuffed = false;

    private void Start() {
        _renderer = chick.GetComponent<Renderer>();
    }

    public void Setup()
    {
        if (controllerPrefix == "P2")
        {
            if (Manager.GetInstance.multiplayer)
            {
                _renderer.material.SetColor("_Color", Color.red);
                Debug.Log(MetaManager.GetInstance.nomPlayer2);
            } else
            {
                this.gameObject.SetActive(false);
            }
        } else
        {
            if (Manager.GetInstance.multiplayer)
            {
                Debug.Log(MetaManager.GetInstance.nomPlayer1);
            }
        }
        lastCheckPointPos = transform.position;
    }

    private void Update() {
        if (!Manager.GetInstance.CanPlay()) { return; }
        if (isDead) { return; }

        if (!Manager.GetInstance.IsOutsideLimit(this.transform.position, false))
        {
            CanIdle();
            CanMove();
        }
        IsVisible();
    }

    private bool IsOutsideLimit()
    {
        if (Mathf.Abs(transform.position.x) >= Manager.GetInstance.outerLimitsX)
        {
            GotHit();
            return true;
        }
        if (transform.position.z <= Manager.GetInstance.outerLimitZ)
        {
            GotHit();
            return true;
        }
        return false;
    }

    private void CanIdle() {
        if (isIdle) {
            if (Input.GetButtonDown(controllerPrefix + "_Up"))      { CheckIfIdle(270f, 0, 0); }
            if (Input.GetButtonDown(controllerPrefix + "_Down"))    { CheckIfIdle(270f, 180f, 0); }
            if (Input.GetButtonDown(controllerPrefix + "_Left"))    { CheckIfIdle(270f, -90f, 0); }
            if (Input.GetButtonDown(controllerPrefix + "_Right"))   { CheckIfIdle(270f, 90f, 0); }
        }
    }

    private void CheckIfIdle(float x, float y, float z)
    {
        chick.transform.rotation = Quaternion.Euler(x, y, z);

        CheckIfCanMove();

        int a = UnityEngine.Random.Range(0, 12);
        if (a < 4) { PlayAudio(audioIdle1); }
    }

    private void CheckIfCanMove() {
        //raycast - find if there's any collider box in front of player
        RaycastHit hit;
        Physics.Raycast(this.transform.position, -chick.transform.up, out hit, moveDistance);
        Debug.DrawRay(this.transform.position, -chick.transform.up * moveDistance, Color.red, 2f);

        if (hit.collider == null) {
            SetMove();
        }else {
            if (hit.collider.tag == "collider") {
                //Debug.Log("Hit something with collider tag");
                isIdle = true;
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
            if (Input.GetButtonUp(controllerPrefix + "_Up")) {
                Move(new Vector3(transform.position.x, transform.position.y, transform.position.z + moveDistance));
                SetMoveForwardState();
            } else if (Input.GetButtonUp(controllerPrefix + "_Down")) {
                Move(new Vector3(transform.position.x, transform.position.y, transform.position.z - moveDistance));
                SetMoveBackwardState();
            } else if (Input.GetButtonUp(controllerPrefix + "_Left")) {
                Move(new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z));
            } else if (Input.GetButtonUp(controllerPrefix + "_Right")) {
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
        int a = UnityEngine.Random.Range(0, 12);
        if (a < 4) { PlayAudio(audioIdle2); }
        if (!parentedToObject) { SnapToGrid(); }
    }

    private void SnapToGrid()
    {
        Vector3 curPos = this.transform.position;
        transform.position = new Vector3(Mathf.Round(curPos.x), curPos.y, (float)Math.Round(curPos.z, 1));
    }

    private void SetMoveForwardState() {
        Manager.GetInstance.UpdateDistanceCount(controllerPrefix, moveDistance);
    }
    private void SetMoveBackwardState()
    {
        Manager.GetInstance.UpdateDistanceCount(controllerPrefix, -moveDistance);
    }

    private void IsVisible() {
        if (_renderer.isVisible) { isVisible = true; }

        if (!_renderer.isVisible && isVisible) {
            //Debug.Log("Player is off screen. Dies");
            //GotHit();
        }
    }

    public void GotHit() {
        ParticleSystem.EmissionModule em = particleDeath.emission;
        em.enabled = true;
        PlayAudio(audioHit);
        //Manager.GetInstance.GameOver();
        HasDied();
    }

    public void GotSoaked()
    {
        if (!isSoaked)
        {
            isSoaked = true;
            ParticleSystem.EmissionModule em = particleSplash.emission;
            em.enabled = true;
            PlayAudio(audioSplash);
            chick.SetActive(false);
            //Manager.GetInstance.GameOver();
            HasDied();
        }
    }

    public void HasDied()
    {
        if (!isDead)
        {
            isDead = true;
            isIdle = false;
            StartCoroutine("RespawnAtCheckpoint", 2f);
        }
    }

    private void PlayAudio(AudioClip clip)
    {
        this.GetComponent<AudioSource>().PlayOneShot(clip);
    }


    public bool ApplyBuffs(BuffValues buffs)
    {
        if (isBuffed) { return false; }
        //Debug.Log("applying buff! " + buffs.ToString());
        moveDistance += buffs.AddToJumpDistance;
        Camera.main.GetComponent<CameraFollow>().speed = moveDistance;

        if (buffs.AddToJumpDistance > 0)
        {
            ParticleSystem.EmissionModule em = particleSpeed.emission;
            em.enabled = true;
        }

        StartCoroutine("RemoveBuffs", buffs);
        isBuffed = true;
        return true;
    }

    public IEnumerator RemoveBuffs(BuffValues buffs)
    {
        yield return new WaitForSecondsRealtime(buffs.duration);
        //Debug.Log("removing buff! " + buffs.ToString());
        moveDistance -= buffs.AddToJumpDistance;
        Camera.main.GetComponent<CameraFollow>().speed = moveDistance;

        if (buffs.AddToJumpDistance > 0)
        {
            ParticleSystem.EmissionModule em = particleSpeed.emission;
            em.enabled = false;
        }

        isBuffed = false;
    }

    public void SaveCheckpoint()
    {
        lastCheckPointPos = transform.position;
        PlayAudio(audioCheckpoint);
        Debug.Log("Save checkpoint for " + controllerPrefix + " at " + lastCheckPointPos.ToString());
    }

    public IEnumerator RespawnAtCheckpoint(float waitForRespawn)
    {
        yield return new WaitForSecondsRealtime(waitForRespawn);
        isDead = false;
        isIdle = true;
        isSoaked = false;
        ParticleSystem.EmissionModule em = particleDeath.emission;
        em.enabled = false;
        em = particleSplash.emission;
        em.enabled = false;
        chick.SetActive(true);

        transform.position = lastCheckPointPos;
        PlayAudio(audioCheckpoint);
        Debug.Log("Respaw at checkpoint for " + controllerPrefix + " at " + lastCheckPointPos.ToString());
    }
}
