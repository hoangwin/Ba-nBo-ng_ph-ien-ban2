using UnityEngine;
using System.Collections;

public class Raking : MonoBehaviour {
	public WWW www;
	public static bool loadRanking = true;
	public static bool isMoveFromMainmenu = false;
	public GameObject[] listUserObject = new GameObject[11];
	public GameObject PanelBoard;
	public GameObject PanelInputName;
	public static Raking instance;
	void Start () {
		DEF.Init();
		PanelBoard = GameObject.Find("PanelBoard");
		PanelInputName = GameObject.Find("PanelInputName");		
		if (ScoreControl.UserName.Length >= 5)
		{
			PostHightScore ();
			getHightScore ();
			loadRanking = true;
			NGUITools.SetActive(PanelInputName,false);  
		} 
		else
		{
			loadRanking = false;
			NGUITools.SetActive(PanelBoard,false);  

		}
		DEF.ScaleAnchorGui();

		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("MainMenu");
			
		}
		getHightScoreDone();
	}
	
	public void  getHightScore()
	{
		www = new WWW("http://gamethuanviet.com/flappybird/SetGetData.php?type=select&username="+ ScoreControl.UserName);
		Debug.Log("http://gamethuanviet.com/flappybird/SetGetData.php?type=select&username=" + ScoreControl.UserName);
		
	}
	public void  getHightScoreDone()
	{
		if(loadRanking && www.isDone)
		{
			if(www.bytesDownloaded <20)
				return;
				

			loadRanking = false;
			Debug.Log(www.text);
			string[] strarray;
			strarray = www.text.Split('|');
			for (int i = 0; i < 10; i++)
			{
				GameObject.Find(("Name" +(i+1))).GetComponent<UILabel>().text = strarray[i * 3 + 1];
				GameObject.Find(("Score" +(i+1))).GetComponent<UILabel>().text = strarray[i * 3 + 2];
			//	listUserObject[i] = GameObject.Find("User"+ (i+1));
			//	listUserObject[i].transform.Find("Label2Name").GetComponent<UILabel>().text = strarray[i * 3 + 1];
			//	listUserObject[i].transform.Find("Label3Money").GetComponent<UILabel>().text = strarray[i * 3 + 2];			
			}
			if (strarray.Length > 33)
			{
				GameObject.Find("MyPos" ).GetComponent<UILabel>().text = strarray[10 * 3 + 1];
				GameObject.Find("MyName").GetComponent<UILabel>().text = strarray[10 * 3 + 2];
				GameObject.Find("MyScore").GetComponent<UILabel>().text = strarray[10 * 3 + 3];
			//	listUserObject[10].transform.Find("Label2Pos").GetComponent<UILabel>().text= strarray[10 * 3 + 1];
			//	listUserObject[10].transform.Find("Label2Name").GetComponent<UILabel>().text = strarray[10 * 3 + 2];
			//	listUserObject[10].transform.Find("Label3Money").GetComponent<UILabel>().text = strarray[10 * 3 + 3];
			}
		}
	}
	public void PostHightScore()
	{
		////http://gamethuanviet.com/baucuatomca/SetGetData.php?type=update&username=%s&Score=%d&Level=0&Played=0&country=NA "
		//string strPost = "http://gamethuanviet.com/flappybird/SetGetData.php?type=update&username=" + ScoreControl.UserName + "&Score=" +ScoreControl.getRealBestScore() +"&Level=0&Played=0&country=NA";
		//Debug.Log(strPost);
		//WWW www = new WWW(strPost);
	}
	// Use this for initialization

}
