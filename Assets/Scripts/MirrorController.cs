using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : Obstacle {

	public float rotateSpeed = 30.0f;
	public float speed = 15.0f;
	protected bool selected;
	protected bool mouseOn;

	public Vector2 range = new Vector2(3f, 3f);
	protected Vector3 startPosition;

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

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();
		selected = false;
		startPosition = transform.position;
		spriteImage = GetComponent<SpriteRenderer> ();
		lightBeams = GetComponentsInChildren<LightBehavior> ();
	}
	
	// Update is called once per frame
	void Update () {
		lightsActive = alwaysOn;
		float x = transform.position.x;
		float y = transform.position.y;

		if (selected) {
			if (-range.x < (x - startPosition.x)) {
				//translate; up down left right
				if (Input.GetKey (KeyCode.LeftArrow)) 
					transform.position += new Vector3 (-0.2f * speed * Time.deltaTime, 0.0f, 0.0f);
			}
			if ((x - startPosition.x) < range.x) {
				if (Input.GetKey (KeyCode.RightArrow)) 
					transform.position += new Vector3 (0.2f * speed * Time.deltaTime, 0.0f, 0.0f);
			}
			if (-range.y < (y - startPosition.y)) {
				if (Input.GetKey (KeyCode.DownArrow))
					transform.position += new Vector3 (0.0f, -0.2f * speed * Time.deltaTime, 0.0f);
			}
			if ((y - startPosition.y) < range.y) {
				if (Input.GetKey (KeyCode.UpArrow)) 
					transform.position += new Vector3 (0.0f, 0.2f * speed * Time.deltaTime, 0.0f);
			}

			//rotation; 9 / 0 on top bar
			if (Input.GetKey (KeyCode.Alpha9)) {
				transform.Rotate (new Vector3 (0.0f, 0.0f, rotateSpeed * Time.deltaTime));
			}
			if (Input.GetKey (KeyCode.Alpha0)) {
				transform.Rotate (new Vector3 (0.0f, 0.0f, -rotateSpeed * Time.deltaTime));
			}
		}


		//activate & deactive selected boolean;
		if (mouseOn == true && Input.GetMouseButton(0) && selected == false) {
			selected = true;
			transform.localScale *= 2.0f;
		}

		if (mouseOn == false && Input.GetMouseButton(0) && selected == true) {
			selected = false;
			transform.localScale *= 0.5f;
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
			transform.localScale *= 1.2f;
		//Debug.Log ("Mouse now Enter");
	}

	void OnMouseExit(){
		if(!selected)
			transform.localScale *= 1f/1.2f;
		mouseOn = false;
		//Debug.Log ("Mouse now Exit");
	}

	public override void HitByLight(Vector3 lightOrigin)
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
			for (int i = 0; i < lightBeams.Length; i++) 
			{
				lightBeams [i].useReflection = true;
				lightBeams [i].reflectionAngle = reverseReflection ? -reflectionAngle : reflectionAngle;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "lightBeam") {
			Debug.Log ("touches lightbeam");
		}
	}
}
