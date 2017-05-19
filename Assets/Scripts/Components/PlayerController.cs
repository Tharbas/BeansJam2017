using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private GameObject player;
	public float movementspeed = 100.0f;
	public string verticalAxisInput;
	public string horizontalAxisInput;



	// Use this for initialization
	void Start () {
		player = this.gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {

		float movementHorizontal = Input.GetAxis (horizontalAxisInput);
		float movementVertical = Input.GetAxis (verticalAxisInput); 

		Vector3 movement = new Vector3 (movementHorizontal, movementVertical, 0);

		//rb3d.AddForce (movement * movementspeed, ForceMode.Force);

		player.GetComponent<Transform> ().Translate ((movement * movementspeed * Time.deltaTime));

		
	}
		
}


