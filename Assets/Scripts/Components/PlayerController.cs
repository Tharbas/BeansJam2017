using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D rb2d;
	public string XAxisInputString = "Horizontal";
	public string YAxisInputString = "Vertical";
	public float movementspeed;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		movementspeed = 100.0f;
		
	}


	virtual void FixedUpdate(){
		
		float moveHorizontal = Input.GetAxis(XAxisInputString);
		float moveVertical = Input.GetAxis (YAxisInputString);

		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

		rb2d.AddForce (movement*movementspeed);

	}

