using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : MonoBehaviour {

    public Color couleur = Color.white;

	// Use this for initialization
	void Start () {
        this.GetComponent<Renderer>().material.SetColor("_Color", couleur);
    }
}
