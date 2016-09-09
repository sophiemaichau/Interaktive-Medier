using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Ports;
using System;

public class ClickController : MonoBehaviour {

	public Text countText;
	public Text timeText;
	public GameObject cube;
	public GameObject cube2;
	public GameObject pointer; 

	private int count;
	private float time;
	private float width;
	private float distance;
	private float id;
	private float mt;
	private int val2;
	private int val3;
	private int val;
	private int val4;

	SerialPort port;

	// Use this for initialization
	void Start () {
		count = 10;
		time = 0;
		SetCountText ();
	    cube2.gameObject.GetComponent<Renderer> ().material.color = Color.yellow;

		port = new SerialPort ("/dev/cu.wchusbserialfa130", 9600);
		port.Open();
	}

	// Update is called once per frame
	void Update () {

		if (port == null) {
			return;
		}
		if (port.IsOpen) {
			port.ReadTimeout = 1;
			try{
				val2 = port.ReadByte();
				val3 = port.ReadByte();
				val = val2 * 256 + val3;
				val4 = port.ReadByte();
				print(val4);
				pointer.transform.position = new Vector3(4*val/1023f-2, transform.position.y, transform.position.z);

			} catch(TimeoutException ex){

			}
			//int val = port.ReadByte();
			//print(val);
		}

		// Vector3 xPos = new Vector3(4*val/1023f-2, transform.position.y, transform.position.z);

		Ray ray = new Ray (pointer.transform.position, pointer.transform.forward);
		//Ray ray = GameObject.Find ("pointer").transform.position;
		// pointer.transform.getpo .ScreenPointToRay (xPos);
		Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);

		if (val4==0) { //button 
			if (count < 10 && count > 0) {
				time = Time.unscaledTime;
			}
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 100)){
				if (count == 0 || count % 2 == 0) {
					cube.gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
					cube2.gameObject.GetComponent<Renderer> ().material.color = Color.white;
				} else {
					cube2.gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
					cube.gameObject.GetComponent<Renderer> ().material.color = Color.white;
				}
				count--;
				SetCountText ();
			}
		}
	}

	void SetCountText(){
		countText.text = "Clicks left: " + count.ToString ();
		if (count == 0) {
			count = -1;
			mt = time / 10;
			timeText.text = "MT: " + mt.ToString () + " seconds";
			WriteToFile ();
		}
	}

	void WriteToFile(){
		width = cube.gameObject.transform.localScale.x;
		distance = cube.gameObject.transform.localPosition.x * 2;
		id = Mathf.Log (((2 * distance) / width), 2);

		System.IO.File.AppendAllText("/Users/sophiemaichau/desktop/tests.txt",
			id.ToString() + " , " + mt.ToString() + "\n");
	}

	void onApplicationQuit(){
		if (port != null && port.IsOpen) {
			port.Close ();
		}
	}
}