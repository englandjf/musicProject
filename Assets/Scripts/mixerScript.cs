using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class mixerScript : MonoBehaviour {

	public globalVars gv;

	//For referencing hashtable
	public string groupName;

	public Slider volumeSlider;
	public Dropdown effectsList;
	public Text textEffects;

	List<AudioSource> includedSounds;

	bool synced = false;

	//If sounds have been added already, have options for each
	bool echo = false;
	bool chorus = false;
	bool distortion = false;
	bool reverb = false;
	

	// Use this for initialization
	void Start () {
		//Debug.Log (gv.allGroups.ContainsKey ("Master"));
		//includedSounds = (List<AudioSource>)gv.allGroups [groupName];

		setUpEvents ();

	}
	
	// Update is called once per frame
	void Update () {
		//Have to delay slightly because of all groups setup
		if (!synced && Time.time > 1) {
			includedSounds = (List<AudioSource>)gv.allGroups [groupName];
			synced = true;
		}
	}

	void setUpEvents()
	{
		volumeSlider.onValueChanged.AddListener (delegate {
			volumeChange();
		});


		//Effects list
		effectsList.onValueChanged.AddListener (delegate {
			effectAdded ();
		});
		List <Dropdown.OptionData> allEffects = new List<Dropdown.OptionData> ();
		allEffects.Add (new Dropdown.OptionData ("Add Effect"));
		//allEffects [0].text = "Add Effect";
		//Echo
		allEffects.Add (new Dropdown.OptionData ("Echo"));
		//allEffects [1].text = "Echo";
		//Chorus
		allEffects.Add (new Dropdown.OptionData ("Chorus"));
		//allEffects [2].text = "Chorus";
		//Distortion
		allEffects.Add (new Dropdown.OptionData ("Distortion"));
		//allEffects [3].text = "Distortion";
		//Reverb
		allEffects.Add (new Dropdown.OptionData ("Reverb"));
		//allEffects [4].text = "Reverb";
		effectsList.options = allEffects;
	}

	void volumeChange()
	{
		//parentMixer.SetFloat ("Volume", volumeSlider.value);
		foreach (AudioSource a in includedSounds)
			a.volume = volumeSlider.value/100;//because volume is 0 to 1

	}

	void effectAdded()
	{
		//Handle each case
		//Echo
		Debug.Log (includedSounds.Count);
		foreach (AudioSource a in includedSounds) {
			//Echo
			Debug.Log(a.gameObject.name);
			if(effectsList.value == 1){
				a.gameObject.AddComponent<AudioEchoFilter> ();
				echo = true;
			}
			else if(effectsList.value == 2){
				a.gameObject.AddComponent<AudioChorusFilter>();
				chorus = true;
			}
			else if(effectsList.value == 3){
				a.gameObject.AddComponent<AudioDistortionFilter>();
				distortion = true;
			}
			else if(effectsList.value == 4){
				a.gameObject.AddComponent<AudioReverbFilter>();
				reverb = true;
			}
		}
		updateText ();


		//and reset
		effectsList.value = 0;

	}

	void updateText()
	{
		string allThings = "";
		if(echo)
			allThings += "Echo \n";
		if(chorus)
			allThings += "Chorus \n";
		if (distortion)
			allThings += "Distortion \n";
		if(reverb)
			allThings += "Reverb \n";
		textEffects.text = allThings;
	}



	//Edit Buttons
	public void editMaster()
	{
		gv.launchEditMenu ("Master");
	}
	public void editGroupA()
	{
		gv.launchEditMenu ("GroupA");
	}



}
