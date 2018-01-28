using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : Obstacle {

	public bool startAlive = false;
	protected Transform transformTarget;

	public float rotateSpeed = 30.0f;
	public float speed = 15.0f;
	protected bool selected;
	protected bool mouseOn;

	public Vector2 range = new Vector2(3f, 3f);
	protected Vector3 startPosition;
	protected Vector3 startScale;

	protected SpriteRenderer spriteImage;

	protected bool lightsActive = false;
	public bool alwaysOn = false;
	public bool realReflection = false;
	public bool reverseReflection = false;
	//protected List<LightBehavior> lightBeams;
	protected LightBehavior[] lightBeams;

	public float correctAngle = 180f;
	public float angleVariance = 180f;

	public float displayAngle; //just for testing

	public enum ObjectType
	{
		MIRROR_TYPE,
		TORCH_TYPE,
		PRISM_TYPE,
	};
	public ObjectType myType = ObjectType.MIRROR_TYPE;

	public MirrorController switchForm;

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();
		selected = false;

		if (switchForm) {
			transformTarget = transform.parent;
		} else {
			transformTarget = transform;
		}

		startPosition = transformTarget.position;
		startScale = transformTarget.localScale;
		spriteImage = GetComponentInChildren<SpriteRenderer> ();
		lightBeams = GetComponentsInChildren<LightBehavior> ();

		if (!startAlive)
			gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		lightsActive = alwaysOn;
		float x = transformTarget.position.x;
		float y = transformTarget.position.y;

		if (selected) {
			if (-range.x < (x - startPosition.x)) {
				//translate; up down left right
				if (Input.GetKey (KeyCode.LeftArrow)) 
					transformTarget.position += new Vector3 (-0.2f * speed * Time.deltaTime, 0.0f, 0.0f);
			}
			if ((x - startPosition.x) < range.x) {
				if (Input.GetKey (KeyCode.RightArrow)) 
					transformTarget.position += new Vector3 (0.2f * speed * Time.deltaTime, 0.0f, 0.0f);
			}
			if (-range.y < (y - startPosition.y)) {
				if (Input.GetKey (KeyCode.DownArrow))
					transformTarget.position += new Vector3 (0.0f, -0.2f * speed * Time.deltaTime, 0.0f);
			}
			if ((y - startPosition.y) < range.y) {
				if (Input.GetKey (KeyCode.UpArrow)) 
					transformTarget.position += new Vector3 (0.0f, 0.2f * speed * Time.deltaTime, 0.0f);
			}

			//rotation; 9 / 0 on top bar
			if (Input.GetKey (KeyCode.Alpha9)) {
				transformTarget.Rotate (new Vector3 (0.0f, 0.0f, rotateSpeed * Time.deltaTime));
			}
			if (Input.GetKey (KeyCode.Alpha0)) {
				transformTarget.Rotate (new Vector3 (0.0f, 0.0f, -rotateSpeed * Time.deltaTime));
			}

			// change objects if you hit spacebar
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (switchForm) {
					//transformObject.SetActive (true);
					switchForm.gameObject.SetActive(true);
					switchForm.selected = true;
					gameObject.SetActive (false);
				}
			}
		}


		//activate & deactive selected boolean;
		if (mouseOn == true && Input.GetMouseButton(0) && selected == false) {
			selected = true;
			transformTarget.localScale = 1.3f * startScale;
		}

		if (mouseOn == false && Input.GetMouseButton(0) && selected == true) {
			selected = false;
			transformTarget.localScale = startScale;
		}
	}

	void LateUpdate ()
	{
		if (lightsActive) 
		{
			spriteImage.color = new Color (255f, 255f, 255f, 255f);
		}

		for (int i = 0; i < lightBeams.Length; i++) 
		{
			lightBeams [i].gameObject.SetActive (lightsActive);
		}
	}

	void OnMouseEnter(){
		mouseOn = true;
		if(!selected)
			transformTarget.localScale = 1.1f * startScale;
		//Debug.Log ("Mouse now Enter");
	}

	void OnMouseExit(){
		if(!selected)
			transformTarget.localScale = startScale;
		mouseOn = false;
		//Debug.Log ("Mouse now Exit");
	}

	public override void HitByLight(Vector3 lightOrigin, Vector3 lightEnd)
	{
		Vector3 lightVec = transform.position - lightOrigin;
		float angleBetween = Vector3.SignedAngle (lightVec.normalized, transformTarget.up, Vector3.up);
		if (angleBetween <= (correctAngle + angleVariance/2) && angleBetween >= (correctAngle - angleVariance/2)) 
		{
			lightsActive = true;
		}
		displayAngle = angleBetween;

		if (realReflection) {
			Vector3 reflectionAngle = Vector3.Reflect (lightVec.normalized, transformTarget.up);
			for (int i = 0; i < lightBeams.Length; i++) {
				lightBeams [i].useReflection = true;
				lightBeams [i].reflectionAngle = reverseReflection ? -reflectionAngle : reflectionAngle;
			}
		} else {
			for (int i = 0; i < lightBeams.Length; i++) {
				lightBeams [i].useReflection = false;
			}
		}
	}

}
