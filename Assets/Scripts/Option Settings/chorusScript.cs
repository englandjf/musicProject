using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class chorusScript : MonoBehaviour {

	//for each, we need to apply to changed effect to all sounds, maybe do it when back is pressed

	public AudioChorusFilter filterReference;

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
		dryMix.value = filterReference.dryMix;
		delay.value = filterReference.delay;
		rate.value = filterReference.rate;
		depth.value = filterReference.depth;
		wetMix1.value = filterReference.wetMix1;
		wetMix2.value = filterReference.wetMix2;
		wetMix3.value = filterReference.wetMix3;
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

	//.1 to 100
	public Slider delay;
	public Text delayText;
	public void delayChanged()
	{
		delay.value = (float)System.Math.Round (delay.value, 1);
		filterReference.delay = delay.value;
		delayText.text = delay.value.ToString ();
	}

	public Slider rate;
	public Text rateText;
	public void rateChanged()
	{
		//rate.value = (float)System.Math.Round (rate.value, 2);
		filterReference.rate = rate.value;
		rateText.text = rate.value.ToString ();
	}

	//0 to 1
	public Slider depth;
	public Text depthText;
	public void depthChanged()
	{
		depth.value = (float)System.Math.Round (depth.value, 2);
		filterReference.depth = depth.value;
		depthText.text = depth.value.ToString ();
	}

	//0 to 1
	public Slider wetMix1;
	public Text wetMix1Text;
	public void wetMix1Changed()
	{
		wetMix1.value = (float)System.Math.Round (wetMix1.value, 2);
		filterReference.wetMix1 = wetMix1.value;
		wetMix1Text.text = wetMix1.value.ToString ();
	}

	//0 to 1
	public Slider wetMix2;
	public Text wetMix2Text;
	public void wetMix2Changed()
	{
		wetMix2.value = (float)System.Math.Round (wetMix2.value, 2);
		filterReference.wetMix2 = wetMix2.value;
		wetMix2Text.text = wetMix2.value.ToString ();
	}

	//0 to 1
	public Slider wetMix3;
	public Text wetMix3Text;
	public void wetMix3Changed()
	{
		wetMix3.value = (float)System.Math.Round (wetMix3.value, 2);
		filterReference.wetMix3 = wetMix3.value;
		wetMix3Text.text = wetMix3.value.ToString ();
	}
}
