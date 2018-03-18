using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holeController : MonoBehaviour {

	public int value;
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Ball"))
		{
			ctrl.instance.potBall(other.gameObject,value);
		}
	}
}
