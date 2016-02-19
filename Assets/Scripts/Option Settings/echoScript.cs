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

	//10 to 5000
	public  Slider delay;
	public Text delayText;
	public void delayChanged()
	{
		filterReference.delay = delay.value;
		delayText.text = delay.value.ToString ();
	}

	//0 to 1
	public Slider decayRatio;
	public Text decayRatioText;
	public void decayRatioChanged()
	{
		decayRatio.value = (float)System.Math.Round (decayRatio.value, 2);
		filterReference.decayRatio = decayRatio.value;
		decayRatioText.text = decayRatio.value.ToString ();

	}

	//0 to 1
	public Slider wetMix;
	public Text wetMixText;
	public void wetMixChanged()
	{
		wetMix.value = (float)System.Math.Round (wetMix.value, 2);
		filterReference.wetMix = wetMix.value;
		wetMixText.text = wetMix.value.ToString ();
	}

	//0 to 1
	public Slider dryMix;
	public Text dryMixText;
	public void dryMixChanged()
	{
		dryMix.value = (float)System.Math.Round (dryMix.value, 2);
		filterReference.dryMix = dryMix.value;
		dryMixText.text = dryMix.value.ToString ();
	}

	//options
}
