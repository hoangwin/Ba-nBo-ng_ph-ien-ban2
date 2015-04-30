using UnityEngine;
using System.Collections;

public class SelectLevelScene : MonoBehaviour
{

    public static SelectLevelScene instance;
    public static int currentpage = 1;
    public static int MAX_PAGE = 32;
    public Sprite Star0;
    public Sprite Star1;
    public Sprite Star2;
    public Sprite Star3;
    public Sprite buttonDisable;

    public UILabel LabelScore;
    public UILabel LabelCoin;

    public GameObject button5;
    public GameObject scrollView;
    public GameObject StarAnim;

    public GameObject GameButton;
    public GameObject GameShop;

    float dy = 350f;
    float y0 = -420f;
	void Start () {
		DEF.Init();
		DEF.ScaleAnchorGui();		
		instance = this;
     //   currentpage = ScoreControl.mUnblockLevel / 20 + 1;
     //   if (currentpage > 34) currentpage = 34;       
          for (int i = 0; i < 24; i++)
        {
            GameObject g = Instantiate(button5) as GameObject;
            g.SetActive(true);
            g.transform.parent = scrollView.transform;
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = new Vector3(0, y0 + i * dy, 0);
            SelectLevels s = g.GetComponent<SelectLevels>();
            s.Init(i * 5);
        }
          LabelCoin.text = ScoreControl.Coin.ToString();
          LabelScore.text = ScoreControl.Score.ToString();
          ShopManager.instance = GameShop.GetComponent<ShopManager>();
        
	}
    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
            if (GameShop.activeSelf == true)
            {
                GameButton.SetActive(true);
                GameShop.SetActive(false);
            }
            else
            {
                Application.LoadLevel("MainMenu");
            }
			
		}
	}
	

}
