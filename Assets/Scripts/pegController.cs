using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pegController : MonoBehaviour {

	public bool PegOfDoom;

	private Vector3 startVec;
	private Quaternion startQ;

	void Start()
	{
		startVec = GetComponent<Rigidbody>().transform.position;
		startQ = GetComponent<Rigidbody>().rotation;
	}
	void OnTriggerEnter(Collider other)
	{
		ctrl.instance.foul(PegOfDoom);
	}		  
	public void reset()
	{
		Rigidbody R = GetComponent<Rigidbody>();
		R.transform.position = startVec;
		R.rotation = startQ;
		R.velocity = new Vector3(0, 0, 0);
		R.angularVelocity =new  Vector3(0, 0, 0);
		Invoke("StopMovement", .25f);
	}
	private void StopMovement()
	{	  
		Rigidbody R = GetComponent<Rigidbody>();
		R.freezeRotation = true;
		R.isKinematic = true;		   
	}

	public void ResetMovement()
	{
		Rigidbody R = GetComponent<Rigidbody>();
		R.freezeRotation = false;
		R.isKinematic = false;
		R.transform.position = startVec;
		R.rotation = startQ;
		R.velocity = new Vector3(0, 0, 0);
		R.angularVelocity = new Vector3(0, 0, 0);
	}
}
