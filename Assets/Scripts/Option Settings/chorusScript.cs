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

	public Slider dryMix;
	public void dryMixChanged()
	{
		filterReference.dryMix = dryMix.value;
	}

	public Slider delay;
	public void delayChanged()
	{
		filterReference.delay = delay.value;
	}

	public Slider rate;
	public void rateChanged()
	{
		filterReference.rate = rate.value;
	}

	public Slider depth;
	public void depthChanged()
	{
		filterReference.depth = depth.value;
	}

	public Slider wetMix1;
	public void wetMix1Changed()
	{
		filterReference.wetMix1 = wetMix1.value;
	}

	public Slider wetMix2;
	public void wetMix2Changed()
	{
		filterReference.wetMix2 = wetMix2.value;
	}

	public Slider wetMix3;
	public void wetMix3Changed()
	{
		filterReference.wetMix3 = wetMix3.value;
	}
}
