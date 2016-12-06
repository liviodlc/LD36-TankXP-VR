using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
	public Transform lever;
	public Transform handle;
	public float neutralAngle;
	public float maxOffset;
	public bool flipHandle = false;
	public float reachDistance;
	public Color reachColor = Color.red;
	public Color grabColor = Color.yellow;
	private bool isHandleOnHover = false;
	public float snapTreshold = 0.1f;

	public ViveInput[] controllers;
	private ViveInput grabbing = null;

	public float leverValue = 0f;
	private Color normalColor;

	void Start ()
	{
		if (flipHandle)
			lever.transform.localEulerAngles = new Vector3(neutralAngle, 180, 0);
		Renderer rend = lever.GetComponent<Renderer>();
		normalColor = rend.material.color;
	}
	
	void FixedUpdate()
	{
		isHandleOnHover = false;

		foreach (ViveInput t in controllers)
			onReachEvent(t);

		if(grabbing != null)
		{
			Vector3 dist = grabbing.transform.position - lever.position;
			float distX = Vector3.Dot(dist, transform.forward);
			float distY = Vector3.Dot(dist, transform.up);
			float deg = Mathf.Atan2(distX, distY) * Mathf.Rad2Deg;
			deg = Mathf.Min(deg, maxOffset);
			deg = Mathf.Max(deg, -maxOffset);
			leverValue = deg / maxOffset;
			if (leverValue >= -snapTreshold && leverValue <= snapTreshold)
			{
				leverValue = 0;
				deg = 0;
			}
			else if (leverValue > 1 - snapTreshold)
			{
				leverValue = 1;
				deg = maxOffset;
			}
			else if (leverValue < snapTreshold - 1)
			{
				leverValue = -1;
				deg = -maxOffset;
			}
			lever.localEulerAngles = new Vector3(neutralAngle + deg * (flipHandle ? -1 : 1), flipHandle ? 180 : 0, 0);
		}
	}

	void Update()
	{
		if (isHandleOnHover)
		{
			Renderer rend = lever.GetComponent<Renderer>();

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
			Renderer rend = lever.GetComponent<Renderer>();
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
		if (Vector3.Distance(hand.transform.position, handle.position) <= reachDistance)
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
			hand.transform.GetChild(0).gameObject.SetActive(true);
		}
	}
}
