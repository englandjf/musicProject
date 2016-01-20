using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class soundScript : MonoBehaviour {

	//Main sound
	AudioSource mainSource;
	//Collider for playing sound
	BoxCollider2D bc;
	//Length of main sound
	float clipSize;
	//Reference to global vars
	public globalVars gv;

	//For mixer group
	public string groupName;

	//start & end time
	public float startTime;
	public float endTime;

	//Line for placemenet
	public LineRenderer guideLine;

	//Parent for placement
	//public GameObject parent;

	// Use this for initialization
	void Start () {
		gv = GameObject.Find ("main").GetComponent<globalVars> ();


		//Make object selected
		//gv = GameObject.Find ("main").GetComponent<globalVars> ();
		gv.selectedObject = this.gameObject;

		//Assign clip from sound bank based on dropdown selection
		mainSource = GetComponent<AudioSource> ();
		mainSource.clip = gv.getSound (gv.soundOptions.value);
		
		//Scale setup
		bc = GetComponent<BoxCollider2D> ();
		clipSize = mainSource.clip.length;
		this.transform.localScale = new Vector3 (clipSize, 1, 1);
		//bc.size = new Vector2 (clipSize, 1);

		//set parent to life side
		//parent = gameObject.transform.parent.gameObject;
		//parent.transform.position = this.transform.position;
		//parent.transform.position = new Vector3 (this.transform.position.x-(clipSize/2), this.transform.position.y, this.transform.position.z);

		//Add self to master list
		List<AudioSource> a =  (List<AudioSource>)gv.allGroups["Master"];
		a.Add (mainSource);

		//handleSnap ();

		//Set default values
		startTime = 0;
		endTime = mainSource.clip.length;

		//Setup guide line
		Debug.Log ("Scale" + this.transform.localScale.x / 2);
		//guideLine.SetPosition (0, new Vector3 (this.transform.localScale.x/2, 10, 0));
		//guideLine.SetPosition (1, new Vector3 (this.transform.localScale.x/2, -10, 0));


		//Previous group settings & master settings
		updateSoundSettings (true);
		//What happens if an effect has already been added
		//will need to check current settings
	}

	//If the mouse pointer enters the menu area
	bool invalid = false;

	// Update is called once per frame
	void Update () {
		//Initial placement
		Vector3 mp = gv.current.ScreenToWorldPoint(Input.mousePosition);
		mp.z = 0;

		if (mp.y >= 4 || mp.y <= -4)
			invalid = true;
		else
			invalid = false;

		//clear selection if selecting blank space or select object
		if (Input.GetMouseButtonDown (0) && !invalid) {	
			if (gv.overObject == null)
				gv.selectedObject = null;
			else
				gv.selectedObject = gv.overObject;
		} else if (Input.GetMouseButton (0) && gv.selectedObject == this.gameObject && !invalid) { //&& !initPlaced){
			
			this.transform.position = new Vector3 (mp.x + (transform.localScale.x / 2), mp.y, mp.z);
			//gv.selectedObject = this.gameObject;
			//gv.selectedObject.transform.position = mp;
		} else if (Input.GetMouseButtonUp (0) && gv.selectedObject == this.gameObject) {
			if(!invalid)
				handleSnap ();
		} 



		//Visually update group options and change color
		if (gv.selectedObject == this.gameObject) {
			if(groupName == "GroupA")
				gv.groupOptions.value = 1;
			else
				gv.groupOptions.value = 0;
			GetComponent<Renderer> ().material.color = Color.red;
		}
		else
			GetComponent<Renderer> ().material.color = Color.white;

		//Delete
		if (Input.GetMouseButtonDown (1) && gv.overObject == this.gameObject) {
			//Delete from list
			List<AudioSource> a =  (List<AudioSource>)gv.allGroups["Master"];
			a.Remove(mainSource);
			Destroy (this.gameObject);
			gv.selectedObject = null;
		}

		//Handle play
		//if (playTrack && !mainSource.isPlaying && Time.time < gameSoundEnd) {
		//	mainSource.Play ();
		//} else if (Time.time >= gameSoundEnd)
		//	playTrack = false;

		//Delays start
		if (mainSource.isPlaying) {
			if(Time.time <= gameSoundStart || Time.time >= gameSoundEnd)
				mainSource.mute = true;
			else
				mainSource.mute = false;
		}

		//Draw guide line
		float leftPos = transform.position.x - (transform.localScale.x/2); 
		if (gv.selectedObject == this.gameObject && gv.current.name == "main") {
			guideLine.SetPosition (0, new Vector3 (leftPos + .025f, 10, 0));
			guideLine.SetPosition (1, new Vector3 (leftPos + .025f, -10, 0));
			guideLine.enabled = true;

		}
		else
			guideLine.enabled = false;


	}

	//bool playTrack = false;
	//values used in game for sound
	//public float hitTime;
	public float gameSoundStart;//actual start time
	public float gameSoundEnd;
	void OnTriggerEnter2D(Collider2D a)
	{
		//playTrack = true;
		mainSource.Play ();
		//hitTime = Time.time;
		gameSoundStart = Time.time+startTime;
		gameSoundEnd = Time.time + endTime;
		//Debug.Log (Time.time + " " + gameSoundStart);
		//mainSource.Play ();
		//Debug.Log ("Tes");
	}


	void OnTriggerExit2D(Collider2D a)
	{
		mainSource.Stop ();
	}

	float currentSnap;
	void handleSnap()
	{
		if (gv.snapping.isOn) {
			//X based on scale(testing), will later be entered(override), default snap will be on be based on length
			/*
			for (float i = -10; i < 10; i+=gv.currentSnap) {
				if (transform.position.x >= i && transform.position.x <= i + gv.currentSnap) {
					Debug.Log(i);
					Vector3 a = new Vector3 (i + (clipSize / 2), Mathf.Round (transform.position.y), 0);
					transform.position = a;
				}
			}
			*/
			//bool isNeg;
			//if(transform.position.x < 0)
			//	isNeg = true;
			//else
			//	isNeg = false;
			float distance = Mathf.Abs(transform.position.x + 10 - (this.transform.localScale.x/2));
			int amountInside = Mathf.FloorToInt(distance/gv.currentSnap);
			float amountExtra = distance%gv.currentSnap;
			Vector3 a;
			Debug.Log(distance + " " + amountInside + " " + amountExtra + " " + (gv.currentSnap/2));
			if(amountExtra < (gv.currentSnap/2)){
				a = new Vector3 (-10+(amountInside * gv.currentSnap) + (clipSize / 2), Mathf.Round (transform.position.y), 0);
				//if(isNeg){
				//	a.x *= -1;
				//}
			}
			else{
				a = new Vector3 (-10+((amountInside+1) * gv.currentSnap) + (clipSize / 2), Mathf.Round (transform.position.y), 0);
				//if(isNeg){
				//	a.x *= -1;
				//}
			}
			transform.position = a;
		} else {
			//Just Y
			transform.position = new Vector3 (transform.position.x, Mathf.Round (transform.position.y), this.transform.position.z);
		}
	}

	void OnMouseEnter()
	{
		//Debug.Log ("ini");
		gv.overObject = this.gameObject;
	}

	void OnMouseExit()
	{
		gv.overObject = null;
	}

	//Syncs with the audio mixer by adding sound to list and grabbing settings
	public void updateSoundSettings(bool spawned)
	{
		//remove from other list if needed and clear features if it was assigned a group
		if (groupName != "" && !spawned) {
			List<AudioSource> sl = (List<AudioSource>)gv.allGroups[groupName];
			if(sl.IndexOf(GetComponent<AudioSource>()) != -1)
				sl.Remove(GetComponent<AudioSource>());
			AudioEchoFilter ef = GetComponent<AudioEchoFilter>();
			AudioChorusFilter cf = GetComponent<AudioChorusFilter>();
			AudioDistortionFilter df = GetComponent<AudioDistortionFilter>();
			AudioReverbFilter rf = GetComponent<AudioReverbFilter>();
			//Disable previous features
			if(ef)
				Component.Destroy(ef);
			if(cf)
				Component.Destroy(cf);
			if(df)
				Component.Destroy(df);
			if(rf)
				Component.Destroy(rf);
			//disable features
		}

		//Master sound grab update


		Debug.Log ("Gn " + groupName);
		//Group sound grab
		if (groupName != "") {
			//Add sound to list
			List<AudioSource> a = (List<AudioSource>)gv.allGroups [groupName];
			Debug.Log(a.Count);
			a.Add (GetComponent<AudioSource> ());
			//Update sound settings
		}


	}


}
