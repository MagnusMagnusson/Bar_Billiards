using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	private float timer;
	private Raytracer raytracer;
	public List<GameObject> holes;
	public GameObject Plane;

	public bool toggle;
	private bool lastToggle;
	// Use this for initialization
	void Start () {
		raytracer = new Raytracer();
		lastToggle = toggle;
	}

	void Update()
	{
		if (toggle != lastToggle)
		{
			lastToggle = toggle;
			GameObject myBall = ctrl.instance.balltray.getCurrentBall();
			GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");
			Vector3 v = Plane.transform.position;
			Quaternion q = Plane.transform.rotation;
			Plane.transform.SetPositionAndRotation(v - new Vector3(0,7,0),q);
			foreach (GameObject ball in allBalls)
			{				  
				if(ball == myBall) continue;
				foreach (GameObject hole in holes)
				{
					Vector3 holePos = hole.transform.position;
					holePos.y = ball.transform.position.y;
					Vector3 maybe = potentialGoodShot(holePos, ball, myBall);

					Debug.Log(maybe + " : " + ball.transform.position + " to " + holePos);
				}
			}
			Plane.transform.SetPositionAndRotation(v, q);
		}
	}

	Vector3 potentialGoodShot(Vector3 hole, GameObject testBall,GameObject myBall)
	{
		Vector3 to = (hole - testBall.transform.position).normalized;
		raytracer.setParam(hole, -to);
		Raytracer.objectAngle result = raytracer.castIgnore(testBall);			
		Debug.Log(result.Object);
		if (result.Object == Plane)
		{													  
			return result.DirectionTo;
		}												  
		return Vector3.zero;	
	} 
}
