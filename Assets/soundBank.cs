using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class soundBank : MonoBehaviour {

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
		soundDropdown.options = new List<Dropdown.OptionData> ();
		soundDropdown.options.Add(new Dropdown.OptionData("Sounds"));
		setupDropdown ();


	}

	//load newly added sounds into app
	void loadAddedSounds()
	{
		foreach (string fileName in addedSounds) {
			AudioClip loadedClip = new AudioClip ();
			//File.
			//downloadedList.Add(
		}
	}

	//do at launch to grab any downloaded sounds 
	void loadDownloadedSounds()
	{
		
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
		return allList[index];
	}

	void setupDropdown()
	{
		Debug.Log (allList.Count);
		for (int i =0; i < allList.Count; i++) {
			soundDropdown.options.Add(new Dropdown.OptionData(allList[i].name));
		}
	}


		
}
