using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class soundBank : MonoBehaviour {

	//For now just have one bank
	//But in the future have multiple based on categories

	//List of all sounds, will in future get from files
	public List<AudioClip> allList;
	//Dropdown of all sounds
	public Dropdown soundDropdown;

	// Use this for initialization
	void Start () {
		//allList = new List<AudioClip> ();
		//Clear and add title
		soundDropdown.options = new List<Dropdown.OptionData> ();
		soundDropdown.options.Add(new Dropdown.OptionData("Sounds"));
		setupDropdown ();
	}
	
	// Update is called once per frame
	void Update () {
	
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
