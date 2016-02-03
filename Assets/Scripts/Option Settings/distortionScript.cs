using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class distortionScript : MonoBehaviour {

	public AudioDistortionFilter filterReference;

	// Use this for initialization
	void Start () {
	
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

	void loadCurrentValues()
	{
		distortionLevel.value = filterReference.distortionLevel;
	}

	public Slider distortionLevel;
	public void distortionLevelChanged()
	{
		filterReference.distortionLevel = distortionLevel.value;
	}


}
