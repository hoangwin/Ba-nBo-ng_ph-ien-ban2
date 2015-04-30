using UnityEngine;
using System.Collections;

public class TN
{

	public static float DISPLAY_RATIO;
	public static bool IS_INIT = false;
	//public static float ADMOD_WIDTH=320;
	//public static float ADMOD_HEIGHT=50;

	public static float HEIGHT =320;
	public static float WIDTH =320;
	public static float HEIGHT_HALF =320;
	public static float WIDTH_HALF =320;
	public static float WIDTH_HALF_MINUS;
	public static Sounds AUDIOS;

	public static void Init()
	{
		TO.Init ();
		if (IS_INIT == true)
						return;
		float DISPLAY_WIDTH = Screen.width/100.0f;
		float DISPLAY_HEIGHT = Screen.height/100.0f;
		DISPLAY_RATIO = DISPLAY_WIDTH / DISPLAY_HEIGHT;
		float ratio = Screen.dpi*1.0f / 160;
		if (ratio == 0) ratio = 1;
		WIDTH = 320 * DISPLAY_RATIO;
		HEIGHT = 320;
		WIDTH_HALF = WIDTH / 2;
		WIDTH_HALF_MINUS = -WIDTH_HALF;
		HEIGHT_HALF = HEIGHT / 2;
		IS_INIT = true;

		//Debug.Log (WIDTH);

	}
	public static void ReSizeSprite(UISprite s,float width,float height)
	{
		s.transform.localScale = new Vector3 (width/ (s.localSize.x),height/ (s.localSize.y), 1);

	}
	public static Vector2 Pixel2NGUI(Vector2 c)
	{
		Vector2 pos = new Vector2 ();
		pos.x = c.x;
		pos.y = c.y;
		float ratio = TO.DISPLAY_HEIGHT / 320;
		pos.x = (pos.x - TO.DISPLAY_WIDTH_HALF)/ratio;
		pos.y = (pos.y - TO.DISPLAY_HEIGHT_HALF)/ratio;
		return pos;
	}
}

