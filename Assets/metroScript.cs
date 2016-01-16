using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class metroScript : MonoBehaviour {

	AudioSource click;
	globalVars gv;

	// Use this for initialization
	void Start () {
		click = GetComponent<AudioSource> ();
		gv = GameObject.Find ("main").GetComponent<globalVars> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D a)
	{
		//Debug.Log ("Hit");
		if (a.gameObject.name == "bar" && click && gv.clickToggle.isOn)
			click.Play ();
	}
}
