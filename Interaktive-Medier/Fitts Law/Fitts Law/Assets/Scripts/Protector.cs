using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Protector : MonoBehaviour {

	CharacterController hero;
	CharacterController protector;
	float velX = 0;
	float velY = 0;
	const float gravity = -14.0f;
	const float jumpspeed = 8;
	const float rayLength = 0.5f;
	Ray ray;
	SerialPort port;
	private int val2;
	private int val3;
	private int val;
	private int val4;


	// Use this for initialization
	void Start () {
		hero = GameObject.FindGameObjectWithTag ("Hero").GetComponent<CharacterController> ();
		protector = GameObject.FindGameObjectWithTag ("Protector").GetComponent<CharacterController> ();
		port = new SerialPort ("/dev/cu.wchusbserialfa130", 9600);
		port.Open ();
	}

	// Update is called once per frame
	void Update () {

		if (port == null) {
			return;
		}
		if (port.IsOpen) {
			port.ReadTimeout = 1;
			try {
				val2 = port.ReadByte ();
				val3 = port.ReadByte ();
				val = val2 * 256 + val3;
				val4 = port.ReadByte ();
			} catch (TimeoutException) {

			}
		}


		velY += gravity * Time.deltaTime;
		float accelerationX = hero.isGrounded ? 18 : 8;

		//Input.GetKey (KeyCode.LeftArrow)
		if (Input.GetKey (KeyCode.LeftArrow)) {
			velX -= accelerationX * Time.deltaTime;
		} // Input.GetKey(KeyCode.RightArrow)
		else if(Input.GetKey(KeyCode.RightArrow)){
			velX += accelerationX * Time.deltaTime;
		}
		else{
			velX *= hero.isGrounded? 0.07f : 0.98f;
		}

		hero.Move (new Vector3 (velX*Time.deltaTime, velY * Time.deltaTime));

		if(hero.isGrounded){
			// Input.GetKeyDown (KeyCode.Space)
			if(Input.GetKeyDown (KeyCode.UpArrow)){
				velY += jumpspeed;
			}
			else{
				velY = 0;
			}
		}

		ray = new Ray (
			transform.position,
			velX>0 ? Vector3.right : Vector3.left
		);
		RaycastHit hitInfo;
		bool hit = Physics.Raycast (ray, out hitInfo,rayLength,1 << 10);

		if (hit) {
			Collider collider = hitInfo.collider;
			//print (collider);
			Rigidbody rb = collider.GetComponent<Rigidbody>();
			rb.AddForceAtPosition (ray.direction * 3, ray.origin, ForceMode.VelocityChange);
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay (ray.origin, ray.direction * rayLength);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Pick Up")) {
			other.gameObject.SetActive (false);
		}
	}
}
