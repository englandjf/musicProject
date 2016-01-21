using UnityEngine;
using System.Collections;

public class importScript : MonoBehaviour {

	public globalVars gv;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void backPressed()
	{
		gv.current.enabled = false;
		gv.currentCanvas.enabled = false;
		gv.current = gv.mainCam;
		gv.currentCanvas = gv.mainCanvas;
		gv.current.enabled = true;
		gv.currentCanvas.enabled = true;
	}
}
