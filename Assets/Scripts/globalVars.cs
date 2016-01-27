using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;

public class globalVars : MonoBehaviour {

	public string filePath;

	public GameObject overObject;
	public GameObject selectedObject;

	//Control which camera is active
	public Camera mainCam;
	public Camera mixerCam;
	public Camera soundCam;
	public Camera importCam;
	public Camera current;
	//Control which canvas is active
	public Canvas mainCanvas; 
	public Canvas mixerCanvas;
	public Canvas soundCanvas;
	public Canvas importCanvas;
	public Canvas currentCanvas;

	//To handle groups
	public Hashtable allGroups;

	//Increment soundName 
	public int soundNumber = 1;

	//Play click(metronome)
	public Toggle clickToggle;

	//All created sounds(master)
	List<AudioSource> allSounds;
	//Group 1
	List<AudioSource>allGroupA;
	//Group 2
	//etc

	//For snap
	public Slider snapSelector;
	public Toggle snapping;
	public Toggle metroSnap;
	public Text snapText;
	public float currentSnap = 1.0f;

	//For editing sounds
	public Button deleteSound;





	//idea for y axis
	//stereo left and right, pitch, allow selection?

	// Use this for initialization
	void Start () {
		//set file path
		filePath =  Application.dataPath + "/Downloads/";

		allGroups = new Hashtable ();
		allSounds = new List<AudioSource> ();
		allGroupA = new List<AudioSource> ();
		allGroups.Add ("Master", allSounds);
		allGroups.Add ("GroupA", allGroupA);

		//File.Delete(Application.persistentDataPath+ "/dataFile");
		checkTokenInfo ();
		//validReady = checkTokenInfo ();
		//if(!validReady)
		//	GameObject.Find ("importCam").GetComponent<importScript> ().refreshToken ();

		//Debug.Log (System.DateTime.Now.);

		//hideButton (deleteSound);

		//Debug.Log (HttpGet ( "https://www.freesound.org/apiv2/sounds/333489/download/"));
		//StartCoroutine(HttpGet());
	}

	//for free sound 
	public bool firstTimeLoad;
	public bool validReady;
	public string accessToken;

	void checkTokenInfo()
	{
		//first check if this file is even there
		if (File.Exists (Application.persistentDataPath + "/dataFile")) {
			//StreamReader a = File.OpenRead (Application.dataPath + "/dataFile");
			importScript.accessInfo temp = importScript.accessInfo.createFromJSON (File.ReadAllText(Application.persistentDataPath + "/dataFile"));
			System.DateTime actualTime = System.DateTime.Parse(temp.expireTime);
			//1 if date is valid, -1 otherwise
			if (System.DateTime.Compare (actualTime, System.DateTime.Now) != -1) {
				accessToken = temp.access_token;
				validReady = true;
			}
			else
			{
				if (GameObject.Find ("importCam").GetComponent<importScript> ().refreshToken (temp.refresh_token)) {
					validReady = true;
				} else {
					//error
				}
				//return false;
				//GameObject.Find ("importCam").GetComponent<importScript> ().refreshToken ();
			}
			Debug.Log (System.DateTime.Compare (actualTime, System.DateTime.Now));
		} else {
			//fresh login
			firstTimeLoad = true;
			validReady = false;
		}
	}

	string URL = "https://www.freesound.org/apiv2/sounds/333489/download/";
	//test
	//IEnumerator HttpGet()
	//{


	//}

	//For double clicking
	bool oneClick = false;
	bool timerRunning;
	float timerDC;
	float delay = .25f;
	
	// Update is called once per frame
	void Update () {


		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			cameraHelp (mixerCam, mixerCanvas);
			/*
			mainCam.enabled = false;
			mainCanvas.enabled = false;
			mixerCam.enabled = true;
			mixerCanvas.enabled = true;
			soundCam.enabled = false;
			soundCanvas.enabled = false;
			current = mixerCam;
			currentCanvas = mixerCanvas;
			*/
		}
		//Main
		else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			cameraHelp (mainCam, mainCanvas);
			/*
			mainCam.enabled = true;
			mainCanvas.enabled = true;
			mixerCam.enabled = false;
			mixerCanvas.enabled = false;
			soundCam.enabled = false;
			soundCanvas.enabled = false;
			current = mainCam;
			currentCanvas = mainCanvas;
			*/
		}

		//scroll, mostly just in editor
		if(current == mainCam){
			if (Input.mousePosition.x > Screen.width - 20) {
				mainCam.transform.position = new Vector3 (mainCam.transform.position.x + .05f, mainCam.transform.position.y, mainCam.transform.position.z);
			} else if (Input.mousePosition.x < 20) {
				mainCam.transform.position = new Vector3 (mainCam.transform.position.x - .05f, mainCam.transform.position.y, mainCam.transform.position.z);
			}
				

		}



		if (Input.GetMouseButtonDown (0)) {
			if (!oneClick) {
				oneClick = true;
				timerDC = Time.time;
			} else {
				oneClick = false;
				launchEditMenu ();
			}
			
		}
		if (oneClick) {
			if ((Time.time - timerDC) > delay)
				oneClick = false;
		}

		//Hide/Show Edit Button
		//if (selectedObject)
		//	editSound.enabled = true;
		//else
		//	editSound.enabled = false;

		//Camera control
		//Different controls if inside 
		/*
		if (!soundCam.enabled) {
			//Mixer
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				mainCam.enabled = false;
				mixerCam.enabled = true;
				soundCam.enabled = false;
				current = mixerCam;
			}
			//Main
			else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				mainCam.enabled = true;
				mixerCam.enabled = false;
				soundCam.enabled = false;
				current = mainCam;
			}
			//Sound
			else if (Input.GetKeyDown (KeyCode.UpArrow) && current == mainCam) {
				mainCam.enabled = false;
				mixerCam.enabled = false;
				soundCam.enabled = true;
				current = soundCam;
			}
		} 
		else {
			//To get back to main
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				mainCam.enabled = true;
				mixerCam.enabled = false;
				soundCam.enabled = false;
				current = mainCam;
			}
		}
		*/


	}

	public Dropdown groupOptions;
	public void updateSelectedGroup()
	{
		selectedObject.GetComponent<soundScript> ().groupName = groupOptions.options[groupOptions.value].text;
		selectedObject.GetComponent<soundScript> ().updateSoundSettings (false);
		/*
		string tempName = selectedObject.GetComponent<soundScript> ().groupName;
		//remove from other list if needed and clear features
		if (tempName != "") {
			List<AudioSource> a = (List<AudioSource>)allGroups[tempName];
			a.Remove(selectedObject.GetComponent<AudioSource>());
			//disable features
		}
		*/
	}

	//Dropdown for sounds
	public Dropdown soundOptions;
	//Bank for all sounds
	public soundBank sb;
	//Get sound for sound object
	public AudioClip getSound(int index)
	{
		return sb.getSoundAtIndex (index-1);
	}

	//Launch edit menu for selected sound
	//Go to edit menu
	public void launchEditMenu()
	{
		if (selectedObject) {
			cameraHelp (soundCam, soundCanvas);
			/*
			mainCam.enabled = false;
			mainCanvas.enabled = false;
			mixerCam.enabled = false;
			mixerCanvas.enabled = false;
			soundCam.enabled = true;
			soundCanvas.enabled = true;
			current = soundCam;
			currentCanvas = soundCanvas;
			*/
			//Set selected object to be edited
			soundCam.GetComponent<soundEditScript> ().soundReference = selectedObject;
			soundCam.GetComponent<soundEditScript> ().enabled = true;
		}
	}
	//with group options
	public void launchEditMenu(string group)
	{
		cameraHelp (soundCam, soundCanvas);
		/*
		mainCam.enabled = false;
		mainCanvas.enabled = false;
		mixerCam.enabled = false;
		mixerCanvas.enabled = false;
		soundCam.enabled = true;
		soundCanvas.enabled = true;
		current = soundCam;
		currentCanvas = soundCanvas;
		*/
		//Set selected object to be edited
		soundEditScript temp = soundCam.GetComponent<soundEditScript> ();
		temp.groupName = group;
		temp.enabled = true;
	}
		

	public void backToMain()
	{
		
		soundEditScript temp = current.gameObject.GetComponent<soundEditScript> ();
		temp.soundReference = null;
		temp.groupName = "";
		temp.enabled = false;

		cameraHelp (mainCam, mainCanvas);
		/*
		mainCam.enabled = true;
		mainCanvas.enabled = true;
		mixerCam.enabled = false;
		mixerCanvas.enabled = false;
		soundCam.enabled = false;
		soundCanvas.enabled = false;
		current = mainCam;
		currentCanvas = mainCanvas;
		*/
		//Disable script



	}
		
	public void snapChanged()
	{
		currentSnap = 1 / snapSelector.value;
		Debug.Log ("Sna " + currentSnap);
		snapText.text = "Snaps/Second:" + snapSelector.value.ToString();
	}

	//Go to import menu
	public void launchImport()
	{
		/*
		current.enabled = false;
		currentCanvas.enabled = false;
		current = importCam;
		currentCanvas = importCanvas;
		current.enabled = true;
		current.enabled = true;
		*/
		cameraHelp (importCam,importCanvas);
	}

	//helper for changing cams
	void cameraHelp(Camera a,Canvas b)
	{
		Debug.Log (current.name);
		current.enabled = false;
		currentCanvas.enabled = false;
		current = a;
		currentCanvas = b;
		current.enabled = true;
		currentCanvas.enabled = true;

	}

	public void deleteHelper()
	{
		if(selectedObject)
			selectedObject.GetComponent<soundScript> ().deleteSound ();
	}

	void hideButton(Button a)
	{
		deleteSound.enabled = false;
		deleteSound.GetComponent<Image> ().material.color = Color.clear;
		deleteSound.GetComponentInChildren<Text> ().color = Color.clear;
	}

	void showButton(Button a)
	{
		
	}
	


}
