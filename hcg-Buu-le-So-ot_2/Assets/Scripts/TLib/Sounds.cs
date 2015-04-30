using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	public AudioClip xoc;
	public AudioClip click;
	public AudioClip mo;
	public bool IS_LOAD = false;
	public static Sounds SOUNDS = null;
    public static SuperInt AUDIO_VALUE = new SuperInt(40, "srtygfvcxgv");
	void Start () 
	{
		AudioListener.volume = (float)(AUDIO_VALUE.NUM*0.01f);
		if (SOUNDS != null) 
		{
			Destroy(this.gameObject);
			return;
		}
        AudioListener.volume = AUDIO_VALUE.NUM * 0.01f;
		Object.DontDestroyOnLoad(gameObject);
		SOUNDS = this;

		//audio.PlayOneShot (start);
		IS_LOAD = true;
	}
	public void PLayLoop(AudioClip c)
	{

	}
	public void PLayOneShot(AudioClip c)
	{
		try
		{
			audio.PlayOneShot (c);
		} catch(MissingComponentException c12)
		{
			Debug.Log("TOAN_STT audio not done loaded yet" + c12.Message);

		}
	}

	void Update () 
	{

	}
}
