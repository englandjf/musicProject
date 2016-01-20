using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class barScript : MonoBehaviour {

	Vector3 leftSide,rightSide;
	int songLength = 20;

	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = new Vector3 (-10, 5, 0);
		leftSide = startPos;
		rightSide = new Vector3 (10, 5, 0);
		startTime = Time.time;
		journeyLength = Vector3.Distance(leftSide, rightSide);
		Debug.Log ("Start time " + Time.time);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!stopped) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			if (transform.position != rightSide)
				transform.position = Vector3.Lerp (leftSide, rightSide, fracJourney);
			else {
				startTime = Time.time;
				transform.position = leftSide;
				stopped = true;
				Debug.Log ("End Time " + Time.time);
			}
		}

	}

	public Text lengthText;
	public void updateRightSide(bool increase)
	{
		if (increase) {
			songLength++;
			rightSide = new Vector3 (rightSide.x+1, 5, 0);
		} else {
			songLength--;
			rightSide = new Vector3 (rightSide.x-1, 5, 0);
		}

		journeyLength = Vector3.Distance(leftSide, rightSide);
		lengthText.text = "Length: " + songLength;

		//reset bar
		this.transform.position = startPos;
		stopped = true;
	}

	bool stopped = true;
	public void changePlay(bool stop)
	{
		if (stop) {
			this.transform.position = startPos;
			stopped = true;
		} else {
			stopped = false;
			startTime = Time.time;
		}
	}



}
