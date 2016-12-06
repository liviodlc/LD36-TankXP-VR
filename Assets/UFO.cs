using UnityEngine;
using System.Collections;

public class UFO : MonoBehaviour
{
	public Transform points;
	public int i = 0;
	private int n;
	private Transform currentPoint;

	public float moveSpeed;
	public bool wasHit;

	void Start ()
	{
		n = points.childCount;
		currentPoint = points.GetChild(i);
	}
	
	void FixedUpdate ()
	{
		if (wasHit)
			return;
		Vector3 fromTo = currentPoint.position - transform.position;
		if(fromTo.magnitude <= moveSpeed + 0.1f)
		{
			i++;
			if (i >= n)
				i = 0;
			currentPoint = points.GetChild(i);
			fromTo = currentPoint.position - transform.position;
		}
		transform.position += (fromTo).normalized * moveSpeed;
	}

	void OnCollisionEnter(Collision collision)
	{
		wasHit = true;
		GetComponent<Rigidbody>().useGravity = true;
		gameObject.layer = 0;
	}

	void Update ()
	{
	
	}
}
