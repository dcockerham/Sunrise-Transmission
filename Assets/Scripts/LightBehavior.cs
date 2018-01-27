using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehavior : MonoBehaviour {

	public GameObject lightSource;
	public Vector3 angleOffset;
	public LineRenderer lightBeam;

	public bool useReflection = false;
	public Vector3 reflectionAngle;

	public float beamLength;
	public float beamWidth; //get rid of this later, just get it from the lineRenderer

	void Start () 
	{
		lightBeam = GetComponent<LineRenderer> ();
		lightBeam.widthMultiplier = beamWidth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// figure out current location and angle
		Vector3 beamDir = !useReflection ? (Quaternion.Euler(angleOffset) * lightSource.transform.up).normalized : reflectionAngle;
		Vector3 firstPoint = lightSource.transform.position; //might get rid of this Vector3, just get it from lineRenderer
		Vector3 lastPoint = lightSource.transform.position + beamDir * beamLength;

		// check collisions with all obstacles
		List<Obstacle> collisionList = new List<Obstacle>();
		List<float> distList = new List<float> ();
		for (int i = 0; i < Obstacle.obstacleList.Count; i++) 
		{
			Vector3 obsVec = Obstacle.obstacleList [i].transform.position - firstPoint;
			float dotProduct = Vector3.Dot (obsVec, beamDir);
			if (dotProduct > 0) 
			{
				Vector3 lineProj = firstPoint + dotProduct * beamDir;
				float distFromBeam = (lineProj - Obstacle.obstacleList [i].transform.position).magnitude;
				if (distFromBeam <= beamWidth && obsVec.magnitude <= beamLength) //later, add in obstacle radius as well
				{
					// it's a hit! add it to the list
					collisionList.Add (Obstacle.obstacleList [i]);
					distList.Add (obsVec.magnitude);
				}
			}
		}

		Obstacle closestCollision = null;
		float closestDist = beamLength;
		for (int i = 0; i < collisionList.Count; i++) 
		{
			if (distList [i] < closestDist) 
			{
				closestCollision = collisionList [i];
				closestDist = distList [i];
			}
		}

		if (closestCollision) 
		{
			// perform hit calcs
			lastPoint = lightSource.transform.position + beamDir * closestDist;
			closestCollision.HitByLight (firstPoint);
		}

		lightBeam.SetPosition (0, firstPoint);
		lightBeam.SetPosition (1, lastPoint);
	}
}
