using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	private float timer;
	private Raytracer raytracer;
	public List<GameObject> holes;	 

	private List<Move> directShots;
	private List<Move> indirectShots;

	public bool toggle;
	private bool lastToggle;
	// Use this for initialization
	void Start () {
		raytracer = new Raytracer();
		lastToggle = toggle;
		directShots = new List<Move>();
		indirectShots = new List<Move>();
	}

	void Update()
	{
		if (toggle != lastToggle)
		{
			lastToggle = toggle;
			MakeMove();
		}
	}

	void MakeMove()
	{
		GetIndirectShots();
		GetDirectShots(); 

	}

	void GetDirectShots()
	{
		directShots.Clear();
		GameObject myBall = ctrl.instance.balltray.getCurrentBall();
		GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");
		foreach (GameObject ball in allBalls)
		{
			if (ball == myBall) continue;
			foreach (GameObject hole in holes)
			{
				Vector3 holePos = hole.transform.position;
				holePos.y = ball.transform.position.y;
				Move maybe = PotentialGoodShot(holePos, ball, myBall);
				if (maybe != null && maybe.angle.z <= 0)
                {
                    Debug.Log("In " + maybe);
                    directShots.Add(maybe);
				}
			}
		}											  
	}

	void GetIndirectShots()
	{
		indirectShots.Clear();
		GameObject myBall = ctrl.instance.balltray.getCurrentBall();
		GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");
		foreach (GameObject ball in allBalls)
		{
			if (ball == myBall) continue;
			foreach (GameObject hole in holes)
			{
				Vector3 holePos = hole.transform.position;
				holePos.y = ball.transform.position.y;
				Move maybe = PotentialIndirectShot(holePos, ball, myBall);
				if (maybe != null && maybe.angle.z <= 0)
				{
                    Debug.Log("In " + maybe);
					indirectShots.Add(maybe);
				}
			}
		}												
	}

	private Move PotentialIndirectShot(Vector3 hole, GameObject testBall, GameObject myBall)
	{
		Raytracer.ObjectAngle result = raytracer.getTriangle(hole,testBall, myBall);
		if (result.Object == myBall)
		{
			Move m = new Move();
			m.angle =  result.DirectionTo;
			Vector3 theta = (hole - testBall.transform.position).normalized;
			Vector3 gamma = (testBall.transform.position - myBall.transform.position).normalized; 
			m.difficulty = Vector3.Angle(theta, gamma)/180f;
			return m;
		}
		return null;
	}

	Move PotentialGoodShot(Vector3 hole, GameObject testBall,GameObject myBall)
	{
		Vector3 to = (hole - testBall.transform.position).normalized;
		raytracer.SetParam(hole, -to);
		Raytracer.ObjectAngle result = raytracer.CastIgnore(testBall);	
		if (result.Object == myBall)
		{
			Move m = new Move();
			m.angle = result.DirectionTo;
			Vector3 theta = (hole - testBall.transform.position).normalized;
			Vector3 gamma = (testBall.transform.position - myBall.transform.position).normalized;
			Debug.Log(Vector3.Angle(Vector3.up, Vector3.up));
			m.difficulty = Vector3.Angle(theta, gamma) / 180f;
			return m;
		}
		return null;
	}

	private class Move
	{
		public Vector3 angle;
		public float difficulty;	

	}
}
