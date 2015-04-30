using UnityEngine;
using System.Collections;

public class BubbleListParent : MonoBehaviour {

	// Use this for initialization
    public static GameObject lastObject = null;//dung de kiem tra thang cuoi cung
    public static GameObject WallTopObject = null;//dung de kiem tra thang cuoi cung
    public static float speedY = 0f;
    public static float SPEED_MAX = 2.5f;
    public static float SPEED_MEDIUM =  0.16f;
    public static float SPEED_SLOW = 0.06f;
    public static BubbleListParent instance;

    public  Sprite[] spritesBubble;
    public  GameObject ObjectBubbleAnimEffect;
    
	void Start () {
        WallTopObject = GameObject.Find("WallTop");
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void moveBeginCOmpleted()
    {
        GamePlay.changeState(GamePlay.STATE_READY_GO);
        Effect.GoReadyPlay();
        GamePlay.TimePlayedSubState = 0;
        GamePlay.instance.BubblePre.SetActive(true);
      //  GamePlay.instance.BubblePre.GetComponent<Bubble>().setValue(GamePlay.instance.BubblePre.GetComponent<Bubble>().value);
        LevelManager.createNewBubble();
     //   GamePlay.instance.WallBottom.SetActive(true);
        LevelManager.BEGIN_Y = this.transform.position.y + LevelManager.BEGIN_Y_DEFAULT;
        LevelManager.isMoveWall = true;
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundReadyGo);       
    }
    public static void GetLastBubbleBottom()
    {
        float y = 0;// lastObject.transform.position.y;
        GameObject obj;
        for (int i = 0; i <LevelManager.bubbleList.size; i++)
        {
            obj = LevelManager.bubbleList[i];
            if (obj.transform.position.y > y)
            {
                lastObject = obj;
            }
        }
    }
    public static float GetLastBubbleBottomY()
    {
        Debug.Log(lastObject.transform.position.y);
        return lastObject.transform.position.y;
    }
    public static void Calcul_BEGIN_Y()
    {
        LevelManager.BEGIN_Y = GetLastBubbleBottomY();
        if (LevelManager.BEGIN_Y < LevelManager.BEGIN_Y_DEFAULT)
            LevelManager.BEGIN_Y = LevelManager.BEGIN_Y_DEFAULT;

    }

    public static void UpdatekMoveWall()
    {
        if (!LevelManager.isMoveWall)
        {
            Effect.timeIceEffect += Time.deltaTime;
            if (Effect.timeIceEffect > 3)
            {
                LevelManager.isMoveWall = true;
                GamePlay.instance.effectIce.anim.Play("Effect_NONE");
            }
        }
        else
        {
            if (LevelManager.speedOffSet < 0)
            {
                if (BubbleListParent.speedY <= (LevelManager.speedOffSet / 3))
                    BubbleListParent.speedY += Time.deltaTime * LevelManager.speedOffSet;
                LevelManager.BEGIN_Y += Time.deltaTime * BubbleListParent.speedY;
                LevelManager.moveOffSet += Time.deltaTime * BubbleListParent.speedY;
                if (LevelManager.moveOffSet < LevelManager.MOVE_OFFSET_DOWN)
                {
                    LevelManager.speedOffSet = 0;
                    BubbleListParent.speedY = 0;
                    LevelManager.moveOffSet = 0;
                }
            }
            else if (LevelManager.speedOffSet > 0)
            {
                if (BubbleListParent.speedY < (LevelManager.speedOffSet / 3))
                    BubbleListParent.speedY -= Time.deltaTime * LevelManager.speedOffSet;
                LevelManager.BEGIN_Y += Time.deltaTime * BubbleListParent.speedY;
                LevelManager.moveOffSet += Time.deltaTime * BubbleListParent.speedY;
                //  LevelManager.speedOffSet -= 0.01f;
                if (LevelManager.moveOffSet > LevelManager.MOVE_OFFSET_UP || LevelManager.speedOffSet <= 0)
                {
                    LevelManager.speedOffSet = 0;
                    BubbleListParent.speedY = 0; ;
                    LevelManager.moveOffSet = 0;
                }
            }
            else if (lastObject == null)
            {
                LevelManager.BEGIN_Y -= Time.deltaTime * SPEED_SLOW;

            }
            else if (lastObject.transform.position.y > 5.5f)
            {
                LevelManager.BEGIN_Y -= Time.deltaTime * SPEED_MAX * 2;
            }
            else if (lastObject.transform.position.y > 2.5f)
            {
                if (speedY < SPEED_MAX)
                    speedY += Time.deltaTime * 1f;
                LevelManager.BEGIN_Y -= Time.deltaTime * speedY;
            }
            else if (lastObject.transform.position.y > 0.5f)
            {
                LevelManager.BEGIN_Y -= Time.deltaTime * SPEED_MEDIUM;
            }
            else// (lastObject.transform.position.y < 0)
            {
                LevelManager.BEGIN_Y -= Time.deltaTime * SPEED_SLOW;
            }

            if (lastObject != null)
            {
                instance.transform.position = new Vector3(0, LevelManager.BEGIN_Y, 0);
                float t1 = lastObject.transform.position.y;
                float t = LevelManager.BEGIN_Y - lastObject.GetComponent<Bubble>().indexY * (LevelManager.WIDTH + LevelManager.HIGHT_OFFSET_Y);
                instance.transform.position = new Vector3(0, instance.transform.position.y - t1 + t, 0);
            }


        }

    }
   
    public static void setLastObject(GameObject obj)
    {        
        // Debug.Log(lastObject.transform.position.y);
        if (obj == null || lastObject == null)
            setLastObject();
        if (obj.transform.position.y < lastObject.transform.position.y)
        {
            lastObject = obj;
        }        
    }
    public static void setLastObject()///khi khong co no bubble
    {
        int count = LevelManager.bubbleList.size;
        GameObject obj = null;
        lastObject = null;
        for (int i = 0; i < count; i++)
        {
            if (lastObject == null)
            {
                //if (LevelManager.bubbleList[i]!=null)
                lastObject = LevelManager.bubbleList[count - 1];
            }
            else
            {
                obj = LevelManager.bubbleList[i];
                // Debug.Log(obj.transform.position.y);
                // Debug.Log(lastObject.transform.position.y);
                if (obj != null)
                {
                    if (obj.transform.position.y < lastObject.transform.position.y)
                    {
                        lastObject = obj;
                    }
                }
            }

        }

    }
}
