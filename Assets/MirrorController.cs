using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : MonoBehaviour {
	public float rotateSpeed = 30.0f;
	public float speed = 15.0f;
	private bool selected;
	private bool shineOn;
	private bool mouseOn;

	public float range;  //for both x & y;
	private float x;
	private float y;

	// Use this for initialization
	void Start () {
		selected = false;
		shineOn = false;
		range = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		x = transform.position.x;
		y = transform.position.y;
		Debug.Log ("x: " + x);
		Debug.Log ("y: " + y);

		if (selected) {
			if (-range < x) {
				//translate; up down left right
				if (Input.GetKey (KeyCode.LeftArrow)) 
					transform.position += new Vector3 (-0.2f * speed * Time.deltaTime, 0.0f, 0.0f);
			}
			if (x < range) {
				if (Input.GetKey (KeyCode.RightArrow)) 
					transform.position += new Vector3 (0.2f * speed * Time.deltaTime, 0.0f, 0.0f);
			}
			if (-range < y) {
				if (Input.GetKey (KeyCode.DownArrow))
					transform.position += new Vector3 (0.0f, -0.2f * speed * Time.deltaTime, 0.0f);
			}
			if (y < range) {
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
			transform.localScale *= 2;
		}

		if (mouseOn == false && Input.GetMouseButton(0) && selected == false) {
			selected = false;
			transform.localScale *= 0.5;
		}
	}

	void OnMouseEnter(){
		mouseOn = true;
		if(!selected)
			transform.localScale = new Vector3 (1.2f, 7.2f, 1.2f);
		Debug.Log ("Mouse now Enter");
	}

	void OnMouseExit(){
		if(!selected)
			transform.localScale = new Vector3 (1.0f, 6.0f, 1.0f);
		mouseOn = false;
		Debug.Log ("Mouse now Exit");
	}


}