using UnityEngine;
using System.Collections;

public class Pulley : MonoBehaviour
{
	//public float neutralY;
	private Vector3 neutralPosition;
	public float maxOffsetY;

	public GameObject pulleyObject;
	public GameObject pulleyHandle;

	public ViveInput[] controllers;
	private ViveInput grabbing = null;
	public float reachDistance;
	public Color reachColor = Color.red;
	public Color grabColor = Color.yellow;
	private bool isHandleOnHover = false;
	public float snapTreshold = 0.1f;

	public float pulleyValue = 0f;
	public bool fire = false;
	public bool hasFired = false;
	public float timeBetweenShots;
	public float timeFired;
	private Color normalColor;

	void Start ()
	{
		neutralPosition = pulleyObject.transform.localPosition;
		Renderer rend = pulleyHandle.GetComponent<Renderer>();
		normalColor = rend.material.color;
	}
	
	void FixedUpdate ()
	{
		if (timeFired != 0)
		{
			if (Time.fixedTime - timeFired > timeBetweenShots)
			{
				timeFired = 0;
				hasFired = false;
			}
		}
		isHandleOnHover = false;

		foreach (ViveInput t in controllers)
			onReachEvent(t);

		if (grabbing != null)
		{
			Vector3 dist = transform.InverseTransformPoint(grabbing.transform.position) - neutralPosition;
			float distY = Vector3.Dot(dist, transform.up);
			if (distY <= -maxOffsetY)
				distY = -maxOffsetY;
			if (distY > 0)
				distY = 0;
			pulleyValue = Mathf.Abs(distY / maxOffsetY);
			if (pulleyValue < snapTreshold)
			{
				pulleyValue = 0;
				distY = 0;
			}
			else if (pulleyValue > 1 - snapTreshold)
			{
				pulleyValue = 1;
				distY = -maxOffsetY;
			}
			if (pulleyValue == 1)
			{
				fire = true;
			}else
			{
				fire = false;
			}
			pulleyObject.transform.localPosition = neutralPosition + new Vector3(0, distY, 0);

			//float distX = Vector3.Dot(dist, transform.forward);
			//float deg = Mathf.Atan2(distX, distY) * Mathf.Rad2Deg;
			//deg = Mathf.Min(deg, maxOffset);
			//deg = Mathf.Max(deg, -maxOffset);
			//leverValue = deg / maxOffset;
			//if (leverValue >= -snapTreshold && leverValue <= snapTreshold)
			//{
			//	leverValue = 0;
			//	deg = 0;
			//}
			//else if (leverValue > 1 - snapTreshold)
			//{
			//	leverValue = 1;
			//	deg = maxOffset;
			//}
			//else if (leverValue < snapTreshold - 1)
			//{
			//	leverValue = -1;
			//	deg = -maxOffset;
			//}
			//lever.localEulerAngles = new Vector3(neutralAngle + deg * (flipHandle ? -1 : 1), flipHandle ? 180 : 0, 0);
		}
		else
		{
			pulleyObject.transform.localPosition = neutralPosition;
			fire = false;
		}
	}
	
	void Update ()
	{
		if (isHandleOnHover)
		{
			Renderer rend = pulleyHandle.GetComponent<Renderer>();

			if (grabbing != null)
			{
				rend.material.color = grabColor;
			}
			else
			{
				rend.material.color = reachColor;
			}
		}
		else
		{
			Renderer rend = pulleyHandle.GetComponent<Renderer>();
			if (grabbing != null)
			{
				rend.material.color = grabColor;
			}
			else
			{
				rend.material.color = normalColor;
			}
		}
	}

	private void onReachEvent(ViveInput hand)
	{
		if (hand == null)
			return;
		if (Vector3.Distance(hand.transform.position, transform.position) <= reachDistance)
		{
			isHandleOnHover = true;
			if (hand.isTriggerDown && grabbing == null)
			{
				grabbing = hand;
				hand.transform.GetChild(0).gameObject.SetActive(false);
			}
		}
		if (!hand.isTriggerDown && grabbing == hand)
		{
			grabbing = null;
			hasFired = false;
			hand.transform.GetChild(0).gameObject.SetActive(true);
		}
	}
}
