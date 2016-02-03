using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class reverbScript : MonoBehaviour {

	public AudioReverbFilter filterReference;

	// Use this for initialization
	void Start () {
		setupDropdown ();
	}

	void setupDropdown()
	{
		//setup hashtable
		nameToReverbPreset = new Hashtable();
		advancedOptions.options = new System.Collections.Generic.List<Dropdown.OptionData> ();

		nameToReverbPreset.Add (AudioReverbPreset.Alley.ToString(), AudioReverbPreset.Alley);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Alley.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Arena.ToString(), AudioReverbPreset.Arena);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Arena.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Auditorium.ToString(), AudioReverbPreset.Auditorium);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Auditorium.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Bathroom.ToString(), AudioReverbPreset.Bathroom);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Bathroom.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.CarpetedHallway.ToString(), AudioReverbPreset.CarpetedHallway);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.CarpetedHallway.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Cave.ToString(), AudioReverbPreset.Cave);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Cave.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.City.ToString(), AudioReverbPreset.City);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.City.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Concerthall.ToString(), AudioReverbPreset.Concerthall);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Concerthall.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Dizzy.ToString(), AudioReverbPreset.Dizzy);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Dizzy.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Drugged.ToString(), AudioReverbPreset.Drugged);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Drugged.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Forest.ToString(), AudioReverbPreset.Forest);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Forest.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Generic.ToString(), AudioReverbPreset.Generic);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Generic.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Hallway.ToString(), AudioReverbPreset.Hallway);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Hallway.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Hangar.ToString(), AudioReverbPreset.Hangar);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Hangar.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Livingroom.ToString(), AudioReverbPreset.Livingroom);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Livingroom.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Mountains.ToString(), AudioReverbPreset.Mountains);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Mountains.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.PaddedCell.ToString(), AudioReverbPreset.PaddedCell);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.PaddedCell.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.ParkingLot.ToString(), AudioReverbPreset.ParkingLot);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.ParkingLot.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Plain.ToString(), AudioReverbPreset.Plain);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Plain.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Psychotic.ToString(), AudioReverbPreset.Psychotic);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Psychotic.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Quarry.ToString(), AudioReverbPreset.Quarry);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Quarry.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Room.ToString(), AudioReverbPreset.Room);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Room.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.SewerPipe.ToString(), AudioReverbPreset.SewerPipe);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.SewerPipe.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.StoneCorridor.ToString(), AudioReverbPreset.StoneCorridor);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.StoneCorridor.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Stoneroom.ToString(), AudioReverbPreset.Stoneroom);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Stoneroom.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.Underwater.ToString(), AudioReverbPreset.Underwater);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.Underwater.ToString()));

		nameToReverbPreset.Add (AudioReverbPreset.User.ToString(), AudioReverbPreset.User);
		advancedOptions.options.Add(new Dropdown.OptionData(AudioReverbPreset.User.ToString()));


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
		Debug.Log (advancedOptions.options [advancedOptions.value].text);
		filterReference.reverbPreset = (AudioReverbPreset) nameToReverbPreset [advancedOptions.options [advancedOptions.value].text];

		//enable advanced panel
		if (advancedOptions.options [advancedOptions.value].text == "User") {
			Debug.Log ("User open");
		}
	}


}
