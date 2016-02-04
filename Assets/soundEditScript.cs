using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class soundEditScript : MonoBehaviour {

	//Add some sort of indicator on track if an effect has been added, or what group they are part of.

	//global vars
	public globalVars gv;

	//Child canvas reference
	public Canvas childCanvas;

	//For enabling controls
	public Toggle echoToggle;
	public Toggle chorusToggle;
	public Toggle reverbToggle;
	public Toggle distortionToggle;

	//Extra settings for each option
	public GameObject echoOptions;
	public GameObject chorusOptions;
	public GameObject reverbOptions;
	public GameObject distortionOptions;

	public GameObject soundReference;

	//Set when sound object is set every single time
	AudioEchoFilter echoFilter;
	AudioChorusFilter chorusFilter;
	AudioReverbFilter reverbFilter;
	AudioDistortionFilter distortionFilter;

	//Sounds sliders
	public Slider volumeSlider;
	public Slider pitchSlider;

	//Audio source object
	public AudioSource sourceSound;

	//start and end times
	public InputField startTime;
	public InputField endTime;


	// Use this for initialization
	void Start () {


		//Set toggle functions
		echoToggle.onValueChanged.AddListener (delegate {
			echoChanged();
		});
		chorusToggle.onValueChanged.AddListener (delegate {
			chorusChanged();
		});
		reverbToggle.onValueChanged.AddListener (delegate {
			reverbChanged();
		});
		distortionToggle.onValueChanged.AddListener (delegate {
			distortionChanged();
		});

		//Set Slider Functions
		volumeSlider.onValueChanged.AddListener (delegate {
			volumeChanged();
		});
		pitchSlider.onValueChanged.AddListener (delegate {
			pitchChanged();
		});

		//Set time functions
		startTime.onValueChange.AddListener (delegate {
			startTimeChanged();
		});
		endTime.onValueChange.AddListener (delegate {
			endTimeChanged();
		});



		this.enabled = false;
		childCanvas.enabled = false;


	}

	//Set only once
	bool soundRefSet = false;

	void OnEnable()
	{

		//special group settings


		Debug.Log (gv.current);
		//Pull information from sound object
		if (gv.current == GetComponent<Camera> ()) {
			if (groupName != "") {
				includedGroup = (List<AudioSource>)gv.allGroups [groupName];
				//Grab first sound as reference
				sourceSound = includedGroup[0];
				soundReference = sourceSound.gameObject;
			}
			else
				sourceSound = soundReference.GetComponent<AudioSource> ();
			pullInformation ();

		}
			



	}
	
	// Update is called once per frame
	void Update () {
		//Turn off canvas if it isnt selected
		if (gv.current != GetComponent<Camera> ())
			childCanvas.enabled = false;
		else
			childCanvas.enabled = true;

	}

	//Pull information fro gameobject
	void pullInformation()
	{

		//Filters
		//Echo
		echoFilter = soundReference.GetComponent<AudioEchoFilter> ();
		if (echoFilter) {
			echoToggle.isOn = true;
			echoOptions.GetComponent<echoScript> ().filterReference = echoFilter;
		}
		 else 
			echoToggle.isOn = false;
		//Chorus
		chorusFilter = soundReference.GetComponent<AudioChorusFilter> ();
		if (chorusFilter) {
			chorusToggle.isOn = true;
			chorusOptions.GetComponent<chorusScript> ().filterReference = chorusFilter;
		}
		else
			chorusToggle.isOn = false;
		//Reverb
		reverbFilter = soundReference.GetComponent<AudioReverbFilter> ();
		if (reverbFilter) {
			reverbToggle.isOn = true;
			reverbOptions.GetComponent<reverbScript> ().filterReference = reverbFilter;
		}
		else
			reverbToggle.isOn = false;
		//Distortion
		distortionFilter = soundReference.GetComponent<AudioDistortionFilter> ();
		if (distortionFilter) {
			distortionToggle.isOn = true;
			distortionOptions.GetComponent<distortionScript> ().filterReference = distortionFilter;
		}
		else
			distortionToggle.isOn = false;

		//Volume
		volumeSlider.value = sourceSound.volume;
		volumeText.text = volumeSlider.value.ToString();
		//Pitch
		pitchSlider.value = sourceSound.pitch;
		pitchText.text = pitchSlider.value.ToString();

		//Start & end
		startTime.text = soundReference.GetComponent<soundScript> ().startTime.ToString ();
		endTime.text = soundReference.GetComponent<soundScript> ().endTime.ToString ();


	}
	//Used by groups as well
	//Instead of applying setting to one, apply to all if group name != ""
	public string groupName = "";
	List<AudioSource> includedGroup;


	void echoChanged()
	{
		//Apply to one vs. apply to all
		//Echo
		if (echoToggle.isOn) {
			if(!echoFilter)
			{
				if (groupName == "") {
					echoFilter = soundReference.AddComponent<AudioEchoFilter> ();
				}
				else{
					foreach (AudioSource a in includedGroup) {
						a.gameObject.AddComponent<AudioEchoFilter> ();
					}
					//use first one as reference
					echoFilter = includedGroup[0].gameObject.GetComponent<AudioEchoFilter>();
				}
				echoOptions.GetComponent<echoScript> ().filterReference = echoFilter;
			}
			echoOptions.SetActive (true);
		} else {
			if(echoFilter)
			{
				if(groupName == ""){
					Component.Destroy(soundReference.GetComponent<AudioEchoFilter>());
				}
				else{
					foreach (AudioSource a in includedGroup) {
						Component.Destroy (a.gameObject.GetComponent<AudioEchoFilter> ());
					}
				}
				echoFilter = null;
				echoOptions.GetComponent<echoScript> ().filterReference = null;
			}
			echoOptions.SetActive (false);
		}
	}

	void chorusChanged()
	{
		//Chorus
		if (chorusToggle.isOn) {
			if(!chorusFilter)
			{
				if (groupName == "") {
					chorusFilter = soundReference.AddComponent<AudioChorusFilter> ();
				}
				else{
					foreach (AudioSource a in includedGroup) {
						a.gameObject.AddComponent<AudioChorusFilter> ();
					}
					//use first one as reference
					chorusFilter = includedGroup[0].gameObject.GetComponent<AudioChorusFilter>();
				}
				chorusOptions.GetComponent<chorusScript> ().filterReference = chorusFilter;
			}
			chorusOptions.SetActive (true);
		} else {
			if(chorusFilter)
			{
				if(groupName == ""){
					Component.Destroy(soundReference.GetComponent<AudioChorusFilter>());
				}
				else
				{
					foreach (AudioSource a in includedGroup) {
						Component.Destroy (a.gameObject.GetComponent<AudioChorusFilter> ());
					}
				}
				chorusFilter = null;
				chorusOptions.GetComponent<chorusScript> ().filterReference = null;
			}
			chorusOptions.SetActive (false);
		}
	}

	void reverbChanged()
	{
		//Reverb
		if (reverbToggle.isOn) {
			if(!reverbFilter)
			{
				if (groupName == "") {
					reverbFilter = soundReference.AddComponent<AudioReverbFilter> ();

				}
				else{
					foreach (AudioSource a in includedGroup) {
						a.gameObject.AddComponent<AudioReverbFilter> ();
					}
					//use first one as reference
					reverbFilter = includedGroup[0].gameObject.GetComponent<AudioReverbFilter>();
				}
				reverbOptions.GetComponent<reverbScript>().filterReference = reverbFilter;
			}
			reverbOptions.SetActive (true);
		} else {
			if(reverbFilter)
			{
				if(groupName == ""){
					Component.Destroy(soundReference.GetComponent<AudioReverbFilter>());
				}
				else{
					foreach (AudioSource a in includedGroup) {
						Component.Destroy (a.gameObject.GetComponent<AudioReverbFilter> ());
					}
				}
				reverbFilter = null;
				reverbOptions.GetComponent<reverbScript> ().filterReference = null;
			}
			reverbOptions.SetActive (false);
		}
	}

	void distortionChanged()
	{
		//Distortion
		if (distortionToggle.isOn) {
			if(!distortionFilter)
			{
				if (groupName == "") {
					distortionFilter = soundReference.AddComponent<AudioDistortionFilter> ();

				}
				else
				{
					foreach (AudioSource a in includedGroup) {
						a.gameObject.AddComponent<AudioDistortionFilter> ();
					}
					//use first one as reference
					distortionFilter = includedGroup[0].gameObject.GetComponent<AudioDistortionFilter>();
				}
				distortionOptions.GetComponent<distortionScript> ().filterReference = distortionFilter;
			}
			distortionOptions.SetActive (true);
		} else {
			if(distortionFilter)
			{
				if(groupName == ""){
					Component.Destroy(soundReference.GetComponent<AudioDistortionFilter>());
				}
				else{
					foreach (AudioSource a in includedGroup) {
						Component.Destroy (a.gameObject.GetComponent<AudioDistortionFilter> ());
					}
				}
				distortionFilter = null;
				distortionOptions.GetComponent<distortionScript> ().filterReference = null;
			}
			distortionOptions.SetActive (false);
		}
	}

	public Text volumeText;
	void volumeChanged()
	{
		//Apply to one vs all
		if (groupName == "")
			sourceSound.volume = volumeSlider.value;
		else {
			foreach (AudioSource a in includedGroup) {
				a.volume = volumeSlider.value;
			}
		}

		volumeText.text = volumeSlider.value.ToString();
	}
	public Text pitchText;
	void pitchChanged()
	{
		Debug.Log (sourceSound.clip.length);
		//Apply to one vs all
		if (groupName == "")
			sourceSound.pitch = pitchSlider.value;
		else {
			foreach (AudioSource a in includedGroup) {
				a.pitch = pitchSlider.value;
			}
		}
		pitchText.text = pitchSlider.value.ToString();
	}

	//start time changed
	void startTimeChanged()
	{
		//Apply to one vs all
		if(groupName ==""){
			float st;
			if(float.TryParse(startTime.text,out st) && st < soundReference.GetComponent<soundScript> ().endTime){
				//get current start time
				float oldST = soundReference.GetComponent<soundScript> ().startTime;
				//set start time
				soundReference.GetComponent<soundScript> ().startTime = st;
				//adjust visually
				soundReference.transform.localScale = new Vector3(soundReference.GetComponent<soundScript> ().endTime - st,1,1);
				//shift to adjusted amount/2
				soundReference.transform.position = new Vector3(soundReference.transform.position.x-((st-oldST)/2),soundReference.transform.position.y,soundReference.transform.position.z);
			}
			else
				startTime.text = soundReference.GetComponent<soundScript> ().startTime.ToString ();
		}
	}

	//end time changed
	void endTimeChanged()
	{
		//Apply to one vs all
		if (groupName == "") {
			float et;
			if(float.TryParse(endTime.text,out et) && et > soundReference.GetComponent<soundScript>().startTime){
				//get current end time
				float oldET = soundReference.GetComponent<soundScript>().endTime;
				//set end time
				soundReference.GetComponent<soundScript> ().endTime = et;
				soundReference.transform.localScale = new Vector3(et - soundReference.GetComponent<soundScript> ().startTime,1,1);
				//shift to adjusted amount/2
				soundReference.transform.position = new Vector3(soundReference.transform.position.x+((et-oldET)/2),soundReference.transform.position.y,soundReference.transform.position.z);

			}
			else
				endTime.text = soundReference.GetComponent<soundScript> ().endTime.ToString ();

		}
	}

	//Play sound 
	public void playSound()
	{
		sourceSound.Play ();
		soundReference.GetComponent<soundScript>().gameSoundStart = Time.time+soundReference.GetComponent<soundScript>().startTime;
		soundReference.GetComponent<soundScript>().gameSoundEnd = Time.time + sourceSound.clip.length;
	}


}
