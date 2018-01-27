using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public static List<Obstacle> obstacleList = new List<Obstacle>();

	public virtual void Start () 
	{
		obstacleList.Add (this);
	}

	public virtual void OnDestroy()
	{
		obstacleList.Remove (this);
	}

	public virtual void HitByLight (Vector3 lightOrigin)
	{
		
	}
}
