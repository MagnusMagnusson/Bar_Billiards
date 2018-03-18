using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballController : MonoBehaviour {

	public bool red;
	public int multiplier;
	public float HighSpeed;
	public float LowSpeed;
	public float speed;

	private Rigidbody Rig;	

	void Start()
	{				 
		Rig = gameObject.GetComponent<Rigidbody>();
	}
	void Update()
	{
		speed = Rig.velocity.magnitude;
		if (speed > HighSpeed)
		{
			Rig.drag = 0.1f;
		}
		else {
			if (speed < LowSpeed && speed > 0.05)
			{
				Rig.drag = 2f;
			}
			else
			{
				if(0.02f < speed && speed < 0.05f)
				{
					Rig.velocity = new Vector3(0, 0, 0);
					Rig.angularVelocity = new Vector3(0, 0, 0);
				}
				Rig.drag = 0.5f;
			}
		}
	}

	public bool isStopped()
	{
		return Rig.velocity.magnitude <= 0.012;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.collider.gameObject.CompareTag("Ball"))
		{
			ctrl.instance.setLegal();
		}
	}
}
