using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    private bool hitCheckpoint = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            Debug.Log("Player CHECKPOINT");
            playerController.SaveCheckpoint();
        }
    }
}
