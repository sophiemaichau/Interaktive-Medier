using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickController : MonoBehaviour {

	public Text countText;
	public Text timeText;
	public GameObject cube;
	public GameObject cube2;

	private int count;
	private float time;
	private float width;
	private float distance;
	private float id;
	private float mt;

	// Use this for initialization
	void Start () {
		count = 10;
		time = 0;
		SetCountText ();
		cube2.gameObject.GetComponent<Renderer> ().material.color = Color.yellow;

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) { //left mouse click
			if (count < 10 && count > 0) {
				time = Time.unscaledTime;
			}
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
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

}
