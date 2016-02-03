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
		nameToReverbPreset.Add ("Alley", AudioReverbPreset.Alley);
		//...mas

		advancedOptions.options = new System.Collections.Generic.List<Dropdown.OptionData> ();
		//advancedOptions.options.Add(
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
