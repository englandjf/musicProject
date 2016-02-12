using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class soundBank : MonoBehaviour {

	//NEED TO LOOK INTO SYNCING ONCE NEW SOUNDS HAVE BEEN ADDED

	//For now just have one bank
	//But in the future have multiple based on categories

	//Include some base sounds
	//If others are wanted, use freesound.org api calls

	//List of all sounds, will in future get from files
	public List<AudioClip> allList;
	//downloaded sounds
	public List<AudioClip> downloadedList;//load sounds from specific location
	//Dropdown of all sounds
	public Dropdown soundDropdown;

	//global vars reference
	public globalVars gv;

	//check if new sounds have been download
	bool newSoundCheck = false;
	//updated when sounds are downloaded 
	public List <string> addedSounds;

	// Use this for initialization
	void Start () {
		loadDownloadedSounds ();
		addedSounds = new List<string> ();
		//allList = new List<AudioClip> ();
		//Clear and add title

		setupDropdown ();



	}

	//load newly added sounds into app
	void loadAddedSounds()
	{
		foreach (string fileName in addedSounds) {
			//AudioClip loadedClip = Resources.Load<AudioClip> (Application.persistentDataPath + "/Downloads/" + fileName);
			StartCoroutine(loadFile(Application.persistentDataPath + "/Downloads/" + fileName));
			//downloadedList.Add (loadedClip);
		}
		addedSounds.Clear ();
	}

	//do at launch to grab any downloaded sounds 
	void loadDownloadedSounds()
	{
		//StartCoroutine (loadFile ());
		//Directory check
		if(!Directory.Exists(Application.persistentDataPath+"/Downloads"))
			Directory.CreateDirectory(Application.persistentDataPath+"/Downloads");

		downloadedList = new List<AudioClip> ();

		string[] downSounds = System.IO.Directory.GetFiles (Application.persistentDataPath + "/Downloads");
		Debug.Log (downSounds.Length);
		for (int i = 0; i < downSounds.Length; i++) {
			//dont get meta files
			if (!downSounds [i].Contains (".meta")) {
				Debug.Log (downSounds [i]);
				StartCoroutine (loadFile (downSounds[i]));
			}
		}


	}
	/*
	 * var bytes = System.IO.File.ReadAllBytes(Application.dataPath+Saves.ToString()+".png");
	var tex = new Texture2D(4,4);
	tex.LoadImage(bytes);
	*/

	IEnumerator loadFile(string filePath)
	{
		Debug.Log ("Enter");
		//byte[] loadedData = System.IO.File.ReadAllBytes (filePath);
		//AudioClip tempClip = loadedData;
		WWW www = new WWW("file://" + filePath);
		int tempIndex = filePath.IndexOf ("Downloads");
		Debug.Log (filePath.Remove(0,tempIndex+10));
		//Object[] tempAll = Resources.LoadAll (Application.persistentDataPath + "/Resources/Downloads");
		//AudioClip loadedClip = Resources.Load<AudioClip> (path);
		yield return www;

		Debug.Log (www.error);
			
		AudioClip tempClip = www.audioClip;
		tempClip.name = filePath.Remove (0, tempIndex + 10);
		downloadedList.Add (tempClip);
		downloadMenu.Add (new Dropdown.OptionData (tempClip.name));
	}
	
	// Update is called once per frame
	void Update () {
		//main view is selected
		if (gv.current == gv.mainCam && !newSoundCheck) {
			Debug.Log ("Checking new sounds");
			//load new sounds from file system
			loadAddedSounds();
			newSoundCheck = true;
		}
		//not selected
		else if(gv.current != gv.mainCam && newSoundCheck)
		{
			newSoundCheck = false;
			addedSounds.Clear ();
			Debug.Log ("reset");
		}
	}

	public AudioClip getSoundAtIndex(int index)
	{
		Debug.Log (index);
		if (currentMenuState == menuState.included)
			return allList [index];
		else if (currentMenuState == menuState.downloads)
			return downloadedList [index];
		else
			return null;
	}


	List<Dropdown.OptionData> mainMenu;
	List<Dropdown.OptionData> downloadMenu;
	List<Dropdown.OptionData> includedSounds;
	public menuState currentMenuState;
	void setupDropdown()
	{
		currentMenuState = menuState.main;
		mainMenu = new List<Dropdown.OptionData> ();
		mainMenu.Add(new Dropdown.OptionData("Sounds"));
		mainMenu.Add (new Dropdown.OptionData ("Downloads"));
		mainMenu.Add (new Dropdown.OptionData (""));
		soundDropdown.options = mainMenu;
		soundDropdown.value = 2;

		downloadMenu = new List<Dropdown.OptionData> ();
		downloadMenu.Add (new Dropdown.OptionData ("Back"));

		includedSounds = new List<Dropdown.OptionData> ();
		includedSounds.Add (new Dropdown.OptionData ("Back"));

		Debug.Log (allList.Count);
		for (int i =0; i < allList.Count; i++) {
			includedSounds.Add(new Dropdown.OptionData(allList[i].name));
		}
	}

	public enum menuState {main,downloads,included};
 	//handle change to download list
	public void listValueChanged()
	{
		Debug.Log ("VAlue " + soundDropdown.value + currentMenuState);
		if (currentMenuState == menuState.main) {
			//got to sound menu
			if (soundDropdown.value == 0) {
				soundDropdown.options = includedSounds;
				currentMenuState = menuState.included;
				soundDropdown.value = 1;
			}
			//go to download menu
			else if(soundDropdown.value == 1)
			{
				soundDropdown.options = downloadMenu;
				currentMenuState = menuState.downloads;
				soundDropdown.value = 1;
			}
		} else if (currentMenuState == menuState.downloads) {
			//go back
			if (soundDropdown.value == 0) {
				soundDropdown.options = mainMenu;

				currentMenuState = menuState.main;
				soundDropdown.value = 2;
			}
		} else if (currentMenuState == menuState.included) {
			if(soundDropdown.value == 0)
			{
				soundDropdown.options = mainMenu;
				currentMenuState = menuState.main;
				soundDropdown.value = 2;
			}
		}
	}


		
}
