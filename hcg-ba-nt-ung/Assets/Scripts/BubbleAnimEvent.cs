using UnityEngine;
using System.Collections;

public class BubbleAnimEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void setDestroy()
    {
        GameObject.Destroy(transform.parent.gameObject.GetComponent<BubbleAnimEffect>().linkBubbleObject);
        GameObject.Destroy(this.gameObject);

        ScoreControl.Score += 100;
        GamePlay.instance.LabelScore.text = ScoreControl.Score.ToString();
        if (GamePlay.currentState == GamePlay.STATE_WIN)
        {
            GameObject.Find("LabelScoreWin").GetComponent<UILabel>().text = ScoreControl.Score.ToString();
        }

    }
}
