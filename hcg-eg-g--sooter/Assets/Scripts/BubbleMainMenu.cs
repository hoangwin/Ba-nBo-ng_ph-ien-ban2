using UnityEngine;
using System.Collections;

public class BubbleMainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	public float min;
	public float max;
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
        //SoundEngine.playSound("SoundBubble");
		GetComponent<Rigidbody2D>().velocity = new Vector2(0f,Random.Range(min, max));//
	}
}
