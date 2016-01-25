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
	//string accessToken;

	//viewing options
	public GameObject freshStart;
	public GameObject validReady;
	public GameObject searchUI;
	public Text[] searchTexts;//->may change to game objects to include more options, play button 

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
			gv.accessToken = accessStorage.access_token;
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

		//rewrite file
		accessInfo accessStorage = accessInfo.createFromJSON (www.text);
		gv.accessToken = accessStorage.access_token;
		accessStorage.expireTime = System.DateTime.Now.AddDays (1).ToString ();
		writeInfo (accessStorage);
		Debug.Log (www.text);
	}

	//check refresh token when import script is selected for the first time on app launch
	//or check refresh state

	//sound object
	//will have play option and add to project option

	public InputField searchInput;

	public void searchSounds()
	{
		string baseUrl = "http://www.freesound.org/apiv2/search/text/";
		string query1 = "?query=" + searchInput.text + "&";
		string typeRequired = "filter=type:wav"+"&";
		string apiKey = "token=081dc0326eb35d42dd3e44fda191713b9fcdba63";
		string wholeUrl = baseUrl + query1 + typeRequired+ apiKey;
		Debug.Log(wholeUrl);
		StartCoroutine(startSearch(wholeUrl));
	}
		


	//will give you one page, add page = 2, etc for others
	IEnumerator startSearch(string wholeURL)
	{

		WWW www = new WWW(wholeURL);
		yield return www;
		searchResultParent sRP = searchResultParent.createFromJSON(www.text);
		displayResults(sRP);
		//parseResults(sRP);
		Debug.Log(www.text);
	}

	//list of results
	//public searchResult[] resultList;
	//creates list of results
	/*
	void parseResults(searchResultParent sRP)
	{
		//15 results returned per query
		Debug.Log(sRP.results[0].name);

		//resultList = new searchResult[15];
		for(int i = 0; i < sRP.results.Length;i++)
		{
			Debug.Log(sRP.results[i].name);
			//searchResult srTemp = searchResult.createFromJSON(sRP.results[i]);
			//resultList[i] = srTemp;
			//Debug.Log(srTemp.name);
		}


	}
	*/

	void displayResults(searchResultParent sRP)
	{
		
		for(int i = 0; i < sRP.results.Length;i++)
		{
			searchTexts[i].text = sRP.results[i].name;
		}
		searchUI.SetActive(true);
		sRPCopy = sRP;
	}
	//save copy(reference)
	private searchResultParent sRPCopy;

	//play sound at index, must download sound first
	public void playSound(int index)
	{
		//get id, downloand
		//Debug.Log();
		StartCoroutine(soundHelper(sRPCopy.results[index].id));

	}

	//used to play sounds 
	public AudioSource playSource;
	IEnumerator soundHelper(string id)
	{
		Dictionary <string,string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", "Bearer " + gv.accessToken);
		string wholeUrl = "https://www.freesound.org/apiv2/sounds/" + id+"/download/";
		WWW www = new WWW(wholeUrl,null,headers);
		Debug.Log("attempting to play " + www.url);
		yield return www;
		if(www.error == null)
			Debug.Log("done");
		else
			Debug.Log(www.error);
		Debug.Log("done");
		//AudioClip loadedClip = www.GetAudioClip(false,false,AudioType.WAV);

		playSource.clip = www.GetAudioClipCompressed(false,AudioType.WAV);
		playSource.Play();

	}



	//searchResults
	[System.Serializable]
	public class searchResultParent
	{
		public string count;
		public string next;
		public string previous;
		public searchResult[]  results;

		public static searchResultParent createFromJSON(string jsonString)
		{
			return JsonUtility.FromJson<searchResultParent> (jsonString);	
		}
	}

	//invdividual result
	[System.Serializable]
	public class searchResult
	{
		public string id;
		public string name;
		public string [] tags;
		public string license;
		public string username;

		public static searchResult createFromJSON(string jsonString)
		{
			return JsonUtility.FromJson<searchResult> (jsonString);	
		}
	}


	//for multiple text query
	public void parseSearch()
	{



	}


}





