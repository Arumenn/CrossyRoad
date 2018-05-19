using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (!playerController.parentedToObject && !playerController.isJumping && !playerController.isSoaked)
            {
                Debug.Log("Player fell into water");

                playerController.GotSoaked();
            }
        }
    }
}
