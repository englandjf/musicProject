using UnityEngine;
using System.Collections;

public class slideScript : MonoBehaviour {

	bool left;
	bool right;
	bool over;
	public globalVars gv;

	// Use this for initialization
	void Start () {
		if(this.gameObject.name == "rightSlide")
			right = true;
		else
			left = true;
	}

	// Update is called once per frame
	void Update () {
		if(over && right && Input.GetMouseButton(0))
			gv.mainCam.transform.position = new Vector3 (gv.mainCam.transform.position.x + .05f,gv.mainCam.transform.position.y, gv.mainCam.transform.position.z);
		else if(over && left && Input.GetMouseButton(0))
			gv.mainCam.transform.position = new Vector3 (gv.mainCam.transform.position.x - .05f,gv.mainCam.transform.position.y, gv.mainCam.transform.position.z);
		
	}

	void OnMouseOver()
	{
		over = true;
	}
	void OnMouseExit()
	{
		over = false;
	}
}
