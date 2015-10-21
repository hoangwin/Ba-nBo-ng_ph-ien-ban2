using UnityEngine;
using System.Collections;

public class SoundEngine : MonoBehaviour {
	public static bool isSoundSFX = true;
    public static bool isSoundMusic = true;
    
    public AudioClip _soundBG1 = null;
    public AudioClip _soundBG2 = null;

    public AudioClip _soundclick = null;

    public AudioClip _soundReadyGo = null;    

    public AudioClip _soundBone = null;
    public AudioClip _soundShoot = null;
    public AudioClip _soundStick = null;
    public AudioClip _soundDrop = null;
    public AudioClip _soundDestroy = null;

    public AudioClip _soundBoom = null;
    public AudioClip _soundPush = null;
    public AudioClip _soundThrough = null;

    public AudioClip _soundCoin = null;    
    public AudioClip _soundStar = null;
    public AudioClip _soundDiamond = null;

    public AudioClip _soundDrop1 = null;
    public AudioClip _soundDrop2 = null;
    public AudioClip _soundDrop3 = null;
    public AudioClip _soundDrop4 = null;
    public AudioClip _soundDrop5 = null;
    public AudioClip _soundDrop6 = null;
    public AudioClip _soundDrop7 = null;
    public AudioClip _soundDrop8 = null;


    public AudioClip _soundLose = null;
    public AudioClip _soundWin = null;
    //public AudioClip _soundShoot = null;
    
    public static SoundEngine instance;
    void Start()
    {
        if (instance != null)
        {
            Debug.Log("Destroy This");
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        //this.gameObject.
        SoundEngine.getInstance().PlayLoop(SoundEngine.getInstance()._soundBG1);
    }
    public static SoundEngine getInstance()
    {
        if(instance == null)
        {
            instance = new SoundEngine();
        }
        return instance;
    }
    public void PlayOneShot(AudioClip e)
    {

        if (e == null)
            return;
       // if (!e..isPlaying)
            if (isSoundSFX)
            {
                GetComponent<AudioSource>().PlayOneShot(e);
            }
    }
    // Update is called once per frame
    public void PlayLoop(AudioClip e)
    {
        
        if (isSoundMusic)
        {
            if (GetComponent<AudioSource>() != null && e != null)
            {
                GetComponent<AudioSource>().clip = e;
                GetComponent<AudioSource>().loop = true;
                if (!GetComponent<AudioSource>().isPlaying)
                    GetComponent<AudioSource>().Play();
            }
        }
    }
    public void stopSound()
    {
        GetComponent<AudioSource>().Stop();
    }

	// Update is called once per frame
	void Update () {
	
	}

	
}
