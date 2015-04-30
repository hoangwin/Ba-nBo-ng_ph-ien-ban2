using UnityEngine;
using System.Collections;

public class MoveCompletedHandle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void CoinMoveBeginCompleted()
    {
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundCoin);
        ScoreControl.Coin += 10;
        GameObject.Destroy(this.gameObject);
        GamePlay.instance.LabelCoin.text = ScoreControl.Coin.ToString();
    }

    public void StarMoveBeginCompleted()
    {
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundStar);
        ScoreControl.Coin += 20;
        ScoreControl.saveCoin();
        GameObject.Destroy(this.gameObject);
        GamePlay.instance.LabelCoin.text = ScoreControl.Coin.ToString();

    }
    public void DiamondMoveBeginCompleted()
    {
        SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundDiamond);
        ScoreControl.Coin += 50;
        GameObject.Destroy(this.gameObject);
        ScoreControl.saveCoin();
        GamePlay.instance.LabelCoin.text = ScoreControl.Coin.ToString();

    }
    public void LightMoveBeginCompleted()
    {

        ScoreControl.mThrough++;
        GamePlay.instance.LabelThrough.text = ScoreControl.mThrough.ToString();
        ScoreControl.saveGame();
        GameObject.Destroy(this.gameObject);


    }
    
}
