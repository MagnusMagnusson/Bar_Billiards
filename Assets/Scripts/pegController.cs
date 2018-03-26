using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pegController : MonoBehaviour {

	public bool PegOfDoom;

	private Vector3 startVec;
	private Quaternion startQ;

    private bool frozen;

	void Start()
	{
		startVec = GetComponent<Rigidbody>().transform.position;
        startQ = Quaternion.Euler(0, 0, 0);
        frozen = false;
	}

    private void Update()
    {
        if (frozen)
        { 
            Rigidbody R = GetComponent<Rigidbody>();
            R.rotation = startQ;
            R.transform.position = startVec;
            R.velocity = new Vector3(0, 0, 0);
            R.angularVelocity = new Vector3(0, 0, 0);
        }
    }
    void OnTriggerEnter(Collider other)
	{
		ctrl.instance.foul(PegOfDoom);
	}		  
	public void reset()
	{
        Rigidbody R = GetComponent<Rigidbody>();
        frozen = true;
        R.velocity = new Vector3(0, 0, 0);
		R.angularVelocity =new Vector3(0, 0, 0);
	}

	public void ResetMovement()
	{
        frozen = false;
	}
}
