using UnityEngine;
using System.Collections;

public class ScoreControl : MonoBehaviour {

    public static string UserName = "NaN";
    public static int Score = 0;
    public static int Coin = 0;	
    public static int MAX_LEVEL = 118;
    public static int[] starArray = new int[118];
    public static int mUnblockLevel = 0;
    public static int mBoom = 0;
    public static int mPush = 0;
    public static int mThrough = 0;

	public static string STRING_USER_NAME ="USER_NAME";
    public static string STRING_COIN = "USER_COIN";	
    public static string STRING_UNBLOCK_LEVEL = "UNBLOCK_LEVEL";	
	public static string STRING_LEVEL_STAR = "LEVEL_STAR";
    public static string STRING_BOOM = "SAVE_BOOM";
    public static string STRING_PUSH = "SAVE_PUSH";
    public static string STRING_THROUGH = "SAVE_THROUGH";	
	// Use this for initialization
	public static void saveGame()
	{        
		PlayerPrefs.SetString(STRING_USER_NAME, UserName);
        PlayerPrefs.SetInt(STRING_COIN, Coin);
        PlayerPrefs.SetInt(STRING_UNBLOCK_LEVEL, mUnblockLevel);
        PlayerPrefsX.SetIntArray(STRING_LEVEL_STAR,starArray);
        PlayerPrefs.SetInt(STRING_BOOM, mBoom);
        PlayerPrefs.SetInt(STRING_PUSH, mPush);
        PlayerPrefs.SetInt(STRING_THROUGH, mThrough);
		PlayerPrefs.Save();
        
	}
	public static void loadGame()
	{
		//PlayerPrefs.DeleteAll();
		UserName = PlayerPrefs.GetString(STRING_USER_NAME);
        Coin = PlayerPrefs.GetInt(STRING_COIN);
        mUnblockLevel = PlayerPrefs.GetInt(STRING_UNBLOCK_LEVEL);
        starArray = PlayerPrefsX.GetIntArray(STRING_LEVEL_STAR);
        if (starArray.Length <10)
            starArray = new int[120];
        mBoom = PlayerPrefs.GetInt(STRING_BOOM);
        mPush = PlayerPrefs.GetInt(STRING_PUSH);
        mThrough = PlayerPrefs.GetInt(STRING_THROUGH);

        if (mUnblockLevel < 1)
        {
            mBoom = 3;
            mPush = 3;
            mThrough = 3;
            mUnblockLevel = 1;
        }
       // mUnblockLevel = 50;
		if (UserName.Length <= 4)
						UserName = "NaN";	
	}
    public static void saveCoin()
    {
        PlayerPrefs.SetInt(STRING_COIN, Coin);
        PlayerPrefs.Save();
    }
}
