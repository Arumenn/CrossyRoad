using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 1.0f;
    public float moveDirection = 0;
    public bool parentOnTrigger = true;
    public bool hitBoxOnTrigger = false;
    public GameObject moverObject = null;

    private Renderer _renderer = null;
    private bool isVisible = false;

    private void Start() {
        _renderer = moverObject.GetComponent<Renderer>();
    }

    private void Update() {
        this.transform.Translate(speed * Time.deltaTime, 0, 0);

        IsVisible();
    }

    private void IsVisible() {
        if (_renderer.isVisible) { isVisible = true; }

        if (Manager.GetInstance.IsOutsideLimit(this.transform.position, true)) { 
            //Debug.Log("Mover is off screen. Cleanup.");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("Enter");
            if (parentOnTrigger) {
                Debug.Log("Enter: Parent to me");
                other.transform.parent = this.transform;
                other.GetComponent<PlayerController>().parentedToObject = true;
            }
            if (hitBoxOnTrigger) {
                Debug.Log("Enter: Got hit. Game Over");
                other.GetComponent<PlayerController>().GotHit();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("Exit");
            if (parentOnTrigger) {
                other.transform.parent = null;
                other.GetComponent<PlayerController>().parentedToObject = false;
            }
        }
    }
}
