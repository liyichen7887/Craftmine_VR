using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Camera>().enabled = false;
        this.GetComponent<Camera>().enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
