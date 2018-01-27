using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBehavior : MonoBehaviour {

	public bool lightsActive = false;
	public bool alwaysOn = false;
	public bool realReflection = false;
	public bool reverseReflection = false;
	public List<LightBehavior> lightBeams;

	public float correctAngle = 180f;
	public float angleVariance = 180f;

	public float displayAngle; //just for testing

	void Start () 
	{
		
	}

	void Update () 
	{
		lightsActive = alwaysOn;
	}

	void LateUpdate ()
	{
		for (int i = 0; i < lightBeams.Count; i++) 
		{
			lightBeams [i].gameObject.SetActive (lightsActive);
		}
	}

	public void HitByLight(Vector3 lightOrigin)
	{
		Vector3 lightVec = transform.position - lightOrigin;
		float angleBetween = Vector3.SignedAngle (lightVec.normalized, transform.up, Vector3.up);
		if (angleBetween <= (correctAngle + angleVariance/2) && angleBetween >= (correctAngle - angleVariance/2)) 
		{
			lightsActive = true;
		}
		displayAngle = angleBetween;

		if (realReflection) 
		{
			Vector3 reflectionAngle = Vector3.Reflect (lightVec.normalized, transform.up);
			for (int i = 0; i < lightBeams.Count; i++) 
			{
				lightBeams [i].useReflection = true;
				lightBeams [i].reflectionAngle = reverseReflection ? -reflectionAngle : reflectionAngle;
			}
		}
	}
}
