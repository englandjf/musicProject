using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class createScript : MonoBehaviour {

	public GameObject soundObject;
	public globalVars gv;
	public Dropdown groupOptions;


	// Use this for initialization
	void Start () {
		//dgv = GameObject.Find ("main").GetComponent<globalVars> ();
		setupGroupOptions ();

		groupOptions.onValueChanged.AddListener (delegate {
			groupChanged ();
		});

		//Metronome
		metroMarks = new List<GameObject> ();
		columnStorage = new List<GameObject> ();
		rowStorage = new List<GameObject> ();
		makeMetronome (60);
		//Seconds
		makeSecondMarkers ();


	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && !gv.overObject && gv.current.name == "main" && gv.soundOptions.value != 0) {
			Vector3 mp = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
			mp.z = 0;
			if(mp.y < 3.5 && mp.y > -3.5){
				//if (!gv.selectedObject) {
					GameObject temp = (GameObject)Instantiate (soundObject, mp, this.transform.rotation);
					temp.name += gv.soundNumber;
					gv.soundNumber++;
					if (groupOptions.value != 0)
						temp.GetComponent<soundScript> ().groupName = groupOptions.options [groupOptions.value].text;
				//} else
				//	gv.selectedObject = null;
			}
		}
	}

	void setupGroupOptions()
	{
		List<Dropdown.OptionData> a = new List<Dropdown.OptionData> ();
		a.Add(new Dropdown.OptionData("Pick Group"));
		a.Add(new Dropdown.OptionData("GroupA"));
		groupOptions.options = a;

	}

	void groupChanged()
	{
		if (gv.selectedObject != null && groupOptions.value != 0)
			gv.selectedObject.GetComponent<soundScript> ().groupName = groupOptions.options [groupOptions.value].text;
	}

	//Will need bpm info
	//slider for changing bpms
	//Create metronome
	public GameObject metroSound;
	//reusable rather than creating them all again
	List<GameObject> metroMarks;
	void makeMetronome(float bpm)
	{
		//Calculate distance between markers from bpm
		float bps = bpm / 60;
		float distanceBet = 1 / bps;

		int listIndex = 0;
		for (float i = -10; i <= rightSidex; i+=distanceBet) {
			//Grab from list
			GameObject temp = null;
			if(listIndex != metroMarks.Count){
				temp = metroMarks[listIndex];
				temp.transform.position = new Vector3(i,-5,0);

			}
			//make new
			else{
				temp = (GameObject)Instantiate (metroSound, new Vector3 (i, -5, 0), this.transform.rotation);
				metroMarks.Add(temp);
			}
			listIndex++;


			//temp.name += i;
			
		}
		//clear extras
		for(int i = listIndex; i < metroMarks.Count;i++){
			GameObject tempMark = metroMarks[i];
			Destroy(tempMark);
			metroMarks.Remove(tempMark);
		}
	}

	//For changing BPM
	public Slider beatSlider;
	//For showing BPM
	public Text BPM;
	public void bpmChanged()
	{
		BPM.text = "BPM: " + beatSlider.value.ToString();
		makeMetronome (beatSlider.value);
	}


	public GameObject secondsLine;
	List <GameObject> columnStorage;
	List <GameObject> rowStorage;
	//Second markers
	void makeSecondMarkers()
	{
		int columnIndex = 0;
		//Columns
		for (int x =-10; x <= 10; x++) {
			GameObject temp = (GameObject)Instantiate(secondsLine,new Vector3(x,-4,0),this.transform.rotation);
			temp.GetComponent<LineRenderer>().SetPosition(0,new Vector3(x,-3.5f,0));
			temp.GetComponent<LineRenderer>().SetPosition(1,new Vector3(x,3.5f,0));
			columnStorage.Add (temp);

		}

		//Rows
		for(int y = -4; y < 4;y++)
		{
			GameObject temp = (GameObject)Instantiate(secondsLine,new Vector3(-10,0,0),this.transform.rotation);
			temp.GetComponent<LineRenderer>().SetPosition(0,new Vector3(-10,y+.5f,0));
			temp.GetComponent<LineRenderer>().SetPosition(1,new Vector3(rightSidex,y+.5f,0));
			rowStorage.Add (temp);
		}
	}

	public float rightSidex = 10;
	public void changedLength(bool more)
	{
		if (more) {
			rightSidex++;
			GameObject temp = (GameObject)Instantiate(secondsLine,new Vector3(rightSidex,-4,0),this.transform.rotation);
			temp.GetComponent<LineRenderer>().SetPosition(0,new Vector3(rightSidex,-3.5f,0));
			temp.GetComponent<LineRenderer>().SetPosition(1,new Vector3(rightSidex,3.5f,0));
			columnStorage.Add (temp);
		}
		else {
			rightSidex--;
			GameObject temp = columnStorage [columnStorage.Count - 1];
			Destroy (temp);
			columnStorage.Remove (temp);
		}

		//Redraw or add to metro and second markers
		makeMetronome (beatSlider.value);
		//Column update


		//Row update
		int yValue = -4;
		foreach(GameObject a in rowStorage){
			a.GetComponent<LineRenderer>().SetPosition(1,new Vector3(rightSidex,yValue+.5f,0));
			yValue++;
		}
	}
	
}
