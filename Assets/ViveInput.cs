using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveInput : MonoBehaviour
{
	SteamVR_TrackedObject trackedObj;
	public bool isTriggerDown = false;

	void Start ()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	void FixedUpdate ()
	{
		var device = SteamVR_Controller.Input((int)trackedObj.index);
		isTriggerDown = device.GetTouch(SteamVR_Controller.ButtonMask.Trigger);
	}
	
	void Update ()
	{
	
	}
}
