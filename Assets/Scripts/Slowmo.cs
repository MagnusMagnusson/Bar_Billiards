using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowmo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Time.timeScale = 0.1f;
		Time.fixedDeltaTime = 0.2f * Time.timeScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
