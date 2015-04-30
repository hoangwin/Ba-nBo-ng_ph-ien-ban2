using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour {

	public GameObject PanelOverGame;
	public GameObject PanelWin;
    public GameObject PanelShop;
	public GameObject PanelPause;
    public GameObject Gunner;
    public GameObject BubblePre;
    public GameObject BubbleListParentObject;

    public GameObject buttonPush;
    public GameObject buttonBoom;
    public GameObject buttonThrough;



    public Sprite SpriteStar1;
    public Sprite SpriteStar2;
    public Sprite SpriteStar3;
	//public static float speedx = 0.06f ;
	public static GamePlay instance;

	//state
	public static int currentState = 0;
	public static int nextState = 0;
	public const int STATE_WATTING = 0;
    public const int STATE_READY_GO = 1;
	public const int STATE_PLAY = 2;
	public const int STATE_DROP = 3; 
	public const int STATE_OVER = 4;
	public const int STATE_PAUSE = 5;
	public const int STATE_WAITING_WIN_LOSE = 6;
	public const int STATE_WIN = 7;
	public const int STATE_LOSE = 8;
	//end state

	public static float TimePlayedSubState = 0f;
	
	public static float CURRENT_START_Y = -3.1f;
	public static int STATE_INIT = -1;
	public static bool isWin = true;
	public UILabel LabelLevel;
	public UILabel LabelScore;
    public UILabel LabelCoin;

    public UILabel LabelBoom;
    public UILabel LabelPush;
    public UILabel LabelThrough;

    public Effect effectSpecial;
    public Effect effectIce;
    public Effect effectText;
    public Effect effectCombo;
    public Animator GunnerAnim;
    public UI2DSprite starSprite;
    public static int countOk = 0;
    public static int countFail = 0;

    static public float timeShowAds = 0;
    static public bool firstShowAds = false;

    static public float timeVirbrationBubble = 0;
	void Start () {
		DEF.Init();
		instance = this;
		//NGUITools.SetActive(PanelOverGame,false);   
		ScoreControl.Score = 0;
		DEF.ScaleAnchorGui();
        restart();
        setCountTextEffect();
        SoundEngine.getInstance().PlayLoop(SoundEngine.getInstance()._soundBG2);
       
	}	
	// Update is called once per frame
    void Update()
    {
       
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (currentState == STATE_PLAY)
            {
                GamePlay.changeState(GamePlay.STATE_PAUSE);
                GamePlay.instance.PanelPause.SetActive(true);
            }
            else if (currentState == STATE_PAUSE)
            {
                GamePlay.changeState(GamePlay.STATE_PLAY);
                GamePlay.instance.PanelPause.SetActive(false);
            }
            else
            {
                Application.LoadLevel("SelectLevel");
            }
        }
        //		Debug.Log(currentState);
        switch (currentState)
        {
           
            case GamePlay.STATE_PLAY:
                shootBubble();
                BubbleListParent.UpdatekMoveWall();
                break;
           
        }

        if (nextState != currentState && nextState != -1)
        {
            currentState = nextState;
            nextState = -1;
            return;
        }
        timeVirbrationBubble += Time.deltaTime;
        if(timeVirbrationBubble > 6)
        {
            timeVirbrationBubble = 0;
            int random = Random.Range(0, LevelManager.bubbleList.size + 4);
            if (random < LevelManager.bubbleList.size)
                if (LevelManager.bubbleList[random].activeSelf )
                    if (LevelManager.bubbleList[random].GetComponent<Bubble>().state == Bubble.STATE_BUBBLE_IDE)
                        LevelManager.bubbleList[random].GetComponent<Bubble>().PlayAnimVirbration();
            
        }
    }

    public void setCountTextEffect()
    {
        LabelBoom.text = ScoreControl.mBoom.ToString();
        LabelPush.text = ScoreControl.mPush.ToString(); ;
        LabelThrough.text = ScoreControl.mThrough.ToString();
    }

	public static void changeState(int State)
	{
		nextState = State;
	}
	void FixedUpdate()
	{
        TimePlayedSubState += Time.deltaTime;
        timeShowAds += Time.deltaTime;
        switch (GamePlay.currentState)
        {

            case GamePlay.STATE_WATTING:
                LevelManager.UpdateVisibleTable();
                if (BubbleListParent.lastObject == null)
                {
                }
                else
                {
                    float y = BubbleListParent.lastObject.transform.position.y;
                    LevelManager.BEGIN_Y = y + BubbleListParent.lastObject.GetComponent<Bubble>().indexY * (LevelManager.WIDTH + LevelManager.HIGHT_OFFSET_Y);
                }
              //LevelManager.BEGIN_Y =BubbleListParent.GetLastBubbleBottomY();
                break;
            case GamePlay.STATE_READY_GO:
                if (TimePlayedSubState < 1.2)
                    break;
                TimePlayedSubState = 0;
                changeState(STATE_PLAY);
                buttonBoom.SetActive(true);
                buttonPush.SetActive(true);
                buttonThrough.SetActive(true);
                break;
            case GamePlay.STATE_PLAY:
                LevelManager.UpdateVisibleTable();
                break;
            case GamePlay.STATE_WAITING_WIN_LOSE:
                if (TimePlayedSubState < 1)
                    break;
                TimePlayedSubState = 0f;
                
                ShowADS();

                if (isWin)
                {   
                    PanelWin.SetActive(true);
                    GameObject.Find("LabelScoreWin").GetComponent<UILabel>().text = ScoreControl.Score.ToString();
                    if (ScoreControl.mUnblockLevel == LevelManager.currentLevel)
                    {
                        ScoreControl.mUnblockLevel++;
                    }

                    //kiem tra star
                    int t = 1;
                    if (LevelManager.countbubbleShoot > 0)
                        t = LevelManager.countAllBubble / LevelManager.countbubbleShoot;
                    int star = 0;
                    if (t >= 7)
                    {
                        star = 3;
                        starSprite.sprite2D = SpriteStar3;
                    }
                    else if (t >= 5)
                    {
                        star = 2;
                        starSprite.sprite2D = SpriteStar2;
                    }
                    else
                    {
                        star = 1;
                        starSprite.sprite2D = SpriteStar1;
                    }
                    if (LevelManager.currentLevel < 1)
                        LevelManager.currentLevel = 1;
                    if (ScoreControl.starArray[LevelManager.currentLevel - 1] < star)
                        ScoreControl.starArray[LevelManager.currentLevel - 1] = star;

                    ScoreControl.saveGame();
                    GamePlay.currentState = STATE_WIN;
                }
                else
                {
                    PanelOverGame.SetActive(true);
                    GamePlay.currentState = STATE_LOSE;
                }
                break;
        }
	}

	public void shootBubble()
	{      
           
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0) || Input.GetMouseButton(0)||
            ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            Vector2 fingerPos = new Vector2(0, 0);
            if (Input.touchCount == 1)
            {
                fingerPos = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                fingerPos = Input.mousePosition;
            }
            fingerPos = Camera.main.ScreenToWorldPoint(fingerPos);

            if (fingerPos.y > 4.1 || fingerPos.y < -2.5)
                return;


            fingerPos.y = fingerPos.y - CURRENT_START_Y;
            //Debug.Log("rrr:"+fingerPos.x+","+fingerPos.y);
            float Max = 15f;

            if (Mathf.Abs(fingerPos.x) > Mathf.Abs(fingerPos.y))
            {
                fingerPos.y = Mathf.Abs(fingerPos.y / fingerPos.x * Max);
                fingerPos.x = fingerPos.x / Mathf.Abs(fingerPos.x) * Max;
            }
            else
            {
                fingerPos.x = fingerPos.x / Mathf.Abs(fingerPos.y) * Max;
                fingerPos.y = Max;
            }

          
            float angle = -Mathf.Atan2(fingerPos.x, fingerPos.y) * 180 / Mathf.PI;  
            Gunner.transform.eulerAngles = new Vector3(0, 0, angle);
            LevelManager.currentBubbleWaiting.transform.eulerAngles = new Vector3(0, 0, angle);
            if (LevelManager.currentBubbleWaiting.GetComponent<Bubble>().state == Bubble.STATE_BUBBLE_WATING_SHOOT )
            {
                if (LevelManager.currentBubble == null || (LevelManager.currentBubble != null && LevelManager.currentBubble.GetComponent<Bubble>().state != Bubble.STATE_BUBBLE_SHOOT))
                {
                    if (Input.GetMouseButtonUp(0) || ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Ended)))
                    {
                        LevelManager.currentBubble = LevelManager.currentBubbleWaiting;
                        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundShoot);
                        LevelManager.countbubbleShoot++;
                        GunnerAnim.Play("GUNNER_SHOOT");
                        LevelManager.currentBubble.rigidbody2D.velocity = fingerPos;//
                        LevelManager.currentBubble.GetComponent<Bubble>().currentvelocity = fingerPos;//
                        LevelManager.currentBubble.transform.eulerAngles = new Vector3(0, 0, 0);
                        LevelManager.currentBubble.GetComponent<Bubble>().state = Bubble.STATE_BUBBLE_SHOOT;
                        LevelManager.currentBubble.GetComponent<Bubble>().spriteRenderer.sortingOrder = -4;
                        LevelManager.createNewBubble();
                    }
                }
            }
        }
	
	}

	public void restart() 
	{

        countOk = 0;
        countFail = 0;
        PanelPause.SetActive(false);
        PanelWin.SetActive(false);
        PanelShop.SetActive(false);
        PanelOverGame.SetActive(false);
        GamePlay.instance.LabelCoin.text = ScoreControl.Coin.ToString();
        isWin = true;
        currentState = STATE_WATTING;
        LevelManager.isMoveWall = false;
            //changeState(STATE_WATTING);
        BubblePre.SetActive(false);
        LevelManager.destroyTable();
        if (LevelManager.currentLevel > 118)
            LevelManager.currentLevel = 1;
		LevelManager.getLevel2(LevelManager.currentLevel);
		
		ScoreControl.Score =0;
		LabelLevel.text = LevelManager.currentLevel.ToString();
		LabelScore.text ="0";
        GameObject.Destroy(LevelManager.currentBubble);
        GameObject.Destroy(LevelManager.currentBubbleWaiting);
        //LevelManager.creatNewBubble();
        //LevelManager.BEGIN_Y = LevelManager.BEGIN_Y_DEFAULT - 15 * LevelManager.WIDTH;
        GamePlay.instance.BubbleListParentObject.transform.position = new Vector3(0, -20 * LevelManager.WIDTH);   


       
        GameObject.Find("WallBottom").layer = 12;
        Physics2D.IgnoreLayerCollision(12, 12, true);
        BubblePre.layer = 12;
        BubblePre.SetActive(false);
        GameObject obj = GameObject.Find("AnimGunner");
        GunnerAnim = obj.GetComponent<Animator>();
        LevelManager.speedOffSet = 0f;
       // iTween.Stop();
        //iTween.MoveTo(GamePlay.instance.BubbleListParentObject, iTween.Hash("y",(LevelManager.MAX_ROW -17)* LevelManager.WIDTH, "easeType", "linear", "delay", 0.0, "time", LevelManager.MAX_ROW * 0.1, "oncomplete", "moveBeginCOmpleted"));
        buttonBoom.SetActive(false);
        buttonPush.SetActive(false);
        buttonThrough.SetActive(false);

        LTDescr d = LeanTween.moveY(GamePlay.instance.BubbleListParentObject, (LevelManager.MAX_ROW - 17) * LevelManager.WIDTH, LevelManager.MAX_ROW * 0.1f);

        d.setOnComplete(onMoveBeginCompleted).setEase(LeanTweenType.notUsed);
        //LeanTween.reset();
        //GamePlay.instance.BubbleListParentObject.transform.position = new Vector3(0, (LevelManager.MAX_ROW - 16) * LevelManager.WIDTH, 0);
        //BubbleListParent.instance.moveBeginCOmpleted();
	}
    void onMoveBeginCompleted()
    {
        BubbleListParent.instance.moveBeginCOmpleted();

    }
    public static void ShowADS()
    {
        if (timeShowAds > 120 || !firstShowAds)
        {
            firstShowAds = true;
            timeShowAds = 0;
#if UNITY_ANDROID
            using (AndroidJavaClass jc = new AndroidJavaClass("com.banbong.bantrung.bantrungkhunglong.UnityPlayerNativeActivity"))
            {
                jc.CallStatic<int>("ShowAds");
            }
            
#elif UNITY_WP8
         WP8Statics.ShowAds("");
#elif UNITY_IOS
            IOsStatic.ShowAds(" ", " ");
#endif
        }
        

    }

	
}
