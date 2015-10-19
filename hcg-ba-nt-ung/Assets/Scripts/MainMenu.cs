using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	// Use this for initialization

    public UILabel LabelScore;
    public UILabel LabelCoin;
	public static MainMenu instance;
    public GameObject GameButton;
    public GameObject GameShop;
	void Start () {
        
		DEF.Init ();
		DEF.ScaleAnchorGui();
		ScoreControl.loadGame();
		instance = this;

        LabelCoin.text = ScoreControl.Coin.ToString();
        LabelScore.text = ScoreControl.Score.ToString();

        ShopManager.instance = GameShop.GetComponent<ShopManager>();

        SoundEngine.getInstance().PlayLoop(SoundEngine.getInstance()._soundBG1);
        //Application.targetFrameRate = 60;
      //  GamePlay.ShowADS();
	}	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
            if (GameShop.activeSelf == true)
            {
                GameButton.SetActive(true);
                GameShop.SetActive(false);
            }
            else
            {
			    Application.Quit();
            }
		}

	}
	
}
