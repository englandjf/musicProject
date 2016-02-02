using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class echoScript : MonoBehaviour {

	public AudioEchoFilter filterReference;

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
		delay.value = filterReference.delay;
		decayRatio.value = filterReference.decayRatio;
		wetMix.value = filterReference.wetMix;
		dryMix.value = filterReference.dryMix;
	}

	public  Slider delay;
	public void delayChanged()
	{
		filterReference.delay = delay.value;
	}

	public Slider decayRatio;
	public void decayRatioChanged()
	{
		filterReference.decayRatio = decayRatio.value;

	}

	public Slider wetMix;
	public void wetMixChanged()
	{
		filterReference.wetMix = wetMix.value;
	}

	public Slider dryMix;
	public void dryMixChanged()
	{
		filterReference.dryMix = dryMix.value;
	}

	//options
}
