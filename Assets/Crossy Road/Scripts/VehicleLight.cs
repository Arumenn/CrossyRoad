using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.SetActive(Manager.GetInstance.isNight);
	}
}
