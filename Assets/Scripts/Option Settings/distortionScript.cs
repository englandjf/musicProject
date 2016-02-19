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
	public Text distortionLevelText;
	public void distortionLevelChanged()
	{
		distortionLevel.value = (float)System.Math.Round (distortionLevel.value, 2);
		filterReference.distortionLevel = distortionLevel.value;
		distortionLevelText.text = distortionLevel.value.ToString ();
	}


}
