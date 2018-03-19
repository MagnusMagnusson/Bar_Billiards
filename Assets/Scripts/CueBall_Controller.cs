using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBall_Controller : MonoBehaviour { 
	public bool shooting;
	public int freeWhite;
	public int freeRed;
	public bool barUp;

	public GameObject ballMarker;
	public GameObject whiteBall;
	public GameObject redBall;

	private List<GameObject> myballs;
	private GameObject nowshooting;
	public LineRenderer Render;
	private float force;
	Vector3 End;

	public float maxForce;
	public float forceAdd;
	private int forceDir;

	// Use this for initialization
	void Start () {
		barUp = true;												 
		myballs = new List<GameObject>();
		End = new Vector3(0,0,0);
		force = 0;
		forceDir = 1;

		if (freeRed > 0)
		{
			GameObject BreakBall = Instantiate(redBall);
			Vector3 bm = ballMarker.transform.position;
			BreakBall.transform.position = new Vector3(bm.x, bm.y, bm.z - 9);
			freeRed--;
			myballs.Add(BreakBall);
		}
		else
		{
			GameObject BreakBall = Instantiate(whiteBall);
			Vector3 bm = ballMarker.transform.position;
			BreakBall.transform.position = new Vector3(bm.x, bm.y, bm.z - 9);
			freeWhite--;
			myballs.Add(BreakBall);
		}

		if (shooting){
			nextShot();
		}										 
	}

	void Update() {
		if (!ctrl.instance.playing)
		{						   
			Render.SetPosition(0, new Vector3(0,0,0));
			Render.SetPosition(0, new Vector3(0, 0, 0));
			return;
		}
		if (shooting)
		{
			Vector3 bball = nowshooting.transform.position;
			Ray R = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit RR = new RaycastHit();
			Debug.DrawRay(R.origin, R.direction * 50, Color.green);
			if (force == 0 && Physics.Raycast(R,out RR))
			{														

				Vector3 dir = RR.point - bball;
				float dist = Mathf.Clamp(Vector3.Distance(bball, RR.point), 0, 6);
				RR.point = bball + (dir.normalized * dist);

				Render.positionCount = 2;
				Render.SetPosition(0, nowshooting.transform.position);
				Render.SetPosition(1, new Vector3(RR.point.x, bball.y,RR.point.z));

				if (Input.GetMouseButtonDown(0))
				{
					End = RR.point;
					force = 0.1f;
				}

			}

			if (force != 0)
			{
				Vector3 dir = End - bball;
				float dist = Mathf.Clamp((force/maxForce) * 11, 0, 12);
				Vector3 EEnd;
				EEnd = bball + (dir.normalized * dist);

				Render.positionCount = 2;
				Render.SetPosition(0, nowshooting.transform.position);
				Render.SetPosition(1, new Vector3(EEnd.x, bball.y, EEnd.z));

				force += forceAdd * forceDir;	 
				if(force > maxForce)
				{
					forceDir = -1;
				}
				if(force < 0)
				{
					forceDir = 1;
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				myballs.Add(nowshooting);
				Vector3 Force = new Vector3(End.x, bball.y, End.z) - bball;
				Force = Force.normalized * force;
				nowshooting.GetComponent<Rigidbody>().AddForce(Force);
				nowshooting = null;
				shooting = false;
				ctrl.instance.startStopTest();	
				force = 0;
				foreach(GameObject Pin in ctrl.instance.Pins)
				{
					pegController Peg = Pin.GetComponent<pegController>();
					Peg.ResetMovement();
				}
			}
		}
	}
	public void nextShot()
	{
		//lines up the next shot. 
		shooting = true;
		if(freeRed > 0){
			freeRed--;
			nowshooting = Instantiate(redBall);
			nowshooting.transform.position = ballMarker.transform.position;
		}
		else{
			if(freeWhite > 0) {
				freeWhite--;
				nowshooting = Instantiate(whiteBall);
				nowshooting.transform.position = ballMarker.transform.position;
			}
			else{
				nowshooting = closestBall();
				if(nowshooting == null)
				{
					throw new NoBallException();
				}
				myballs.Remove(nowshooting);
				nowshooting.transform.position = ballMarker.transform.position;
				nowshooting.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			}
		}
	}
	private GameObject closestBall()
	{
		//Gets the ball closest to the players.
		GameObject candidate = null;
		float highZ = float.MinValue;
		foreach(GameObject ball in myballs)
		{
			if(ball.transform.position.z > highZ)
			{
				highZ = ball.transform.position.z; 
				candidate = ball;
			}
		}
		return candidate;
	}

	public void potBall(GameObject Ball)
	{
		//Removes ball from circulation and puts it back into the ball tray.
		myballs.Remove(Ball);
		ballController BC = Ball.GetComponent<ballController>();
		if (barUp)
		{
			if (BC.red)
			{
				freeRed++;
			}
			else
			{
				freeWhite++;
			}
		}
		Destroy(Ball);
	}
	public void foulBall(GameObject Ball)
	{
		//Removes ball from circulation and puts it back into the ball tray, even if the bar is down.
		myballs.Remove(Ball);
		ballController BC = Ball.GetComponent<ballController>(); 
			if (BC.red)
			{
				freeRed++;
			}
			else
			{
				freeWhite++;
			}		  
		Destroy(Ball);
	}
	public List<GameObject> ballList()
	{
		//returns a copy of the ball list.
		return new List<GameObject>(myballs);
	}
	public class NoBallException : System.Exception { };

	public GameObject getCurrentBall()
	{
		return nowshooting;
	}
}
				  