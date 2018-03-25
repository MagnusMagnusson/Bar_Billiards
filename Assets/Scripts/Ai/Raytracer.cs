using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Raytracer
{
	private Vector3 from;
	private Vector3 to;

	private int reflectionCount = 2;

	public void SetParam(Vector3 From, Vector3 To) {
		this.from = From;
		this.to = To;
	}
	public ObjectAngle CastIgnore(GameObject Ignore)
	{
		Ignore.SetActive(false);									  
		Ray ray = new Ray(from, to);
		RaycastHit hit;
		Vector3 inDirection;
		inDirection = to;

		GameObject currentBall = ctrl.instance.balltray.getCurrentBall();
		SphereCollider collider = currentBall.GetComponent<SphereCollider>();
		float diameter = 2 * collider.radius * currentBall.transform.localScale.z;

		for (int i = 0; i <= this.reflectionCount; i++)
		{
			if (Physics.SphereCast(ray, diameter, out hit, 50))
			{
				Debug.DrawRay(ray.origin, (hit.point - ray.origin), Color.green, 5);	
				if (hit.transform.gameObject.CompareTag("Table"))
				{
					inDirection = Vector3.Reflect(ray.direction, hit.normal);
					ray = new Ray(hit.point, inDirection);
				}
				else
				{
					Ignore.SetActive(true);
					ObjectAngle o = new ObjectAngle();	
					o.DirectionTo = -inDirection;
					o.Object = hit.transform.gameObject; 
					return o;
				}
			}
		}
		Ignore.SetActive(true);

		ObjectAngle oa = new ObjectAngle();
		oa.DirectionTo = Vector3.zero;
		oa.Object = null;
		return oa;
	}

	public struct ObjectAngle
	{
		public Vector3 DirectionTo;
		public GameObject Object;
	}

	public ObjectAngle getTriangle(Vector3 origin, GameObject target, GameObject goal)
	{
		Vector3 to = -(origin - target.transform.position).normalized;

		SphereCollider collider = target.GetComponent<SphereCollider>();
		float diameter = 2 * collider.radius * target.transform.localScale.z;

		Ray ray = new Ray(origin, to);
		RaycastHit hit;
		UnityEngine.Random dice = new UnityEngine.Random();
		Color col = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

		if (Physics.SphereCast(ray,diameter / 2, out hit))
		{
			Debug.DrawRay(ray.origin, (hit.point - ray.origin), col, 5);
			if (hit.transform.gameObject == target)  //We have a clear line of site to the target ball. 
			{							  
				Vector3 rayPath = (hit.point - ray.origin);
				float magnitude = rayPath.magnitude + diameter;
				Vector3 newPos = ray.origin + (rayPath.normalized * magnitude);
				Vector3 direction = -(newPos - goal.transform.position).normalized;
				ray = new Ray(newPos, direction); 							
				if (Physics.SphereCast(ray, diameter / 2, out hit))
				{
					Debug.DrawRay(ray.origin, (hit.point - ray.origin), col, 5);
					if (hit.transform.gameObject == goal)  //We can make a reasonable hit!
					{
						ObjectAngle o = new ObjectAngle();
						o.DirectionTo = -to;
						o.Object = goal;
						return o;
					}
				}						 
			}
		}

		ObjectAngle oa = new ObjectAngle();
		oa.DirectionTo = Vector3.zero;
		oa.Object = null;
		return oa;
	}
}
