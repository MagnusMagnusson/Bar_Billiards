using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour {
					  
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Ball"))
		{
			Debug.Log(ctrl.instance);
			Debug.Log(ctrl.instance.balltray);
			Debug.Log(ctrl.instance.balltray.shooting);		
			if (!ctrl.instance.balltray.shooting)
			{
				ctrl.instance.foul(false);
				ctrl.instance.foulBall(other.gameObject, 0);
			}
		}
	}
}
