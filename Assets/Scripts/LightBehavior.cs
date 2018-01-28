using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehavior : MonoBehaviour {

	public MirrorController lightSource;
	public Vector3 angleOffset;
	public LineRenderer lightBeam;

	public bool useReflection = false;
	public Vector3 reflectionAngle;

	public float beamLength;
	public float beamWidth; //get rid of this later, just get it from the lineRenderer

	protected ParticleSystem partSys;

	void Start () 
	{
		lightSource = transform.parent.gameObject.GetComponent<MirrorController> ();
		partSys = GetComponentInChildren<ParticleSystem> ();
		lightBeam = GetComponent<LineRenderer> ();
		lightBeam.widthMultiplier = beamWidth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// figure out current location and angle
		Vector3 beamDir = !useReflection ? (Quaternion.Euler(angleOffset) * lightSource.transform.up).normalized : reflectionAngle;
		Vector3 firstPoint = lightSource.transform.position; //might get rid of this Vector3, just get it from lineRenderer
		float cutoffDist = beamLength;

		// check collisions with all obstacles
		List<Obstacle> collisionList = new List<Obstacle>();
		List<float> distList = new List<float> ();
		for (int i = 0; i < Obstacle.obstacleList.Count; i++) 
		{
			if (Obstacle.obstacleList [i] == this)
				continue;
			Vector3 obsVec = Obstacle.obstacleList [i].transform.position - firstPoint;
			float dotProduct = Vector3.Dot (obsVec, beamDir);
			if (dotProduct > 0) 
			{
				Vector3 lineProj = firstPoint + dotProduct * beamDir;
				float distFromBeam = (lineProj - Obstacle.obstacleList [i].transform.position).magnitude;
				if (distFromBeam <= beamWidth && obsVec.magnitude <= cutoffDist) //later, add in obstacle radius as well
				{
					// it's a hit! add it to the list
					collisionList.Add (Obstacle.obstacleList [i]);
					distList.Add (obsVec.magnitude);

					if (Obstacle.obstacleList [i].blockLight) 
					{
						cutoffDist = obsVec.magnitude;
					}
				}
			}
		}

		for (int i = 0; i < collisionList.Count; i++) 
		{
			if (distList [i] <= cutoffDist) 
			{
				collisionList[i].HitByLight (firstPoint, lightSource.transform.position + beamDir * cutoffDist);
			}
		}

		Vector3[] beamPositions = new Vector3[30];
		for (int i = 0; i < beamPositions.Length; i++) {
			beamPositions[i] = lightSource.transform.position + beamDir * cutoffDist * ((float)i/beamPositions.Length);
		}
		lightBeam.positionCount = beamPositions.Length;
		lightBeam.SetPositions (beamPositions);

		if (partSys) {
			partSys.transform.position = lightSource.transform.position + beamDir * cutoffDist / 2f;
			ParticleSystem.ShapeModule shapeMod = partSys.shape;
			shapeMod.scale = new Vector3 (cutoffDist / 2f, 1f, beamWidth / 2f);
		}
	}
}
