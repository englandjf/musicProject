using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;

public class globalVars : MonoBehaviour {


	//Things left to do
	/*
	 * Save the song
	 * Clean up UI
	 * Figure out what to do with group
	 * handle the shit out of those exceptions
	 * 
	//idea for y axis
	//stereo left and right, pitch, allow selection?
	 */

	public string filePath;

	//assigned when the mouse/touch is over an object
	public GameObject overObject;
	//assigned when the mouse/touch is over an object and is clicked
	public GameObject selectedObject;

	//Control which camera is active
	//Camera for the main track 
	public Camera mainCam;
	//Camera for the group/mixer settings
	public Camera mixerCam;
	//Camera for the sound editor
	public Camera soundCam;
	//Camera for the Free Sound search/download
	public Camera importCam;
	//Camera that is active
	public Camera current;

	//Control which canvas is active
	//Canvas for the main track
	public Canvas mainCanvas; 
	//Canvas for group/mixer settings
	public Canvas mixerCanvas;
	//Canvas for the sound editor
	public Canvas soundCanvas;
	//Canvas for the Free Sound search/download
	public Canvas importCanvas;
	//Canvas that is active
	public Canvas currentCanvas;

	//To handle all groups
	public Hashtable allGroups;

	//Increment soundName
	public int soundNumber = 1;

	//Play click(metronome) if enabled
	public Toggle clickToggle;

	//All created sounds(master)
	List<AudioSource> allSounds;
	//Group 1
	List<AudioSource>allGroupA;
	//Group 2
	//etc

	//For snap
	public Slider snapSelector;
	//Snap is on when enabled
	public Toggle snapping;
	//Snaps to metronome if enabled
	public Toggle metroSnap;
	//Shows snaps per second
	public Text snapText;
	//Starting snap
	public float currentSnap = 1.0f;

	//For editing sounds
	public Button deleteSound;




	// Use this for initialization
	void Start () {
		//set file path
		filePath =  Application.dataPath+ "/Downloads/";

		//Setup tables for groups
		allGroups = new Hashtable ();
		allSounds = new List<AudioSource> ();
		allGroupA = new List<AudioSource> ();
		allGroups.Add ("Master", allSounds);
		allGroups.Add ("GroupA", allGroupA);

		//File.Delete(Application.persistentDataPath+ "/dataFile");

		//verifies if the API token is valid
		//checkTokenInfo ();
	}

	//for free sound 
	//True if there is no file information for Free Sound
	public bool firstTimeLoad;
	//True if token is valid
	public bool validReady;
	//Stores the access token used for Free Sound
	public string accessToken;

	//Verifies the validity of the Free Sound token in the file system
	void checkTokenInfo()
	{
		//Checks if the file is present
		if (File.Exists (Application.persistentDataPath + "/dataFile")) {
			importScript.accessInfo temp = importScript.accessInfo.createFromJSON (System.IO.File.ReadAllText(Application.persistentDataPath + "/dataFile"));
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
			}
			Debug.Log (System.DateTime.Compare (actualTime, System.DateTime.Now));
		}
		//If not present
		else {
			//fresh login
			firstTimeLoad = true;
			validReady = false;
		}
	}



	//For double clicking
	/*
	bool oneClick = false;
	bool timerRunning;
	float timerDC;
	float delay = .25f;
	*/
	
	// Update is called once per frame
	void Update () {

		//Will need to change this, but is used to get to group/mixer options
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			cameraHelp (mixerCam, mixerCanvas);
		}
		//Will need to change this, but goes to main track
		else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			cameraHelp (mainCam, mainCanvas);
		}
			
		//scroll, mostly just in editor, will probably eventually take out 
		/*
		if(current == mainCam && Input.GetMouseButton(1)){
			if (Input.mousePosition.x > Screen.width/2) {
				mainCam.transform.position = new Vector3 (mainCam.transform.position.x + .05f, mainCam.transform.position.y, mainCam.transform.position.z);
			} else if (Input.mousePosition.x < Screen.width/2) {
				mainCam.transform.position = new Vector3 (mainCam.transform.position.x - .05f, mainCam.transform.position.y, mainCam.transform.position.z);
			}	
		}
		*/


		//double click
		//not working correctly, might just make button, will take out soon
		/*
		if (Input.GetMouseButtonDown (0) && selectedObject) {
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
		*/

		//Only allow one toggle for yAxis option
		//yValueCheck();
	}


	//Drop down to show group options, might not include in first version because of stability
	public Dropdown groupOptions;
	//Not currently used
	/*
	public void updateSelectedGroup()
	{
		selectedObject.GetComponent<soundScript> ().groupName = groupOptions.options[groupOptions.value].text;
		selectedObject.GetComponent<soundScript> ().updateSoundSettings (false);

		string tempName = selectedObject.GetComponent<soundScript> ().groupName;
		//remove from other list if needed and clear features
		if (tempName != "") {
			List<AudioSource> a = (List<AudioSource>)allGroups[tempName];
			a.Remove(selectedObject.GetComponent<AudioSource>());
			//disable features
		}

	}
	*/

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
	public void launchEditMenu()
	{
		if (selectedObject) {
			cameraHelp (soundCam, soundCanvas);
			soundCam.GetComponent<soundEditScript> ().soundReference = selectedObject;
			soundCam.GetComponent<soundEditScript> ().enabled = true;
		}
	}

	//Launch edit menu with group 
	public void launchEditMenu(string group)
	{
		cameraHelp (soundCam, soundCanvas);
		//Set selected object to be edited
		soundEditScript temp = soundCam.GetComponent<soundEditScript> ();
		temp.groupName = group;
		temp.enabled = true;
	}
		
	//Return to main track
	public void backToMain()
	{
		
		soundEditScript temp = current.gameObject.GetComponent<soundEditScript> ();
		temp.enabled = false;
		cameraHelp (mainCam, mainCanvas);
	}

	//Called when the snap value is modified 
	public void snapChanged()
	{
		currentSnap = 1 / snapSelector.value;
		Debug.Log ("Sna " + currentSnap);
		snapText.text = "Snaps/Second:" + snapSelector.value.ToString();
	}

	//Go to import menu
	public void launchImport()
	{
		cameraHelp (importCam,importCanvas);
	}

	//Helper for changing cams
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

	//assigned in editor
	public void deleteHelper()
	{
		Debug.Log (groupOptions.value);
		//need to do some tweaks to delete unwanted deletions
		if (selectedObject && sb.soundDropdown.value != 0)
			selectedObject.GetComponent<soundScript> ().deleteSound ();
	}

	//might use later
	/*
	void hideButton(Button a)
	{
		deleteSound.enabled = false;
		deleteSound.GetComponent<Image> ().material.color = Color.clear;
		deleteSound.GetComponentInChildren<Text> ().color = Color.clear;
	*/

	/*
	void showButton(Button a)
	{
		
	}
	*/

	//For modifying song settings
	public GameObject songInfo;
	//Called to show/hide song settings
	public void showSongInfo()
	{
		if(songInfo.activeSelf)
			songInfo.SetActive(false);
		else
			songInfo.SetActive(true);
	}


	public Toggle yPitch;
	public Toggle yStereo;
	public Toggle yVolume;
	public string yCurrent;

	public void yValueChange(Toggle current)
	{
		Debug.Log (current.name);
		yCurrent = current.name;
	}

	/*
	void yValueCheck()
	{
		if (yPitch.isOn) {
			yStereo.isOn = false;
			yVolume.isOn = false;
		} else if (yStereo.isOn) {
			yPitch.isOn = false;
			yVolume.isOn = false;
		} else if (yVolume.isOn) {
			yPitch.isOn = false;
			yStereo.isOn = false;
		}
	}
	*/
	


}
