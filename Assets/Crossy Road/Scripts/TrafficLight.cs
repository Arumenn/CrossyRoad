using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public GameObject _light = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "train")
        {
            _light.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "train")
        {
            _light.SetActive(false);
        }
    }
}
