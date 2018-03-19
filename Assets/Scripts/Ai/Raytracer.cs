using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Raytracer
{
	private Vector3 from;
	private Vector3 to;

	private Ray ray;
	private RaycastHit hit;

	private Vector3 inDirection;

	private int reflectionCount = 5;

	public void setParam(Vector3 From, Vector3 To) {
		this.from = From;
		this.to = To;
	}
	public objectAngle castIgnore(GameObject Ignore)
	{
		Ignore.SetActive(false);									  
		this.ray = new Ray(from, to);

		for (int i = 0; i <= this.reflectionCount; i++)
		{
			if (Physics.Raycast(this.ray.origin, this.ray.direction, out this.hit, 50))
			{
				Debug.DrawRay(this.ray.origin, (hit.point - this.ray.origin), Color.green, 5);	
				if (this.hit.transform.gameObject.CompareTag("Table"))
				{
					this.inDirection = Vector3.Reflect(ray.direction, hit.normal);
					ray = new Ray(hit.point, this.inDirection);
				}
				else
				{
					Ignore.SetActive(true);
					objectAngle o = new objectAngle();
					o.DirectionTo = -this.ray.direction;
					o.Object = hit.transform.gameObject;
					return o;
				}
			}
		}
		Ignore.SetActive(true);

		objectAngle oa = new objectAngle();
		oa.DirectionTo = Vector3.zero;
		oa.Object = null;
		return oa;
	}

	public struct objectAngle
	{
		public Vector3 DirectionTo;
		public GameObject Object;
	}
}
