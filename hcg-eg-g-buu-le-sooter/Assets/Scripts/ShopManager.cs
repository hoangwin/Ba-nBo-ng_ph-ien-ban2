using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour {

	// Use this for initialization
    public UILabel LabelBoom;
    public UILabel LabelPush;
    public UILabel LabelThrough;
    public static ShopManager instance;
    public UILabel LabelCoin;
    public UILabel LabelTitle;
	void Start () {

        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void init()
    {
        LabelBoom.text = ScoreControl.mBoom.ToString();
        LabelPush.text = ScoreControl.mPush.ToString();
        LabelThrough.text = ScoreControl.mThrough.ToString();
             
    }

    public void BuyBombPress()
    {
        if(ScoreControl.Coin >= 100)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
            ScoreControl.Coin -= 100;

            ScoreControl.mBoom += 1;
            if (ScoreControl.mBoom <= 0)
                ScoreControl.mBoom = 1;
            ScoreControl.saveGame();
            init();
            LabelCoin.text = ScoreControl.Coin.ToString();
        }
    }
    public void BuyPushPress()
    {
        if(ScoreControl.Coin >= 80)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
            ScoreControl.Coin -= 80;

            ScoreControl.mPush += 1;
            if (ScoreControl.mPush <= 0)
                ScoreControl.mPush = 1;
            ScoreControl.saveGame();
            init();
            LabelCoin.text = ScoreControl.Coin.ToString();
        }
    }
    public void BuyThroughPress()
    {
        if (ScoreControl.Coin >= 120)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
            ScoreControl.Coin -= 120;

            ScoreControl.mThrough += 1;
            if (ScoreControl.mThrough <= 0)
                ScoreControl.mThrough = 1;
            ScoreControl.saveGame();
            init();
            LabelCoin.text = ScoreControl.Coin.ToString();
        }
    }

    public void ShopMainmenuBackPress()
    {
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
	        MainMenu.instance.GameButton.SetActive(true);
        MainMenu.instance.GameShop.SetActive(false);
    }
    public void ShopSelectLevelBackPress()
    {
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
        SelectLevelScene.instance.GameButton.SetActive(true);
        SelectLevelScene.instance.GameShop.SetActive(false);
    }
    public void ShopInGameBackPress()
    {
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
        Application.LoadLevel("SelectLevel");

    }
    public void PlayInSelectLevelPress()
    {
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
        GamePlay.currentState = GamePlay.STATE_WATTING;
        Application.LoadLevel("GamePlayScence");
    }
    public void PlayInOverGamePress()
    {
        GamePlay.instance.restart();
    }

}
