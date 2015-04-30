using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

	// Use this for initialization
    public Animator anim;
    public static int countBubbleBegin = 0;
    public static int countBubbleEnd = 0;
    public static float yEffectText = 0;
    public static float timeIceEffect = 0;
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
    public void effectTextPlay(int i)
    {
        anim.Play("Effect_Text" + i.ToString());
    }
    public static void CountBeginBubble(float _yEffectText)
    {
        yEffectText = _yEffectText;
        countBubbleBegin = LevelManager.bubbleList.size;
    }
    public static void GoReadyPlay()
    {
        GamePlay.instance.effectText.gameObject.transform.position = new Vector3(0, 0, 0);// transform.position;
        GamePlay.instance.effectText.anim.Play("Effect_Go");
    }
    public static void CountEndBubble()
    {
        countBubbleEnd = LevelManager.bubbleList.size;
    }
    public static void PlayEffectText()
    {
        CountEndBubble();
        GamePlay.instance.effectText.gameObject.transform.position = new Vector3(0, yEffectText, 0);// transform.position;
        if (!Bubble.isPlaySpecial && countBubbleBegin - countBubbleEnd >= 1)
        {   
          
            
            GamePlay.countOk++;
            if (GamePlay.countOk >= 2 || GamePlay.countFail >= 5)
            {
              //  Debug.Log("aaaaaaaaaaaaa");
                LevelManager.MOVE_OFFSET_UP = 0.3f;
                LevelManager.speedOffSet = LevelManager.SPEED_OFFSET_UP;
                BubbleListParent.speedY = LevelManager.speedOffSet;
            }
            GamePlay.countFail = 0;
//            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundDrop);
            
        }
        else 
        {
            GamePlay.countOk = 0;
            GamePlay.countFail++;
        }


        if (countBubbleBegin - countBubbleEnd >= 12)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundDrop6);
            GamePlay.instance.effectText.effectTextPlay(6);
        }
        else if (countBubbleBegin - countBubbleEnd >= 10)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundDrop5);
            GamePlay.instance.effectText.effectTextPlay(5);                
        }
        else if (countBubbleBegin - countBubbleEnd >= 8)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundDrop4);
            GamePlay.instance.effectText.effectTextPlay(4);
        }
        else if (countBubbleBegin - countBubbleEnd >= 6)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundDrop3);
            GamePlay.instance.effectText.effectTextPlay(3);
        }
        else if (countBubbleBegin - countBubbleEnd >= 4)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundDrop2);
            GamePlay.instance.effectText.effectTextPlay(2);
        }
        else if (countBubbleBegin - countBubbleEnd >= 1)
        {
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundDrop1);
            GamePlay.instance.effectText.effectTextPlay(1);
        }
    }

}
