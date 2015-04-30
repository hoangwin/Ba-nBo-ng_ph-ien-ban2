using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D orther)
    {
     //  Debug.Log("Tao day ne");
       if (orther.gameObject.name.Contains("BubblePrefab") && GamePlay.currentState == GamePlay.STATE_PLAY)
       {
            GamePlay.TimePlayedSubState = 1f;
            GamePlay.isWin = false;
			GamePlay.changeState(GamePlay.STATE_WAITING_WIN_LOSE);
            SoundEngine.getInstance().PlayOneShot(SoundEngine.getInstance()._soundLose);
		
		}
    }
}
