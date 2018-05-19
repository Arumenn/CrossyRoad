using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public GameObject coin = null;
    public AudioClip audioClip = null;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            string p = other.GetComponent<PlayerController>().controllerPrefix;
            Debug.Log("Player " + p + " picked up Coin");
            Manager.GetInstance.UpddateCoinCount(p, coinValue);


            coin.SetActive(false);
            GetComponent<AudioSource>().PlayOneShot(audioClip);

            Destroy(this.gameObject, audioClip.length);
        }
    }
}
