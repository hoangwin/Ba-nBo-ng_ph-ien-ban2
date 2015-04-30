
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
public class LevelManager {
    
	public static string allLevels;	
	public static float BEGIN_X = -2.20f;
    public static float BEGIN_Y_DEFAULT = 3.8f;
    public static float BEGIN_Y_TARGET = 3.8f;
	public static float BEGIN_Y = 3.8f;
	public static float WIDTH = 0.58f;
	public static float HIGHT_OFFSET_Y = -0.039f;
	public static int MAX_COL = 10;
	public static int MAX_ROW = 17;
    public static int BEGIN_SHOW_ROW = 0;
    public static int END_SHOW_ROW = 0;

    //cai nay dung cho di chuyen len xuong khi du
    public static float speedOffSet = 0;
    public static float moveOffSet = 0;
    public static float SPEED_OFFSET_DOWN = -0.5f;
    public static float SPEED_OFFSET_UP = 0.8f;    
    public static float MOVE_OFFSET_DOWN = -0.8f;
    public static float MOVE_OFFSET_UP = 0.7f;
    

	public static Bubble[,] bubbleTableArray = null;//new GameObject[MAX_ROW,MAX_COL];     
    public static BetterList<Bubble> bubbleListNeighboursSameValue = new BetterList<Bubble>();//la de tinh cac object se cung value
    public static BetterList<Bubble> bubbleListNeighbours = new BetterList<Bubble>();//la xem thu thang nao se rot xuong
    public static ArrayList bubbleListStillAliveIndex = new ArrayList();// la nhung gia tri da co san tren bang. neu ko co thi remove ra
    public static BetterList<GameObject> bubbleListEffect = new BetterList<GameObject>();//DUng tinh toan cho hieu ung cau vong
    public static BetterList<GameObject> bubbleList = new BetterList<GameObject>();//Con lai bao nhieu bubble tren man hinh
    //public static BetterList<Bubble> bubbleListWillDestroy = new BetterList<Bubble>();//Cai nay de destroy theo luot

    public static BetterList<int> bubbleListWillShoot = new BetterList<int>();// se ban


	public static int currentLevel = -1;
    public static GameObject currentBubble = null;
    public static GameObject currentBubbleWaiting = null;    
    public static int countAllBubble = 0;
    public static int countAllBubbleType = 0;

    public static int countbubbleShoot;
    public static bool isMoveWall;

    public static void getLevel2(int level)
    {
        SetBeginPositon();
        countAllBubble = 0;
        countbubbleShoot = 0;
        if (level <= 0) level = 1;
        string file = "Level2/level"+ (level);
        TextAsset txt = (TextAsset)Resources.Load(file, typeof(TextAsset));
        Debug.Log("file :" + file + " , Level :" + (level));
        Debug.Log(txt.text);
        allLevels = txt.text;

        JSONNode N = JSON.Parse(allLevels);
        bubbleListWillShoot.Clear();
        int temp = 0;
        while (temp != -1)
        {
            int _value = N["serveballs"]["balls"][temp].AsInt;        // versionString will be a string containing "1.0"
            temp++;
            if (_value > 0)
                bubbleListWillShoot.Add(_value);
            else
                temp = -1;
        }      

        MAX_COL = N["levballs"]["oneline"].AsInt;        // versionString will be a string containing "1.0"
        MAX_ROW = N["levballs"]["lines"].AsInt;
        BEGIN_SHOW_ROW = 0;
        END_SHOW_ROW = MAX_COL - 1;		

        destroyTable();
        bubbleTableArray = new Bubble[MAX_ROW + 15, MAX_COL];
        int value =  0;
        GameObject obj;
        Bubble bubble;

        for (int j = 0; j < MAX_ROW; j++)
        {
            for (int i = 0; i < MAX_COL; i++)
            {
                value = N["levballs"]["balls"][j * MAX_COL + i].AsInt;
                if (bubbleTableArray[j, i] != null)
                {
                    GameObject.Destroy(bubbleTableArray[j, i]);
                }
                if (value > 0 && value < 1202)
                {
                    countAllBubble++;
                    obj = (GameObject)GameObject.Instantiate(Resources.Load("BubblePrefab"));
                    bubble = obj.GetComponent<Bubble>();
                    bubble.indexX = i;
                    bubble.indexY = j;
                    bubble.setPos();
                    bubble.setValue(value);
                    bubble.state = Bubble.STATE_BUBBLE_IDE;

                    obj.GetComponent<Rigidbody2D>().isKinematic = true;
                    bubbleTableArray[bubble.indexY, bubble.indexX] = bubble;
                    obj.transform.parent = GamePlay.instance.BubbleListParentObject.transform;
                    bubbleList.Add(obj);
                }
            }
        }
        BubbleListParent.lastObject =  bubbleList[bubbleList.size -1];
        MAX_ROW += 15;
        LevelManager.getBubbleListStillAliveIndex();
        countAllBubbleType = bubbleListStillAliveIndex.Count;

      
       // GamePlay.instance.BubbleListParent.transform.position = new Vector3(0, BEGIN_Y, 0);
    }
    public static void SetBeginPositon()
	{
	    BEGIN_Y = BEGIN_Y_DEFAULT;// + (MAX_ROW)*WIDTH;
        isMoveWall = false;
        GameObject objWall = GameObject.Find("WallTop");
        GamePlay.instance.BubbleListParentObject.transform.position = new Vector3(0, 0, 0);
        objWall.transform.position = new Vector3(objWall.transform.position.x, BEGIN_Y + WIDTH / 2 + 0.1f, 0);
	}
    public static void UpdateVisibleTable()
    {

        int tempBegin = BEGIN_SHOW_ROW;
        int tempEnd = END_SHOW_ROW;
        BEGIN_SHOW_ROW = calIndexY(4.5f);
        if(BEGIN_SHOW_ROW <0 )
            BEGIN_SHOW_ROW = 0;
        END_SHOW_ROW = calIndexY(-6.5f);
        if (END_SHOW_ROW < 0)
            END_SHOW_ROW = 0;
        if(END_SHOW_ROW >=MAX_ROW)
            END_SHOW_ROW = MAX_ROW-1;
        if (tempBegin != BEGIN_SHOW_ROW)
        {
            if (tempBegin < BEGIN_SHOW_ROW)
            {
                for (int i = tempBegin; i < BEGIN_SHOW_ROW; i++)
                {
                    for (int j = 0; j < MAX_COL; j++)
                    {
                        if (bubbleTableArray[i, j] != null)
                        {
                            bubbleTableArray[i, j].gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                for (int i = BEGIN_SHOW_ROW; i < tempBegin; i++)
                {
                    for (int j = 0; j < MAX_COL; j++)
                    {
                        if (bubbleTableArray[i, j] != null)
                        {
                            bubbleTableArray[i, j].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }


        if (tempBegin != BEGIN_SHOW_ROW || tempEnd != END_SHOW_ROW)
        {
            for (int i = BEGIN_SHOW_ROW; i < END_SHOW_ROW; i++)
            {
                    for (int j = 0; j < MAX_COL; j++)
                    {
                        if (bubbleTableArray[i, j] != null)
                        {
                            if (!bubbleTableArray[i, j].gameObject.active)
                            {
                                bubbleTableArray[i, j].gameObject.SetActive(true);
                                bubbleTableArray[i, j].RePlayAnim();
                            }
                        }
                    }
             
            }
        }

        if (tempEnd != END_SHOW_ROW)
        {
            if (tempEnd < END_SHOW_ROW)
            {
                for (int i = tempEnd; i < END_SHOW_ROW; i++)
                {
                    for (int j = 0; j < MAX_COL; j++)
                    {
                        if (bubbleTableArray[i, j] != null)
                        {
                            
                            bubbleTableArray[i, j].gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                for (int i = END_SHOW_ROW; i < tempEnd; i++)
                {
                    for (int j = 0; j < MAX_COL; j++)
                    {
                        if (bubbleTableArray[i, j] != null)
                        {
                            bubbleTableArray[i, j].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
       // Debug.Log(BEGIN_SHOW_ROW + "," + END_SHOW_ROW);
    }  
  
    public static float getposX(int indexX, int indexY)
    {
        if ((indexY % 2) == 0)
            return BEGIN_X + indexX * WIDTH - WIDTH/2;
        else
            return BEGIN_X + indexX * WIDTH;
    }

    public static float getposY(int indexX, int indexY)
    {
        return BEGIN_Y - indexY * (WIDTH + HIGHT_OFFSET_Y);
    }

    public static int calIndexX(float x, int indexY)
    {
        if ((indexY % 2) == 0)
        {
            int i = (int)((x - BEGIN_X + WIDTH) / WIDTH);
            // if (i == 0)
            //     i++;
            return i;
        }
        else
        {
		            return (int)((x - BEGIN_X + WIDTH / 2) / WIDTH);
        }
    }

    public static int calIndexY(float y)
    {
        //BEGIN_Y - indexY*(WIDTH +HIGHT_OFFSET_Y ) = y;
        return (int)((BEGIN_Y - y + WIDTH / 2) / (WIDTH + HIGHT_OFFSET_Y));
    }

    public static void destroyTable()
    {        
        for (int j = 0; j < bubbleList.size; j++)
        {
            if (bubbleList[j] != null)
               GameObject.Destroy(bubbleList[j]);
        }
        bubbleList.Clear();  
    }

    public static void getBubbleListStillAliveIndex()
    {
        bubbleListStillAliveIndex.Clear();

        for (int j = 0; j < bubbleList.size; j++)
        {
            if (bubbleList[j] != null)
            {
                if (bubbleList[j].GetComponent<Bubble>().state == Bubble.STATE_BUBBLE_IDE)//here
                {
                    int value = bubbleList[j].GetComponent<Bubble>().value;
					if(value<9)
					{
					bool isAdd = true;
		            int n = 0;
                    for (n = 0; n < bubbleListStillAliveIndex.Count; n++)
                    {

                        if ((int)bubbleListStillAliveIndex[n] == value)
						{
                            isAdd=false;;
							n = bubbleListStillAliveIndex.Count +1;
						}
                    }
                    if (isAdd)
                        bubbleListStillAliveIndex.Add(value);
						}
                }
            }

        }
    }
  
    public static bool checkABubbleWillShoot(int value)
    {
        for (int i = 0; i < bubbleListStillAliveIndex.Count; i++)
        {
            if(value == ((int)bubbleListStillAliveIndex[i]))
                return true;
        }
        return false;
    }

    public static void setDefauldBeforeCheck()
    {
        for (int i = 0; i < bubbleList.size; i++)
            if (bubbleList[i] != null)
                bubbleList[i].GetComponent<Bubble>().isCheck = false;
        /*
		for (int j=0 ; j<MAX_ROW ; j++)
		{
			for (int i=0 ; i<MAX_COL ; i++)
			{
				if(bubbleTableArray[j,i] != null)
					bubbleTableArray[j,i].GetComponent<Bubble>().isCheck = false;
			}
		}
         * */
    }
	
    public static void addObjectNeighborsSameValue(int row,int col,int _value)
	{
        if (!Bubble.isTrueRowCol(row, col))
            return;
        if (bubbleTableArray[row, col] != null)
        {
            if ((bubbleTableArray[row, col].GetComponent<Bubble>().value == _value
                || bubbleTableArray[row, col].GetComponent<Bubble>().value == BubbleType.BUBBLE_TYPE_SHOOT_MATCH_1)
                && !bubbleTableArray[row, col].GetComponent<Bubble>().isCheck)
            {
                bubbleListNeighboursSameValue.Add(bubbleTableArray[row, col]);
                bubbleTableArray[row, col].GetComponent<Bubble>().isCheck = true;
            }
        }

	}
	
    public static void addObjectNeighbors(int row,int col,int _Depth)
	{
        if (!Bubble.isTrueRowCol(row, col))
            return;
		if(bubbleTableArray[row,col] != null)
			//if(bubbleTableArray[row,col].GetComponent<Bubble>() != null)
            
				if(!bubbleTableArray[row,col].GetComponent<Bubble>().isCheck)
				{
                    bubbleTableArray[row, col].GetComponent<Bubble>().depth = _Depth;
					bubbleListNeighbours.Add(bubbleTableArray[row,col]);
					bubbleTableArray[row,col].GetComponent<Bubble>().isCheck = true;
				}		
	}

	public static bool getAllNeighborsSameValue(GameObject currentBubble)
	{
		bubbleListNeighboursSameValue.Clear();
		setDefauldBeforeCheck();
		bubbleListNeighboursSameValue.Add(currentBubble.GetComponent<Bubble>());
		int value = currentBubble.GetComponent<Bubble>().value;
		currentBubble.GetComponent<Bubble>().isCheck = true;
		Bubble obj = null;
		int row = 0 ;
		int col = 0;
        int temp = 0;
		for(int i = 0;i< bubbleListNeighboursSameValue.size;i++)
		{
			obj = bubbleListNeighboursSameValue[i];
			row = obj.GetComponent<Bubble>().indexY;
			col = obj.GetComponent<Bubble>().indexX;
			//kiem tra 6 vi tri xung quanh
			//
			//		6   6   4   4   2   2   3   3
			//		  6   6   4   4   2   2   3
			//		2   2   3  (1)   (2)   6   4   4
			//		  2   3  (3)  (0)  (4)   4   4
			//		-   -   -  (5)   (6)   -   -   -
			//		  -   -   -   -   -   -   -
			//		-   -   -   -   -   -   -   -
			//		  -   -   -   -   -   -   -
			//		-   -   -   -   -   -   -   -
			//		  -   -   -   -   -   -   -
			//vi tri so 1
            temp = 1;
            if ((row % 2) == 0)
                temp = 0;

            addObjectNeighborsSameValue(row - 1, col - 1 + temp,value);
            addObjectNeighborsSameValue(row - 1, col + temp, value);
            addObjectNeighborsSameValue(row, col - 1, value);
            addObjectNeighborsSameValue(row, col + 1, value);
            addObjectNeighborsSameValue(row + 1, col - 1 + temp, value);
            addObjectNeighborsSameValue(row + 1, col + temp, value);

		}
		if(bubbleListNeighboursSameValue.size>=3)
		{
			return true;
		}

		return false;
	}

    public static void AllBubbleStickEffect()
    {
        
        setDefauldBeforeCheck();
        Bubble bubble = currentBubble.GetComponent<Bubble>();
        
        bubbleListNeighbours.Clear();
        bubbleListNeighbours.Add(bubble);
        bubble.depth = 0;
        bubble.isCheck = true;

        int row = bubble.indexY;
        int col = bubble.indexX;
        getAllNeighbors(bubbleTableArray[row, col], 3);
//        Debug.Log(bubbleListNeighbours.size);
        for (int i = 0; i < bubbleListNeighbours.size; i++)
        {
            if (bubbleListNeighbours[i].depth ==0)
                bubbleListNeighbours[i].anim.Play("MOVE_UP");
            else if (bubbleListNeighbours[i].depth == 1)
                bubbleListNeighbours[i].anim.Play("MOVE_UP1");
            else if (bubbleListNeighbours[i].depth ==2)
                bubbleListNeighbours[i].anim.Play("MOVE_UP2");
            else
                bubbleListNeighbours[i].anim.Play("MOVE_UP3");
        }
      
    }

    public static void FindAllBubbleDrop()
    {
        setDefauldBeforeCheck();
        Bubble bubble = null;
        int row;
        int col;
        for (int i = 0; i < bubbleList.size; i++)
        {
            bubbleListNeighbours.Clear();
            if (bubbleList[i] != null)
            {
                bubble = bubbleList[i].GetComponent<Bubble>();
                if (!bubbleList[i].GetComponent<Bubble>().isCheck)
                {
                    row = bubble.indexY;
                    col = bubble.indexX;
                    getAllNeighbors(bubbleTableArray[row, col], -1);
                    //Debug.Log("Nummm : "+ bubbleListNeighbours.Count);
                    //kiem tra o day. neu ko co lien ket den 0 thi se cho roi
                    bool isdrop = true;
                    for (int n = 0; n < bubbleListNeighbours.size; n++)
                    {
                        if ((bubbleListNeighbours[n]).indexY == 0)
                        {
                            isdrop = false;
                            break;
                        }
                    }
                    if (isdrop)
                    {
                        for (int n = 0; n < bubbleListNeighbours.size; n++)
                        {
                            bubble = (bubbleListNeighbours[n]);
                            bubble.BubbleDrop();
                            // Debug.Log("aaaaaaaa :" + LevelManager.bubbleList.size);							
                        }                        
                        i = 0;//reset lai de  fix bug drop ko dc
                    }
                }
            }
        }
    }
                
            
	
    public static void checkSetAnimDestroyAllMatch2()
    {        
        for (int i = 0; i < bubbleListNeighboursSameValue.size; i++)
        {
            LevelManager.bubbleList.Remove(bubbleListNeighboursSameValue[i].gameObject);

            LevelManager.bubbleTableArray[bubbleListNeighboursSameValue[i].indexY, bubbleListNeighboursSameValue[i].indexX] = null;
            bubbleListNeighboursSameValue[i].destroyAfterAnim();            
        }        
    }
  
    public static bool checkWin()
	{
        if (GamePlay.currentState != GamePlay.STATE_PLAY)
            return false;
        if(bubbleList.size > 0)
            return false;
        /*
		for (int i=0 ; i<MAX_COL ; i++)
		{
            
            if (bubbleTableArray[0, i] != null && (bubbleTableArray[0, i].GetComponent<Bubble>().state != Bubble.STATE_BUBBLE_WAITTING_DESTROY))
				return false;					
		}
         */
        GamePlay.TimePlayedSubState = 0f;
        GamePlay.isWin = true;
         
        GamePlay.changeState(GamePlay.STATE_WAITING_WIN_LOSE);
        
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundWin);
		return true;
	}

    public static void getAllNeighbors(Bubble currentBubble,int _depth)
	{
           
			bubbleListNeighbours.Clear();
            currentBubble.GetComponent<Bubble>().depth = 0;
			bubbleListNeighbours.Add(currentBubble);			

			currentBubble.isCheck = true;

            Bubble obj = null;
			int row = 0 ;
			int col = 0;
            int temp = 0;

			for(int i = 0;i< bubbleListNeighbours.size;i++)
			{
				obj = bubbleListNeighbours[i];
				row = obj.indexY;
				col = obj.indexX;
                temp = 1;
                if ((row % 2) == 0)
                    temp = 0;
                //Debug.Log("a:" + obj.depth);
                if (_depth == -1 || obj.depth < _depth)
                {
                    addObjectNeighbors(row - 1, col - 1 + temp, obj.depth + 1);
                    addObjectNeighbors(row - 1, col + temp, obj.depth + 1);
                    addObjectNeighbors(row, col - 1, obj.depth + 1);
                    addObjectNeighbors(row, col + 1, obj.depth + 1);
                    addObjectNeighbors(row + 1, col - 1 + temp, obj.depth + 1);
                    addObjectNeighbors(row + 1, col + temp, obj.depth + 1);
                }
			}
	}
    public static void DestroyAllNeighborsWhenBoom(int row, int col)
    {
        bubbleListNeighbours.Clear();
        for (int i = -2; i <= 2;i++ )
            for (int j = -2; j <= 2; j++)
            {
                getNeighborsWhenBoom(row + i, col+j);
            }
        
        for(int i =0;i<bubbleListNeighbours.size;i++)
        {
                LevelManager.bubbleList.Remove(bubbleListNeighbours[i].gameObject);

                LevelManager.bubbleTableArray[bubbleListNeighbours[i].indexY, bubbleListNeighbours[i].indexX] = null;
                bubbleListNeighbours[i].destroyAfterAnim();             
        }
    }
    public static void getNeighborsWhenBoom(int row, int col)
    {
        if (row < 0 || row >= MAX_ROW)
            return;
        if (col < 0 || col >= MAX_COL)
            return;
        if (bubbleTableArray[row, col] != null)
        {
            bubbleListNeighbours.Add(bubbleTableArray[row, col]);
        }
					
    }
    
    public static void createNewBubble()
    {

        int value = GamePlay.instance.BubblePre.GetComponent<Bubble>().value;
        if (countbubbleShoot < bubbleListWillShoot.size)
        {
            value = bubbleListWillShoot[countbubbleShoot];
        }
        else if (LevelManager.bubbleListStillAliveIndex.Count > 0)
        {
            //here
            value = changeValue(value);
            
        }
        else
        {
           // value = 1;// chong crash
        }
        //value = 29;
        //MOVE_NEW_CURRENT_BUBBLE
        LevelManager.currentBubbleWaiting = (GameObject)(GameObject.Instantiate(Resources.Load("BubblePrefab")));
        // LevelManager.currentBubbleWaiting = (GameObject)(GameObject.Instantiate(GamePlay.instance.BubblePre));
        LevelManager.currentBubbleWaiting.GetComponent<Bubble>().transform.position = GamePlay.instance.BubblePre.transform.position;
        LevelManager.currentBubbleWaiting.GetComponent<Bubble>().spriteRenderer.sortingOrder = -1;
        LevelManager.currentBubbleWaiting.GetComponent<Bubble>().setValue(value);
        //LevelManager.currentBubbleWaiting.GetComponent<Bubble>().transform.position = new Vector3(0f, GamePlay.CURRENT_START_Y, 0f);
        LevelManager.currentBubbleWaiting.GetComponent<Bubble>().state = Bubble.STATE_BUBBLE_IDE;
        LevelManager.currentBubbleWaiting.layer = 12;


        iTween.MoveTo(LevelManager.currentBubbleWaiting, iTween.Hash("x", 0, "y", GamePlay.CURRENT_START_Y, "easeType", "linear", "delay", 0.0, "time", 0.4, "oncomplete", "moveCreateNewBubbleCompleted"));
      //  iTween.RotateTo(LevelManager.currentBubbleWaiting,iTween.Hash("rotation",new Vector3(0,0,0)),iTween.EaseType.easeInOutSine,"time",0.4f);
        //iTween.RotateTo(LevelManager.currentBubbleWaiting,new Vector3(0, 0,120), 0.6f);
        
        if (LevelManager.bubbleListStillAliveIndex.Count > 0)
        {
            if (countbubbleShoot < bubbleListWillShoot.size-1)
            {
                 value = bubbleListWillShoot[countbubbleShoot +1];
                GamePlay.instance.BubblePre.GetComponent<Bubble>().setValue(value);
            }
            else
            {
                int special = Random.Range(0, 20);
                int index = 0;

                if (special >= 19)
                {
                    index = 25;
                    GamePlay.instance.BubblePre.GetComponent<Bubble>().setValue(index);
                }
                else if (special >= 18)
                {
                    index = 26;
                    GamePlay.instance.BubblePre.GetComponent<Bubble>().setValue(index);
                }
                else if (special >= 17)
                {
                    index = 29;
                    GamePlay.instance.BubblePre.GetComponent<Bubble>().setValue(index);
                }                    
                else
                {
                    index = Random.Range(0, LevelManager.bubbleListStillAliveIndex.Count);
                    GamePlay.instance.BubblePre.GetComponent<Bubble>().setValue((int)(LevelManager.bubbleListStillAliveIndex[index]));
                }
                GamePlay.instance.BubblePre.GetComponent<Bubble>().anim.Play("BONE");
            }

        }
    }
    public static  int changeValue(int _value)
    {
        bool isChange = false;
        if (_value != 25 && _value != 26 && _value != 29)//bong cau bong
        {
            isChange = true;
            for (int i = 0; i < LevelManager.bubbleListStillAliveIndex.Count; i++)
            {
                if (_value == (int)(LevelManager.bubbleListStillAliveIndex[i]))
                    isChange = false;
            }
        }

        if (isChange)
        {
            int index = Random.Range(0, LevelManager.bubbleListStillAliveIndex.Count);
            // Debug.Log("Change Change : " + LevelManager.bubbleListStillAliveIndex.Count + "," + index);
            _value = (int)(LevelManager.bubbleListStillAliveIndex[index]);

        }
        return _value;

    }
}