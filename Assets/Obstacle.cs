using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
	public static List<Obstacle> obstacleList;
	// Use this for initialization

	public virtual void HitByLight(Vector3 lightOrigin){
		
	}
}
