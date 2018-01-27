using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : Obstacle {
	public float rotateSpeed = 30.0f;
	public float speed = 15.0f;
	private bool selected;
	private bool shineOn;
	private bool mouseOn;

	public float xRange;  //for both x & y;
	public float yRange;

	private float x;
	private float y;

	private float startX;
	private float startY;

	// Use this for initialization
	void Start () {
		selected = false;
		shineOn = false;

		startX = transform.position.x;
		startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		x = transform.position.x;
		y = transform.position.y;

		if (shineOn) {
			this.GetComponent<SpriteRenderer> ().color = new Color (255f, 255f, 255f, 255f);
		}

		if (selected) {
			if (startX-xRange < x) {
				//translate; up down left right
				if (Input.GetKey (KeyCode.LeftArrow)) 
					transform.position += new Vector3 (-0.2f * speed * Time.deltaTime, 0.0f, 0.0f);
			}
			if (x < startX+xRange) {
				if (Input.GetKey (KeyCode.RightArrow)) 
					transform.position += new Vector3 (0.2f * speed * Time.deltaTime, 0.0f, 0.0f);
			}
			if (startY-yRange < y) {
				if (Input.GetKey (KeyCode.DownArrow))
					transform.position += new Vector3 (0.0f, -0.2f * speed * Time.deltaTime, 0.0f);
			}
			if (y < startY+yRange) {
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

	void OnMouseEnter(){
		mouseOn = true;
		if(!selected)
			transform.localScale *= 1.2f;
		Debug.Log ("Mouse now Enter");
	}

	void OnMouseExit(){
		if(!selected)
			transform.localScale *= 1f/1.2f;
		mouseOn = false;
		Debug.Log ("Mouse now Exit");
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "lightBeam") {
			Debug.Log ("touches lightbeam");
		}
	}


}