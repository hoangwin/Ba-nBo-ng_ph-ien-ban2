 using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {
	public int indexX; 
	public int indexY;
	public float fixPosX; 
	public float fixPosY;
	public float currentPosX; 
	public float currentPosY;
	public Vector2 currentvelocity;
    public const int STATE_BUBBLE_WATING_SHOOT = 0;
    public const int STATE_BUBBLE_SHOOT = 1;    
    public const int STATE_BUBBLE_IDE = 2;
    public const int STATE_BUBBLE_DROP = 3;
    public const int STATE_BUBBLE_WAITTING_DESTROY = 4;
    public const int STATE_BUBBLE_DESTROY = 5;
    public int state = 0;
	
    public int value;
    public int effect;
    public const int EFFECT_NONE = 0;
    public const int EFFECT_BONE = 1;
    public const int EFFECT_BONE_REPLACE = 2;
    public const int EFFECT_BONE_NEW_VALUE = 3;// dau hoi. hien thi ra thang moi
	public bool isCheck = false;
    public int depth = -1;// dung de check xem so dang cach bong can xet bao nhieu qua bong
    public static bool isPlaySpecial = false;
    
   // public bool isShow = true;

	public Animator anim;// = gameObject.GetComponent<Animator>();    
    public SpriteRenderer spriteRenderer;// = gameObject.GetComponent<Animator>();
    
    public bool isMoveThrough;
    public static float lastTimePlaySoundDestroy;
	// Use this for initialization
    public static int countAnimScore = 0;
	public void setValue(int _value)
	{

     //   anim.run = null;
        isMoveThrough = false;
        GetComponent<Collider2D>().isTrigger = false;
		value = _value;		


       // anim.Play("IDE" + value.ToString());
        

        if(value >=9 && value <=16)
        {
            effect = EFFECT_BONE;
            value = value - 8;
        }
        else if (value >= 17 && value <= 24)
        {
            effect = EFFECT_BONE_REPLACE;
            value = value - 16;
        }
        else if ((value > 32 && value <= 40) || (value >= 70 && value <= 1201))//key
        {
            isMoveThrough = true;
            GetComponent<Collider2D>().isTrigger = true;
            // this.gameObject.layer = 12;// de co the xuyen
            // Debug.Log("Bubble Value :" + LevelManager.bubbleListStillAliveIndex.Count);
        }
        else if (value == 41 || value == 30)//key va bom xuyen
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
        RePlayAnim();
        //spriteRenderer.sprite = BubbleListParent.instance.spritesBubble[_value];
        //anim.Play("IDE");
	}

    public void RePlayAnim()
    {
        int temp = value;
        if ( effect == EFFECT_BONE)
        {
            if (temp <=8) 
                temp = value + 8;
        }
        else if (effect == EFFECT_BONE_REPLACE)
        {
            effect = EFFECT_BONE_REPLACE;
            if (temp <= 8) 
                temp = value + 16;
        }    
        //set trigger
        if ((value > 32 && value <= 41) || value ==37 || (value >=70 && value <= 1201))//key bom
            GetComponent<Collider2D>().isTrigger = true;
       // anim.Play("IDE" + temp.ToString());       
        if (temp > 100)
            temp = 100 + temp / 100;
        
        spriteRenderer.sprite = BubbleListParent.instance.spritesBubble[temp];
        anim.Play("IDE");

    }
    public void PlayAnimBONE()
    {        
        spriteRenderer.sprite = BubbleListParent.instance.spritesBubble[value];
        anim.Play("BONE");
    }
    public void PlayAnimVirbration()
    {
        spriteRenderer.sprite = BubbleListParent.instance.spritesBubble[value];
        anim.Play("VIBRATION");
    }
	public void setPos()
	{
		fixPosX = LevelManager.getposX(indexX,indexY);
		fixPosY = LevelManager.getposY(indexX,indexY);
		transform.position = new Vector3(fixPosX,fixPosY ,1);
	}

	void Start () {		
      
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//if(state == 2)//drop
		//	Debug.Log ("u:" + transform.localPosition.y);


        if (GamePlay.currentState != GamePlay.STATE_WATTING)
        {
            
            if (transform.position.y < -3.55f && GamePlay.currentState != GamePlay.STATE_WATTING)
            {
                if (state == STATE_BUBBLE_DROP)
                {
                    this.destroyAfterAnim();
                    if ((Time.time > lastTimePlaySoundDestroy + 0.05f) || (lastTimePlaySoundDestroy > Time.time))
                    {
                        lastTimePlaySoundDestroy = Time.time;
                        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundDestroy);                  
                        lastTimePlaySoundDestroy = Time.time;
                    }
                    
                }
                
                //  SoundEngine.playSound("SoundCoin");			
            }
            else if (state == STATE_BUBBLE_SHOOT && transform.position.y > 4.7f)
            {
                state = STATE_BUBBLE_DESTROY;
                GamePlay.Destroy(this.gameObject);
                LevelManager.FindAllBubbleDrop();
                LevelManager.getBubbleListStillAliveIndex();                
              //  LevelManager.createNewBubble();              
            }
            
        } 
	}

	public GameObject getChildGameObject(string withName) {
		//Author: Isaac Dart, June-13.
		Transform pTransform =GetComponent<Transform>();
		
		foreach (Transform trs in pTransform) {
			
			if (trs.gameObject.name == withName)
				
				return trs.gameObject;
			
		}
		
		return null;
	}
    bool coliderWithWall(string name)
    {
        if (name.Equals("Wall1") || name.Equals("Wall2"))
        {
            if (state != STATE_BUBBLE_IDE)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(currentvelocity.x * (-1f), currentvelocity.y);
                currentvelocity = GetComponent<Rigidbody2D>().velocity;
            }
            else
            {
                setPos();
            }
            return true;
        }
        return false;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
      //  Debug.Log(collision.name);       
        if (! coliderWithWall(collision.gameObject.name) && collision.tag.Equals("Bubble"))
        {
            if (collision.gameObject.GetComponent<Bubble>().state == STATE_BUBBLE_SHOOT)
            {
                if (isMoveThrough)
                {
                    destroyAfterAnim();
                    LevelManager.bubbleList.Remove(this.gameObject);
                    //   Debug.Log("bbb :"+LevelManager.bubbleList.Count);
                    LevelManager.bubbleTableArray[indexY, indexX] = null;
                   

                }
                else if (value == BubbleType.BUBBLE_TYPE_SPECIAL_ROCK)//41 // cuc da. rot nhanh
                {
                    destroyAfterAnim();
                    LevelManager.bubbleList.Remove(this.gameObject);
                    LevelManager.bubbleTableArray[indexY, indexX] = null;
                    LevelManager.speedOffSet = LevelManager.SPEED_OFFSET_DOWN;
                    BubbleListParent.speedY = LevelManager.speedOffSet;
                    LevelManager.moveOffSet = 0;
                    //  Bubble bubble = collision.gameObject.GetComponent<Bubble>();
                    //  bubble.destroyAfterAnim();
                    //  BubbleListParent.setLastObject();

                }
            }
            else if (state == STATE_BUBBLE_SHOOT)//boom xuyen phai xet vi no se ko goi lai colider de check
            {
                SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundDestroy);
                Bubble bubble = collision.gameObject.GetComponent<Bubble>();
                bubble.destroyAfterAnim();
                LevelManager.bubbleList.Remove(bubble.gameObject);
                LevelManager.bubbleTableArray[bubble.indexY, bubble.indexX] = null;
                LevelManager.FindAllBubbleDrop();
                BubbleListParent.setLastObject();
                Debug.Log("Day ne 1111111111111");
                if(LevelManager.checkWin())
                {
                    BubbleDrop();
                }
            }
        }
    }

    public void BubbleDrop()
    {
        GetComponent<Rigidbody2D>().gravityScale = 1;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2f, 2f), Random.Range(1f, 3f));
        LevelManager.bubbleList.Remove(this.gameObject);
        //   Debug.Log("bbb :"+LevelManager.bubbleList.Count);
        LevelManager.bubbleTableArray[indexY, indexX] = null;
        spriteRenderer.sortingOrder = -1;//here
        state = STATE_BUBBLE_DROP;
        transform.parent = null;// GamePlay.instance.BubbleListParentObject.transform;
    }

	void OnCollisionEnter2D(Collision2D collision)
	{       
        if (GamePlay.currentState == GamePlay.STATE_WATTING)
            return;
        
        if(!coliderWithWall(collision.gameObject.name) && state == STATE_BUBBLE_SHOOT)
        {
            if (collision.gameObject.tag.Equals("Bubble"))
            {
//                Debug.Log(value);
                if (collision.gameObject.GetComponent<Bubble>().value == 32)
                {
                    collision.gameObject.GetComponent<Bubble>().setValue(Random.Range(1, 9));
                }
               
              
            }           
         //  if(!CheckCollisionEnter2DBubbleThrought(collision))
            CheckCollisionEnter2D(collision);// kiem tra hieu ung 
        }	
	}
    //lastObject

    void CheckCollisionEnter2D(Collision2D collision)
    {
        isPlaySpecial = false;
        Effect.CountBeginBubble(LevelManager.currentBubble.transform.position.y);
        calIndexXY();// se nhan dc indexx va index y ung voi so o trong bang
        state = STATE_BUBBLE_IDE;
        LevelManager.currentBubble.layer = 0;// da dinh tren do        
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        if (value == BubbleType.BUBBLE_TYPE_SHOOT_FROZEN)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundPush);
            GameObject.Destroy(this.gameObject);
            GamePlay.instance.effectIce.anim.Play("Effect_ICE_BEGIN");
            LevelManager.isMoveWall = false;
            Effect.timeIceEffect = 0;
            //  BubbleListParent.setLastObject();
            //return;                    
        }
        else if (value == BubbleType.BUBBLE_TYPE_SHOOT_BOOM_CICLE)
        {
            GameObject.Destroy(this.gameObject);
            GamePlay.instance.effectSpecial.anim.Play("Effect_BOOM");
            LevelManager.DestroyAllNeighborsWhenBoom(indexY, indexX);
            GamePlay.instance.effectSpecial.anim.transform.parent.transform.position = transform.position;
            isPlaySpecial = true;
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundBoom);
        }
        else
        {
            LevelManager.bubbleTableArray[indexY, indexX] = GetComponent<Bubble>(); // LevelManager.currentBubble.GetComponent<Bubble>();
            //LevelManager.currentBubble.transform.parent = GamePlay.instance.BubbleListParentObject.transform;
            //LevelManager.currentBubble.rigidbody2D.isKinematic = true;
            //if (LevelManager.currentBubble != null)
             //   LevelManager.bubbleList.Add(LevelManager.currentBubble);
            transform.parent = GamePlay.instance.BubbleListParentObject.transform;
            GetComponent<Rigidbody2D>().isKinematic = true;            
            LevelManager.bubbleList.Add(this.gameObject);

            LevelManager.AllBubbleStickEffect();
            if (LevelManager.bubbleTableArray[indexY, indexX].value == BubbleType.BUBBLE_TYPE_SHOOT_MATCH_1)
            {
                GetNearBubbleWhenRaiwBound1(indexY, indexX);
                bool isOK = true;
                for (int i = 0; i < LevelManager.bubbleListEffect.size; i++)
                {
                    LevelManager.setDefauldBeforeCheck();
                    if (LevelManager.getAllNeighborsSameValue(LevelManager.bubbleListEffect[i]))
                    {
                        LevelManager.bubbleListNeighboursSameValue.Remove(LevelManager.bubbleTableArray[indexY, indexX]);
                        isOK = false;
                        LevelManager.checkSetAnimDestroyAllMatch2();
                    }
                }
                if (!isOK)
                {
                    //LevelManager.bubbleList.Remove(LevelManager.currentBubble);
                    LevelManager.bubbleList.Remove(this.gameObject);
                    LevelManager.bubbleTableArray[indexY, indexX].destroyAfterAnim();
                    LevelManager.bubbleTableArray[indexY, indexX] = null;
                }
            }
            else if (LevelManager.bubbleTableArray[indexY, indexX].value == BubbleType.BUBBLE_TYPE_SHOOT_MATCH_2)
            {
                if (collision.gameObject.tag.Equals("Bubble"))
                {
                    GetAllBubbleWhenRaiwBound2(collision.gameObject.GetComponent<Bubble>().value);

                    LevelManager.bubbleListNeighboursSameValue.Add(LevelManager.bubbleTableArray[indexY, indexX]);
                    LevelManager.checkSetAnimDestroyAllMatch2();

                }
                if (LevelManager.bubbleTableArray[indexY, indexX] != null)
                {
                    LevelManager.bubbleList.Remove(this.gameObject);
                    LevelManager.bubbleTableArray[indexY, indexX].destroyAfterAnim();
                    LevelManager.bubbleTableArray[indexY, indexX] = null;
                }

            }
            else if (LevelManager.getAllNeighborsSameValue(this.gameObject))
            {
                //SoundEngine.playSound("SoundMatch");
                LevelManager.checkSetAnimDestroyAllMatch2();
            }
            else
            {
                //if(value == BubbleType.BUBBLE_TYPE_SHOOT_FROZEN)
                BubbleListParent.setLastObject(this.gameObject);
              //  animScore.Play("BUBBLE_ANIM_STICK");    
                //LevelManager.AllBubbleStickEffect();
                SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundStick);
                PlayeBubbleEffect(collision.gameObject.GetComponent<Bubble>()); //setPos();
            }
            
        }
        //Debug.Log(LevelManager.bubbleList.size);
        LevelManager.FindAllBubbleDrop();            
        BubbleListParent.setLastObject();
        Effect.PlayEffectText();
       
        if (LevelManager.bubbleList.size > 0)
        {
            LevelManager.getBubbleListStillAliveIndex();
          //  LevelManager.createNewBubble();
        }
     //   Debug.Log("Day ne 22222222222222222222");
        LevelManager.checkWin();
        setPos();
    }
    void PlayeBubbleEffect(Bubble bubble)
    {        
        if (bubble == null)//here sao crash??
            return;
        switch (bubble.effect)
        {
            case EFFECT_NONE:
                break;
            case EFFECT_BONE:                
                //bubble.anim.Play("BONE" + bubble.value.ToString());
                bubble.effect = EFFECT_NONE;
                bubble.PlayAnimBONE();
                bubble.CreateBubbleWhenBoneEffect(false);
                break;
            case EFFECT_BONE_REPLACE:                
                //bubble.anim.Play("BONE" + bubble.value.ToString());
                bubble.effect = EFFECT_NONE;
                bubble.PlayAnimBONE();
                bubble.CreateBubbleWhenBoneEffect(true);
                break;                
        }
    }
   
    public static bool isTrueRowCol(int row,int col)//kiem tra coi co dung la no nam tren bang hay ngoai
    {
        if (row < 0 || row >= LevelManager.MAX_ROW || col < 0 || col >= LevelManager.MAX_COL)
            return false;
        return true;
    }

    void CreateBubbleWhenBoneEffect(bool isReplace)
    {
        //Debug.Log(indexX + "," + indexY);
        int col = indexX;
        int row = indexY;
		
        int temp = 1;        
        if ((row % 2) == 0)
            temp = 0;

        addObjectEffect1(row - 1, col - 1 + temp, isReplace);
        addObjectEffect1(row - 1, col + temp, isReplace);
        addObjectEffect1(row, col - 1, isReplace);
        addObjectEffect1(row, col + 1, isReplace);
        addObjectEffect1(row + 1, col - 1 + temp, isReplace);
        addObjectEffect1(row + 1, col + temp, isReplace);
       
    }
    public void addObjectEffect1(int row, int col,bool isReplace)
    {
        if (!isTrueRowCol(row, col))
            return;
        if (LevelManager.bubbleTableArray[row, col] == null)
        {
            GameObject obj = (GameObject)(GameObject.Instantiate(this.gameObject));//Resources.Load("BubblePrefab")));
            Bubble bubble = obj.GetComponent<Bubble>();
            obj.transform.parent = GamePlay.instance.BubbleListParentObject.transform;
            LevelManager.bubbleTableArray[row, col] = bubble;
           
            bubble.setValue(bubble.value);
            bubble.indexX = col;
            bubble.indexY = row;
            
            bubble.setPos();//.position = new Vector3(0f, GamePlay.CURRENT_START_Y, 0f);
            //bubble.anim.Play("BONE" + bubble.value.ToString());
            bubble.effect = EFFECT_NONE;
            bubble.PlayAnimBONE();
            LevelManager.bubbleList.Add(obj);
        }else if(isReplace)
        {

            Bubble bubble1 = this.gameObject.GetComponent<Bubble>();
            Bubble bubble = LevelManager.bubbleTableArray[row, col].GetComponent<Bubble>();
            bubble.setValue(bubble1.value);
            bubble.setPos();//.position = new Vector3(0f, GamePlay.CURRENT_START_Y, 0f);
            //bubble.anim.Play("BONE" + bubble.value.ToString());
            bubble.effect = EFFECT_NONE;
            bubble.PlayAnimBONE();
          //  LevelManager.bubbleList.Add(obj);
        }
    }

    public void addObjectNearRainBown1(int row, int col)
    {

        if (!isTrueRowCol(row, col))
            return;
        if (LevelManager.bubbleTableArray[row, col] != null)
            LevelManager.bubbleListEffect.Add(LevelManager.bubbleTableArray[row, col].gameObject);
    }
    void GetNearBubbleWhenRaiwBound1(int row,int col)//cau vong 1
    {
        int temp = 1;        
        if ((row % 2) == 0)
            temp = 0;
        LevelManager.bubbleListEffect.Clear();
        addObjectNearRainBown1(row - 1, col - 1 + temp);
        addObjectNearRainBown1(row - 1, col + temp);
        addObjectNearRainBown1(row, col - 1);
        addObjectNearRainBown1(row, col + 1);
        addObjectNearRainBown1(row + 1, col - 1 + temp);
        addObjectNearRainBown1(row + 1, col + temp);        
    }
	void GetAllBubbleWhenRaiwBound2(int _value)
    {
        LevelManager.bubbleListNeighboursSameValue.Clear();
        for (int i = 0; i < LevelManager.bubbleList.size;i++ )
        {
            if(LevelManager.bubbleList[i].activeSelf)
            {
                if (LevelManager.bubbleList[i].GetComponent<Bubble>().value == _value)
                    LevelManager.bubbleListNeighboursSameValue.Add(LevelManager.bubbleList[i].GetComponent<Bubble>());
            }
        }
            
    }
    void calIndexXY()
    {
        //Debug.Log ("ball");
        indexY = LevelManager.calIndexY(transform.position.y);
        indexX = LevelManager.calIndexX(transform.position.x, indexY);
        //can ckeck cho nay
       // Debug.Log(" " + indexX + ", " + indexY);
        if (indexX >= LevelManager.MAX_COL)
            indexX = LevelManager.MAX_COL - 1;
        if (LevelManager.bubbleTableArray[indexY, indexX] != null)
            indexY++;

        if (indexX < 0)
            indexX=0;
        if (LevelManager.bubbleTableArray[indexY, indexX] != null)
            indexY++;
        //Debug.Log(" " + indexX +", " +indexY);
    }

    public void destroyAfterAnim()
    {
        countAnimScore++;
        GameObject obj =  this.gameObject;
        obj.GetComponent<Bubble>().anim.Play("IDE_NONE");
        GameObject animEffect = (GameObject)(Instantiate(BubbleListParent.instance.ObjectBubbleAnimEffect, transform.position, transform.rotation));//, transform.position, transform.rotation);
        
        if(countAnimScore%2 ==0)
            animEffect.GetComponent<BubbleAnimEffect>().AnimScore.Play("BUBBLE_ANIM_SCORE_EFFECT");//here
        else
            animEffect.GetComponent<BubbleAnimEffect>().AnimScore.Play("BUBBLE_ANIM_SCORE_EFFECT2");//here

        
        animEffect.GetComponent<BubbleAnimEffect>().AnimEffect.Play("DESTROY");//here

        animEffect.transform.parent = this.transform;
        animEffect.GetComponent<BubbleAnimEffect>().linkBubbleObject = this.gameObject;
        //obj.GetComponent<Bubble>().animScore.Play("BUBBLE_ANIM_SCORE_EFFECT");
        obj.GetComponent<Bubble>().state = Bubble.STATE_BUBBLE_WAITTING_DESTROY;
        //obj.GetComponent<Bubble>().rigidbody2D.isKinematic = false;
        obj.GetComponent<Bubble>().GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        obj.GetComponent<Bubble>().GetComponent<Rigidbody2D>().isKinematic = true;
        obj.layer = 12;

        if (value == BubbleType.BUBBLE_TYPE_THROUGH_COIN)//coin 34
        {
            GameObject obj1 = (GameObject)(GameObject.Instantiate(Resources.Load("Coin")));
            obj1.GetComponent<Animator>().Play("Coin");
            obj1.transform.position = transform.position;
            iTween.MoveTo(obj1, iTween.Hash("y", 4.8, "x", -2.8, "delay", 0.0, "speed", 3, "easetype", "easeInOutSine", "oncomplete", "CoinMoveBeginCompleted"));//                        
        }
        else if (value == 37 || (value >= 70 && value < 84 || value == 40 ))//hinh nguoi va trai cay
        {
            GameObject obj1 = (GameObject)(GameObject.Instantiate(Resources.Load("Coin")));
            obj1.GetComponent<Animator>().Play("Star");
            obj1.transform.position = transform.position;
            iTween.MoveTo(obj1, iTween.Hash("y", 4.8, "x", -2.8, "delay", 0.0, "speed", 3, "easetype", "easeInOutSine", "oncomplete", "StarMoveBeginCompleted"));//
        }
        else if (value > 100)//kim cuong
        {
            GameObject obj1 = (GameObject)(GameObject.Instantiate(Resources.Load("Coin")));
            obj1.GetComponent<Animator>().Play("Diamond");
            obj1.transform.position = transform.position;
            iTween.MoveTo(obj1, iTween.Hash("y", 4.8, "x", -2.8, "delay", 0.0, "speed", 3, "easetype", "easeInOutSine", "oncomplete", "DiamondMoveBeginCompleted"));//
        }
        else if (value == BubbleType.BUBBLE_TYPE_SHOOT_NEW_EFFECT)//36
        {
            GameObject obj1 = (GameObject)(GameObject.Instantiate(Resources.Load("Coin")));
            obj1.GetComponent<Animator>().Play("Light");
            obj1.transform.position = transform.position;
            iTween.MoveTo(obj1, iTween.Hash("y", -3.4, "x", 1.2, "delay", 0.0, "speed", 3, "easetype", "easeInOutSine", "oncomplete", "LightMoveBeginCompleted"));//
        }
    }
    public void moveCreateNewBubbleCompleted()
    {
        //if(LevelManager.currentBubble == null)
            state = Bubble.STATE_BUBBLE_WATING_SHOOT;
            spriteRenderer.sortingOrder = -3;
     //   Debug.Log("aaaaaaa");
    }
}
