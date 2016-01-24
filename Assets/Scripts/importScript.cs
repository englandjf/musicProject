using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;


public class importScript : MonoBehaviour {

	//global vars connection
	public globalVars gv;

	//Auth input
	public InputField authCode;

	//the token used to make calls to free sound
	string accessToken;

	//viewing options
	public GameObject freshStart;
	public GameObject validReady;

	// Use this for initialization
	void Start () {
		//update ui as well
		//check for valid token in file, therefore skip login
		//if invalid, request refresh token
		//or if not present, prompt login and do whole process

		//need something that handles first time login and sets valid ready to true;
	}


	bool UISetup = false;
	// Update is called once per frame
	void Update () {
		//selected
		if (gv.current.name == "importCam" && !UISetup) {
			if (gv.validReady) {
				validReady.SetActive (true);
				freshStart.SetActive (false);
			} else if (gv.firstTimeLoad) {
				validReady.SetActive (false);
				freshStart.SetActive (true);
			}
			UISetup = true;
			
		}
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

	//Login to free sound
	public void loginPressed()
	{
		Application.OpenURL ("https://www.freesound.org/apiv2/oauth2/authorize/?client_id=4656d370ed84017ab3bc&response_type=code");

	}

	//for initil login
	public void verifyCode()
	{
		StartCoroutine (verifyCall ());
	}


	IEnumerator verifyCall()
	{
		
		//Dictionary<string,string> headers = new Dictionary<string, string> ();
		//headers.Add("client_id",);
		//headers.Add("client_seceret",);
		//headers.Add("grant_type","authorization_code");
		//headers.Add("code",);
		WWWForm wwwf = new WWWForm();
		wwwf.AddField("client_id","4656d370ed84017ab3bc");
		wwwf.AddField("client_secret","081dc0326eb35d42dd3e44fda191713b9fcdba63");
		wwwf.AddField("grant_type","authorization_code");
		wwwf.AddField("code",authCode.text);


		WWW www = new WWW ("https://www.freesound.org/apiv2/oauth2/access_token/",wwwf);
		yield return www;
		Debug.Log (www.text);
		if (www.error == null) {
			accessInfo accessStorage = accessInfo.createFromJSON (www.text);
			accessToken = accessStorage.access_token;
			accessStorage.expireTime = System.DateTime.Now.AddDays (1).ToString ();
			writeInfo (accessStorage);

			//change ui and vars
			gv.validReady = true;
			UISetup = false;
		} else {
			//error
		}
		//accessStorage.createFromJSON(www.text);

		//Debug.Log (accessInfo.toJson(accessStorage));
		//strore this information
	}

	[System.Serializable]
	public class accessInfo
	{
		public string access_token;
		public string scope;
		public string expires_in;
		public string refresh_token;
		public string expireTime;

		public static accessInfo createFromJSON(string jsonString)
		{
			return JsonUtility.FromJson<accessInfo> (jsonString);	
		}

		public static string toJson(accessInfo a)
		{
			return JsonUtility.ToJson (a);
		}
	}

	public void writeInfo(accessInfo info)
	{
		//Debug.Log (Application.dataPath);
		StreamWriter a = File.CreateText(Application.dataPath + "/dataFile");
		a.Write(accessInfo.toJson(info));
		a.Close ();
	}
		

	public bool refreshToken(string RT)
	{
		Debug.Log ("Refresh called");
		//error checking
		refreshHelper(RT);
		return true;
		//gv.validReady = true;
	}

	//called if token is expired and login is present
	IEnumerator refreshHelper(string RT)
	{
		WWWForm wwwf = new WWWForm();
		wwwf.AddField("client_id","4656d370ed84017ab3bc");
		wwwf.AddField("client_secret","081dc0326eb35d42dd3e44fda191713b9fcdba63");
		wwwf.AddField("grant_type","refresh_token");
		wwwf.AddField("code",RT);

		WWW www = new WWW ("https://www.freesound.org/apiv2/oauth2/access_token/",wwwf);
		yield return www;
		Debug.Log (www.text);
	}

	//check refresh token when import script is selected for the first time on app launch
	//or check refresh state

	//sound object
	//will have play option and add to project option



}





