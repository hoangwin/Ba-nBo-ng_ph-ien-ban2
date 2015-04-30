using UnityEngine;
using System.Collections;

public class SelectLevelButton : MonoBehaviour {

	// Use this for initialization
    public UILabel my_label;
    public UI2DSprite _UI2DSpriteStar;
    public UI2DSprite _UI2DSpriteBG;
    public int INDEX;
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnClick()
    {
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundclick);
        Debug.Log("my id=" + INDEX);
        LevelManager.currentLevel = INDEX;
        SelectLevelScene.instance.GameShop.SetActive(true);
        SelectLevelScene.instance.GameButton.SetActive(false);
        if (ShopManager.instance == null)
            ShopManager.instance = SelectLevelScene.instance.GameShop.GetComponent<ShopManager>();
        ShopManager.instance.LabelTitle.text = "Màn " + LevelManager.currentLevel.ToString();
        ShopManager.instance.init();
    }
}
