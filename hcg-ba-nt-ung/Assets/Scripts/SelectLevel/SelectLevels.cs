using UnityEngine;
using System.Collections;

public class SelectLevels : MonoBehaviour
{
    public SelectLevelButton[] Lavel; 
	void Start () 
    {
	
	}
	void Update () 
    {
	
	}
    public void Init(int index_start)
    {
        for (int i = 0; i < 5; i++)
        {
            Lavel[i].INDEX = i + index_start + 1;
//            Debug.Log(Lavel[i].INDEX);
            if (Lavel[i].INDEX > 118)
            {
                Lavel[i].gameObject.SetActive(false);
            }
            else
            {
                
                if (Lavel[i].INDEX > ScoreControl.mUnblockLevel)
                {
                    Lavel[i].my_label.text = "";// (i + index_start + 1).ToString();
                    //Lavel[i]._UI2DSpriteBG.sprite2D = SelectLevelScene.instance.buttonDisable;
                    //Lavel[i].GetComponent<UIButton>().normalSprite2D = SelectLevelScene.instance.buttonDisable;
                    Lavel[i].GetComponent<UIButton>().isEnabled = false;
                    Lavel[i]._UI2DSpriteStar.sprite2D = SelectLevelScene.instance.Star0;
                }
                else if (Lavel[i].INDEX == ScoreControl.mUnblockLevel)
                {
                    Lavel[i].my_label.text = (i + index_start + 1).ToString();

                    Lavel[i]._UI2DSpriteStar.sprite2D = SelectLevelScene.instance.Star0;
                    //SelectLevelScene.instance.GameButton.transform.position = new Vector3(0, -8 + *1, 0);
                    SelectLevelScene.instance.GameButton.GetComponent<UIScrollView>().MoveRelative(new Vector3(0, -((Lavel[i].INDEX -1) / 5)*300, 0));//Lavel[i].transform.position);
                    //SelectLevelScene.instance.StarAnim.transform.position = Lavel[i].transform.position;
                }
                else
                {
                    Lavel[i].my_label.text = (i + index_start + 1).ToString();

                    if (ScoreControl.starArray[Lavel[i].INDEX - 1] == 1)
                        Lavel[i]._UI2DSpriteStar.sprite2D = SelectLevelScene.instance.Star1;
                    else if (ScoreControl.starArray[Lavel[i].INDEX - 1] == 2)
                        Lavel[i]._UI2DSpriteStar.sprite2D = SelectLevelScene.instance.Star2;
                    else if (ScoreControl.starArray[Lavel[i].INDEX - 1] == 3)
                        Lavel[i]._UI2DSpriteStar.sprite2D = SelectLevelScene.instance.Star3;
                }
            }
        }
    }
}

