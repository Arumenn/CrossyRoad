using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour {

    public BuffValues buffs;
    [Header("Game Object")]
    public GameObject buffObject = null;
    public AudioClip audioClip = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerController>().ApplyBuffs(buffs))
            {
                Debug.Log("Player picked up Buff");
                buffObject.SetActive(false);
                GetComponent<AudioSource>().PlayOneShot(audioClip);

                Destroy(this.gameObject, audioClip.length);
            }
        }
    }
}
