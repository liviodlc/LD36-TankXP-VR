using UnityEngine;
using System.Collections;

public class TankWheel : MonoBehaviour
{

	public float speed = 0f;
	public Rigidbody rb;
	public Tank t;
	public bool isLeft;

	public float scrollSpeed = 0.5F;
	public float raycastDist = 0.9f;
	public float raycastDist2 = 0.2f;
	public bool onRoad = false;
	public Vector3 roadNormal;
	private Renderer rend;

	private float offset;

	public Vector3 vel;
	public float spe;

	void Start ()
	{
		rend = GetComponent<Renderer>();
	}

	void FixedUpdate()
	{
		bool road = false;

		RaycastHit hit;
		//Debug.DrawRay(transform.position, -transform.up * raycastDist, Color.white, 2f);
		if (Physics.Raycast(transform.position, -transform.up, out hit, raycastDist))
		{
			road = true;
			roadNormal = hit.normal;
		}

		RaycastHit hit2;
		//Debug.DrawRay(transform.position + (transform.right * -1.8f), -transform.up * raycastDist2, Color.white, 2f);
		if (Physics.Raycast(transform.position + (transform.right * -1.8f), -transform.up, out hit2, raycastDist2))
		{
			road = true;
			roadNormal = hit2.normal;
		}

		RaycastHit hit3;
		//Debug.DrawRay(transform.position + (transform.right * 1.4f), -transform.up * raycastDist2, Color.white, 2f);
		if (Physics.Raycast(transform.position + (transform.right * 1.4f), -transform.up, out hit3, raycastDist2))
		{
			road = true;
			roadNormal = hit3.normal;
		}

		onRoad = road;


		if (onRoad)
		{
			//Vector3 lastPosition = transform.position;
			//Vector3 newPosition = transform.position;
			//newPosition += Vector3.ProjectOnPlane(transform.right, Vector3.up) * -speed * Time.deltaTime;
			//Vector3 lastVelocity = rb.GetPointVelocity(transform.position);
			//Vector3 velocity = (newPosition - lastPosition) / Time.deltaTime;
			//Vector3 acceleration = (velocity - lastVelocity) / Time.deltaTime;
			//Vector3 force = rb.mass * acceleration;
			//float vForce = Vector3.Dot(transform.up, force); /*fight gravity*/
			//force -= transform.up * vForce;
			//rb.AddForceAtPosition(force, transform.position);

			//Vector3 forw = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
			vel = rb.velocity;
			spe = Vector3.Dot(rb.velocity, -transform.right);
			float currentSpeed = (isLeft ? t.currentSpeedLeft : t.currentSpeedRight);
			if (currentSpeed > 0)
			{
				Vector3 force = transform.right * -speed * Time.deltaTime;
				if (Vector3.Dot(rb.velocity, -transform.right) < currentSpeed)
					rb.AddForceAtPosition(force, transform.position);
			}
			else if(currentSpeed < 0)
			{
				Vector3 force = transform.right * speed * Time.deltaTime;
				if (Vector3.Dot(rb.velocity, -transform.right) > currentSpeed)
					rb.AddForceAtPosition(force, transform.position);
			}
		}
	}
	
	void Update ()
	{
		offset += Time.deltaTime * scrollSpeed * rb.GetPointVelocity(transform.position).magnitude;
		rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
	}
}
