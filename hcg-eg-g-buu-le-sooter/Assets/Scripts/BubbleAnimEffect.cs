using UnityEngine;
using System.Collections;

public class BubbleAnimEffect : MonoBehaviour
{

    // Use this for initialization
    public GameObject linkBubbleObject;
    public Animator AnimScore;
    public Animator AnimEffect;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /*
    public void setDestroy()
    {

        GameObject.Destroy(linkBubbleObject);
        GameObject.Destroy(this.gameObject);

        ScoreControl.Score += 100;
        GamePlay.instance.LabelScore.text = ScoreControl.Score.ToString();
        if (GamePlay.currentState == GamePlay.STATE_WIN)
        {
            GameObject.Find("LabelScoreWin").GetComponent<UILabel>().text = ScoreControl.Score.ToString();
        }

    }
     */
}
