using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class reverbScript : MonoBehaviour {

	public AudioReverbFilter filterReference;

	// Use this for initialization
	void Start () {
		
	}

	void setupDropdown()
	{
		//setup hashtable
		nameToReverbPreset = new Hashtable();
		advancedOptions.options = new System.Collections.Generic.List<Dropdown.OptionData> ();
		nameToReverbPreset.Add ("Alley", AudioReverbPreset.Alley);
		advancedOptions.options.Add(new Dropdown.OptionData("Alley"));
		//...mas

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable()
	{
		//Debug.Log ("Enabled");
		if(filterReference)
			loadCurrentValues ();
	}

	void loadCurrentValues(){
	}

	Hashtable nameToReverbPreset;
	public Dropdown advancedOptions;
	public void advancedOptionsChanged()
	{
		//advancedOptions.options [advancedOptions.value];
	}


}
