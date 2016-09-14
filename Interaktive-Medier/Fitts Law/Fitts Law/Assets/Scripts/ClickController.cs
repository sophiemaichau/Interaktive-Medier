using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Ports;
using System;

public class ClickController : MonoBehaviour {

	public Text countText;
	public Text timeText;
	public GameObject cubeRight;
	public GameObject cubeLeft;
	public GameObject pointer; 
	public Rigidbody rb;

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
	private float val5;
	private float cubeRxPos;
	private float cubeLxPos;
	private float pointerXpos;
	private float lastClick;
	private Char controller = 'a';

	SerialPort port;

	// Use this for initialization
	void Start () {
		val4 = 1;
		count = 10;
		time = 0;
		SetCountText ();
		cubeLeft.gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
		rb = pointer.GetComponent<Rigidbody> ();

		port = new SerialPort ("/dev/cu.wchusbserialfa130", 9600);
		port.Open();
		width = cubeRight.transform.localScale.x;
		distance = cubeRight.transform.localPosition.x * 2;
		cubeRxPos = cubeRight.transform.position.x;
		cubeLxPos = cubeLeft.transform.position.x;
	}

	// Update is called once per frame
	void Update () {
		pointerXpos = pointer.transform.position.x;
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
				gameController(controller);
			} catch(TimeoutException){

			}
			//			int val = port.ReadByte();
			print(val);
		}

		if (val4==0) { //button

			if (lastClick > (Time.time - 0.2f)) return;
			lastClick = Time.time;

			if (count < 10 && count > 0) {
				time = Time.unscaledTime;
			}

			if( (cubeLxPos-width/2 >= pointerXpos || pointerXpos <= cubeLxPos+width/2) || 
				(cubeRxPos-width/2 >= pointerXpos || pointerXpos <= cubeRxPos+width/2) ){
				if (count == 0 || count % 2 == 0) {
					cubeRight.gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
					cubeLeft.gameObject.GetComponent<Renderer> ().material.color = Color.white;
				} else {
					cubeLeft.gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
					cubeRight.gameObject.GetComponent<Renderer> ().material.color = Color.white;
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
		id = Mathf.Log (((2 * distance) / width), 2);

		System.IO.File.AppendAllText("/Users/sophiemaichau/desktop/tests.txt",
			id.ToString() + " , " + mt.ToString() + "\n");
	}

	void onApplicationQuit(){
		if (port != null && port.IsOpen) {
			port.Close ();
		}
	}

	void gameController(Char c){
		if(c=='p'){
			pointer.transform.position = new Vector3(4*val/1023f-2, transform.position.y, transform.position.z);
		}
		if(c=='r'){
			if (val >= 527) {
				pointer.transform.position = new Vector3(transform.position.x+2, transform.position.y, transform.position.z);
			}
			if (val < 490) {
				pointer.transform.position = new Vector3 (transform.position.x - 2, transform.position.y, transform.position.z);
			}
//			if(val < 527 || val >= 490){
//				pointer.transform.position.x = 0f;
//			}
		}
		if(c=='a'){
			if(val >= 527){
				rb.AddForce (pointer.transform.right * 2f);
			}
			if(val < 490){
				rb.AddForce (pointer.transform.right * -2f);
			}
		}
	}
}