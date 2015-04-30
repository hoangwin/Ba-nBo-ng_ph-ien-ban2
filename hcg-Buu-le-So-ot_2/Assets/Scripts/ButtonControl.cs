using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour 
{


    public UI2DSprite sfx;
    public UI2DSprite music;
    public UIButton sfxUiButton;
    public UIButton MusicUiButton;
    public Sprite sfx1Sprite;
    public Sprite sfx2Sprite;
    public Sprite music1Sprite;
    public Sprite music2Sprite;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void PuzzleButtonPress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
		Application.LoadLevel("SelectLevel");
	
	}
	public void HelpButtonPress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
		//Application.LoadLevel("GamePlayScence");
        Application.LoadLevel("Help");
	
	}
    public void ExitButtonPress()
    {
        Application.Quit();
    }
    public void BackButtonPress()
    {

        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
        Application.LoadLevel("MainMenu");
    }

	public void PlayInGamePress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
		GamePlay.changeState (GamePlay.STATE_PLAY);
		GamePlay.instance.PanelPause.SetActive(false);

	}

	public void RestartInGamePress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
		GamePlay.instance.restart();
		//Application.LoadLevel("GamePlayScence");
		//Debug.Log ("aaaaaaaaaa");
	}
	public void ButtonRatePress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
#if UNITY_ANDROID
            Application.OpenURL("market://details?id=com.bubbleshoot2.free");	
            
#elif UNITY_WP8
        WP8Statics.RateApp("");
#elif UNITY_IOS
        Application.OpenURL("https://itunes.apple.com/us/app/bubble-shoot-free/id914220826?ls=1&mt=8");	
        
         //   IOsStatic.ShowAds(" ", " ");
#endif
        	
	}
	public void RankingButtonPress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
		Application.LoadLevel("Ranking");
	}
	public void InputOkButtonPress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
		string str = GameObject.Find ("LabelInputName").GetComponent<UILabel> ().text;
		str = str.Trim();
		str = str.Replace("'","_");
		str = str.Replace("\"","_");
		str = str.Replace(" ","_");
		str =str.Replace("=","_");
		GameObject.Find ("LabelInputName").GetComponent<UILabel> ().text = str;
		if (str.Length >= 5) {
			ScoreControl.UserName = str;
			ScoreControl.saveGame();
			NGUITools.SetActive(Raking.instance.PanelInputName,false);  
			NGUITools.SetActive(Raking.instance.PanelBoard,true);  
			Raking.instance.PostHightScore ();
			Raking.instance.getHightScore ();
			Raking.loadRanking = true;				
		}
	}

	public void ButtonSoundFXPress()
	{
		SoundEngine.isSoundSFX= !SoundEngine.isSoundSFX;
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
        if (SoundEngine.isSoundSFX)
        {
            sfxUiButton.normalSprite2D = sfx1Sprite;
        }
        else
        {
          
            sfxUiButton.normalSprite2D = sfx2Sprite;          
        }

	}
    public void ButtonSoundMusicPress()
    {
        //SoundEngine.isSound = !SoundEngine.isSound;
        SoundEngine.isSoundMusic = !SoundEngine.isSoundMusic;
        if (SoundEngine.isSoundMusic)
        {
            MusicUiButton.normalSprite2D = music1Sprite;
            SoundEngine.getInstance().PlayLoop(SoundEngine.getInstance()._soundBG1);

        }
            
        else
        {
            MusicUiButton.normalSprite2D = music2Sprite;
            SoundEngine.getInstance().stopSound();
        }


    }
	public void IGMButtonPress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
        if (GamePlay.currentState == GamePlay.STATE_PLAY)
        {
            GamePlay.changeState(GamePlay.STATE_PAUSE);
            GamePlay.instance.PanelPause.SetActive(true);
        }
	}

	public void ReplayButtonPress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
		GamePlay.instance.restart();
	}

	public void MainMenuButtonPress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
		Application.LoadLevel ("MainMenu");
	}
	public void NextButtonPress()
	{
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
        GamePlay.instance.PanelWin.SetActive(false);
        GamePlay.instance.PanelShop.SetActive(true);
		LevelManager.currentLevel++;
        ShopManager.instance = GamePlay.instance.PanelShop.GetComponent<ShopManager>();
        ShopManager.instance.LabelTitle.text = "Level " + LevelManager.currentLevel.ToString();
        ShopManager.instance.init();
        //GamePlay.instance.setCountTextEffect();
       // GamePlay.instance.restart();
	}   
    public void PushButtonPress()
    {
        //here
        if (ScoreControl.mPush > 0 && GamePlay.currentState == GamePlay.STATE_PLAY)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundPush);
            Debug.Log("here");
            GamePlay.instance.effectSpecial.gameObject.transform.position = new Vector3(0, 0, 0);// transform.position;
            GamePlay.instance.effectSpecial.anim.Play("Effect_PUSH");
            LevelManager.speedOffSet = LevelManager.SPEED_OFFSET_UP * 3;
            BubbleListParent.speedY = LevelManager.speedOffSet;
            LevelManager.moveOffSet = 0;
            LevelManager.MOVE_OFFSET_UP = 2f;
            LevelManager.isMoveWall = true;
            ScoreControl.mPush--;
            ScoreControl.saveGame();
            GamePlay.instance.setCountTextEffect();
        }
        
        
    }
    public void BoomThroughButtonPress()
    {
        //here
        if (ScoreControl.mThrough > 0 && GamePlay.currentState == GamePlay.STATE_PLAY && LevelManager.currentBubbleWaiting != null )
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundBone);
            LevelManager.currentBubbleWaiting.collider2D.isTrigger = true;
            LevelManager.currentBubbleWaiting.GetComponent<Bubble>().setValue(BubbleType.BUBBLE_TYPE_SHOOT_BOOM_ROAD);
            ScoreControl.mThrough--;
            ScoreControl.saveGame();
            GamePlay.instance.setCountTextEffect();
        }
        
    }
    public void BoomButtonPress()
    {
        //here
        if (ScoreControl.mBoom > 0 && GamePlay.currentState == GamePlay.STATE_PLAY && LevelManager.currentBubbleWaiting != null)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundBone);
            //LevelManager.currentBubble.collider2D.isTrigger = true;
            LevelManager.currentBubbleWaiting.GetComponent<Bubble>().setValue(BubbleType.BUBBLE_TYPE_SHOOT_BOOM_CICLE);
            ScoreControl.mBoom--;
            ScoreControl.saveGame();
            GamePlay.instance.setCountTextEffect();
        }
    }

    public void CoinButtonPress()
    {
        
    }
    ///
    public void ShopMainmenuPress()
    {
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
        MainMenu.instance.GameButton.SetActive(false);
        MainMenu.instance.GameShop.SetActive(true);
        if (ShopManager.instance == null)
            ShopManager.instance = MainMenu.instance.GameShop.GetComponent<ShopManager>();
        ShopManager.instance.init();
    }




    
}
