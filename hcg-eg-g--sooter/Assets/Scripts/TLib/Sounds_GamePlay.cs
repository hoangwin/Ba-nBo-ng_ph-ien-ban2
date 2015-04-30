using UnityEngine;
using System.Collections;

public class Sounds_GamePlay : MonoBehaviour
{

	public AudioClip eat;
    public AudioClip get_coin;
	public AudioClip new_fish;
	public AudioClip newsence;
    public AudioClip click;
    public AudioClip drop;
	public bool IS_LOAD = false;
    public static Sounds_GamePlay SOUNDS = null;
    //public static SuperInt AUDIO_VALUE = new SuperInt(40, "srtygfvcxgv");
	void Start () 
	{
		//AudioListener.volume = (float)(AUDIO_VALUE.NUM*0.01f);
		if (SOUNDS != null) 
		{
			Destroy(this.gameObject);
			return;
		}
        //AudioListener.volume = AUDIO_VALUE.NUM * 0.01f;
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
			GetComponent<AudioSource>().PlayOneShot (c);
		} catch(MissingComponentException c12)
		{
			Debug.Log("TOAN_STT audio not done loaded yet" + c12.Message);

		}
	}
    float count = 0;
    float count_max = 0.5f;
    public void PLayOneShot2(AudioClip c)
    {
        if (count > 0) return;
        count = Random.Range(0.1f,0.5f);

        try
        {
            GetComponent<AudioSource>().PlayOneShot(c);
        }
        catch (MissingComponentException c12)
        {
            Debug.Log("TOAN_STT audio not done loaded yet" + c12.Message);

        }
    }

	void Update () 
	{
        count -= Time.deltaTime;

	}
}
