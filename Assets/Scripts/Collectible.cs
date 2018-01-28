using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Obstacle {

	public bool shineOn = false;
	protected GameObject childSprite;
	protected Vector3 targetPos;
	protected float currentDriftSpeed;
	protected Vector3 currentDriftRotate;
	protected float driftRotateTimer;

	public float maxDriftRadius = 1f;
	public float averageDriftSpeed = 0.5f;
	public float driftSpeedVariance = 0.1f;

	public float averageDriftRotate = 20f;
	public float averageRotateTimer = 2f;
	public float rotateTimerVariance = 1f;

	public ParticleSystem rippleEffect;
	public ParticleSystem bloomEffect;
	protected bool wasLit = false;

	public override void Start ()
	{
		base.Start ();
		childSprite = GetComponentInChildren<SpriteRenderer> ().gameObject;
		SetDriftTarget ();
		bloomEffect.Stop ();
	}

	void Update () 
	{
		shineOn = false;

		childSprite.transform.localPosition = Vector3.MoveTowards (childSprite.transform.localPosition, targetPos, currentDriftSpeed * Time.deltaTime);
		childSprite.transform.localEulerAngles += Time.deltaTime * currentDriftRotate;
		/*childSprite.transform.localEulerAngles = new Vector3(Mathf.Clamp(childSprite.transform.localEulerAngles.x, -45f, 45f), 
			Mathf.Clamp(childSprite.transform.localEulerAngles.y, -45f, 45f), 
			childSprite.transform.localEulerAngles.z);*/
		driftRotateTimer -= Time.deltaTime;
		if (childSprite.transform.localPosition == targetPos) {
			SetDriftTarget ();
		}
		if (driftRotateTimer <= 0f) {
			SetDriftRotate ();
		}
	}

	void LateUpdate ()
	{
		if (!wasLit && shineOn) {
			//rippleEffect.Play ();
			bloomEffect.Play ();
		} else if (wasLit && !shineOn) {
			bloomEffect.Stop ();
		}

		wasLit = shineOn;
	}

	public override void HitByLight(Vector3 lightOrigin, Vector3 lightEnd)
	{
		shineOn = true;
	}

	void SetDriftTarget ()
	{
		targetPos = Random.insideUnitCircle * maxDriftRadius;
		currentDriftSpeed = averageDriftSpeed + Random.Range (-driftSpeedVariance, driftSpeedVariance);
	}

	void SetDriftRotate ()
	{
		//currentDriftRotate = new Vector3 (Random.Range (-averageDriftRotate, averageDriftRotate), Random.Range (-averageDriftRotate, averageDriftRotate), Random.Range (-averageDriftRotate, averageDriftRotate));
		currentDriftRotate = new Vector3 (0f, 0f, Random.Range (-averageDriftRotate, averageDriftRotate));
		driftRotateTimer = averageRotateTimer + Random.Range (-rotateTimerVariance, rotateTimerVariance);
	}
}
