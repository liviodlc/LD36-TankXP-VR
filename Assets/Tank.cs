using UnityEngine;
using System.Collections;

public class Tank : MonoBehaviour {

	public float motorForce = 2000f;
	public float currentSpeedLeft = 0f;
	public float currentSpeedRight = 0f;
	public float maxSpeed = 10f;

	public TankWheel[] wheels;
	public Rigidbody rb;
	public bool isOnRoad = false;
	public float flatteningForce = 0.1f;
	public float maxFlatteningForce = 50;

	public Cannon cannon;
	public Lever cannon_aim_horiz;
	public Lever cannon_aim_vert;

	public Lever left;
	public Lever right;

	void Start ()
	{
		//rb.centerOfMass += new Vector3(0, -1, 0);
		rb.maxAngularVelocity = 1;
	}

	void FixedUpdate()
	{
		currentSpeedLeft = left.leverValue * maxSpeed;
		currentSpeedRight = right.leverValue * maxSpeed;

		cannon.rotateCannon(cannon_aim_horiz.leverValue, cannon_aim_vert.leverValue);

		isOnRoad = false;
		Vector3 roadNormal = Vector3.zero;
		foreach (TankWheel a in wheels)
		{
			isOnRoad = isOnRoad || a.onRoad;
			roadNormal += a.roadNormal;
			a.speed = motorForce;
		}
		roadNormal /= 2;

		if (isOnRoad)
		{
			//stabilizing force
			float angle = Vector3.Angle(transform.up, roadNormal);
			Vector3 axis = Vector3.Cross(transform.up, roadNormal).normalized;
			float targetSpeed = angle * flatteningForce;
			float acc = (targetSpeed - Vector3.Dot(rb.angularVelocity, axis)) / Time.deltaTime;
			if (acc < 0)
				acc = Mathf.Max(acc, -maxFlatteningForce);
			else
				acc = Mathf.Min(acc, maxFlatteningForce);
			rb.AddTorque(axis * acc, ForceMode.Force);
		}
	}
	
	void Update ()
	{

	}
}
