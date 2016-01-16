using UnityEngine;
using System.Collections;

public class barScript : MonoBehaviour {

	Vector3 leftSide,rightSide;

	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	// Use this for initialization
	void Start () {
		leftSide = this.transform.position;
		rightSide = new Vector3 (10, 5, 0);
		startTime = Time.time;
		journeyLength = Vector3.Distance(leftSide, rightSide);
		Debug.Log ("Start time " + Time.time);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		if(transform.position != rightSide)
			transform.position = Vector3.Lerp (leftSide, rightSide, fracJourney);
		else
		{
			startTime = Time.time;
			transform.position = leftSide;
			Debug.Log("End Time " + Time.time);
		}

	}

}
