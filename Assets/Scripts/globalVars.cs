using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;

public class globalVars : MonoBehaviour {

	//https://www.freesound.org/apiv2/oauth2/authorize/
	//auth code for free sound
	//43cd1af77fac90c89b7d8c092c801e9003454d31 ->expires every 10 minutes
	//access token
	//413d294614cf0e1eef31f9a5e51fd681bae739f4 -> expires every 24 hours
	//curl -X POST -d "client_id=4656d370ed84017ab3bc&client_secret=081dc0326eb35d42dd3e44fda191713b9fcdba63&grant_type=authorization_code&code=43cd1af77fac90c89b7d8c092c801e9003454d31" https://www.freesound.org/apiv2/oauth2/access_token/
	//for refresh, curl -X POST -d "client_id=YOUR_CLIENT_ID&client_secret=YOUR_CLIENT_SECRET&grant_type=refresh_token&refresh_token=REFRESH_TOKEN" "https://www.freesound.org/apiv2/oauth2/access_token/"
	//for getting sounds
	//curl -H "Authorization: Bearer 413d294614cf0e1eef31f9a5e51fd681bae739f4" 'https://www.freesound.org/apiv2/sounds/333489/download/

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
	public Text snapText;
	public float currentSnap = 1.0f;

	//For editing sounds
	public Button editSound;

	// Use this for initialization
	void Start () {
		allGroups = new Hashtable ();
		allSounds = new List<AudioSource> ();
		allGroupA = new List<AudioSource> ();
		allGroups.Add ("Master", allSounds);
		allGroups.Add ("GroupA", allGroupA);

		//Debug.Log (HttpGet ( "https://www.freesound.org/apiv2/sounds/333489/download/"));
		StartCoroutine(HttpGet());
	}

	string URL = "https://www.freesound.org/apiv2/sounds/333489/download/";
	//test
	IEnumerator HttpGet()
	{
		//THIS WORKS
		Dictionary<string,string> headers = new Dictionary<string, string> (); 
		headers.Add("Authorization",  "Bearer 413d294614cf0e1eef31f9a5e51fd681bae739f4");
		WWW www = new WWW (URL,null,headers);
		yield return www;
		//AudioClip temp = www.GetAudioClip(true,true,AudioType.WAV);
		AudioClip temp = www.GetAudioClipCompressed(false,AudioType.WAV);
		AudioSource asTest = gameObject.AddComponent<AudioSource> ();
		asTest.clip = temp;
		asTest.Play ();
		//Debug.Log (temp);


		/*
		HttpWebRequest req = WebRequest.Create (url) as HttpWebRequest;
		req.Headers.Add ("Authorization: Bearer 413d294614cf0e1eef31f9a5e51fd681bae739f4");
		string result = null;
		using (HttpWebResponse resp = req.GetResponse () as HttpWebResponse) {
			Stream a = resp.GetResponseStream ();
			BinaryReader b = new BinaryReader (a);

			//StreamReader reader = new StreamReader (resp.GetResponseStream ());
			//result = reader.ReadToEnd ();
		}
		WWW www;

		return result;
		*/


	}
	
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
		soundEditScript temp = soundCam.GetComponent<soundEditScript> ();
		temp.soundReference = null;
		temp.groupName = "";
		temp.enabled = false;


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
		current.enabled = false;
		currentCanvas.enabled = false;
		current = a;
		currentCanvas = b;
		current.enabled = true;
		currentCanvas.enabled = true;
	}

	


}
