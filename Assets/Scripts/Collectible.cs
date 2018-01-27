using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Obstacle {

	public bool shineOn = false;

	void Update () 
	{
		shineOn = false;
	}

	public override void HitByLight (Vector3 lightOrigin)
	{
		shineOn = true;
	}
}
