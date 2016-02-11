using UnityEngine;
using System.Collections;

public class soundRecord : MonoBehaviour {

	public AudioListener audioOut;
	public AudioSource mainOut;
	bool recordOutput;

	// Use this for initialization
	void Start () {
		recordOutput = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	//play button and check mark?
	//seperate button?
				float [] data;
	public void recordIt()
	{
		
		data = new float[1600];
		AudioListener.GetOutputData (data, 0);
		AudioClip a = new AudioClip ();
		//a.SetData (data, 0);
		//mainOut.clip = a;
		//mainOut.clip = Microphone.Start (null, false, 10, 31000);
		recordOutput = true;
	}

	//called when track is finished
	//call when stop button is pressed
	public void saveIt()
	{
		mainOut.clip = new AudioClip ();
		mainOut.clip.SetData (testData, 0);
		if (recordOutput) {
			mainOut.Play ();		
		}
	}

	float gain = 10;
	float [] testData;
	void OnAudioFilterRead(float[] data, int channels) {
		if (recordOutput){
			for (var i = 0; i < data.Length; ++i) {
				data [i] = data [i] * gain;
				testData = data;
			}
		}
	}
}
