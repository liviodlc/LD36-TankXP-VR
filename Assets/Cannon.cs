using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
	public float neutralAngleHoriz;
	public float neutralAngleVert;
	public float maxAngleOffset;

	public GameObject projectile;
	public Transform tip;
	public Pulley pulley;
	public float blastStrength;

	//void Start ()
	//{

	//}

	public void rotateCannon(float percentHoriz, float percentVert)
	{
		transform.localEulerAngles = new Vector3(neutralAngleVert + -percentVert * maxAngleOffset, neutralAngleHoriz + percentHoriz * maxAngleOffset, 0);
	}

	void FixedUpdate()
	{
		if(pulley.fire && !pulley.hasFired)
		{
			pulley.hasFired = true;
			pulley.timeFired = Time.fixedTime;
			GameObject obj = Instantiate(projectile);
			obj.transform.position = tip.position;
			obj.GetComponent<Rigidbody>().velocity = transform.forward * blastStrength;
		}
	}

	//void Update ()
	//{

	//}
}
