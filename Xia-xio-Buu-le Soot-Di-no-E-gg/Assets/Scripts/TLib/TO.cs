using UnityEngine;
using System.Collections;

public class TO
{
	public static float DISPLAY_WIDTH;
	public static float DISPLAY_HEIGHT;
	public static float DISPLAY_WIDTH_HALF;
	public static float DISPLAY_HEIGHT_HALF;
	public static float DISPLAY_RATIO;
	public static float DISPLAY_WIDTH_ORTHO;
	public static float DISPLAY_HEIGHT_ORTHO;
	public static float DISPLAY_WIDTH_ORTHO_HALF;
	public static float DISPLAY_HEIGHT_ORTHO_HALF;
	public static float DISPLAY_RATIO_ORTHOR_X;
	public static float DISPLAY_RATIO_ORTHOR_Y;
	//public static float MINX,MINY,MAXX,MAXY;
	public static bool IS_INIT = false;


	//public static float ADMOD_WIDTH=320;
	//public static float ADMOD_HEIGHT=50;

	//public static float NGUI_WIDTH =320;
	//public static Sounds AUDIOS;

	public static void Init()
	{
		if (IS_INIT == true)
						return;
		DISPLAY_WIDTH = Screen.width;
		DISPLAY_HEIGHT = Screen.height;
		DISPLAY_WIDTH_HALF = DISPLAY_WIDTH / 2;
		DISPLAY_HEIGHT_HALF = DISPLAY_HEIGHT / 2;
		DISPLAY_RATIO = DISPLAY_WIDTH / DISPLAY_HEIGHT;
		DISPLAY_HEIGHT_ORTHO = 2;
		DISPLAY_WIDTH_ORTHO = DISPLAY_HEIGHT_ORTHO * DISPLAY_RATIO;
		DISPLAY_RATIO_ORTHOR_X = DISPLAY_WIDTH_ORTHO / DISPLAY_WIDTH;
		DISPLAY_RATIO_ORTHOR_Y = DISPLAY_HEIGHT_ORTHO / DISPLAY_HEIGHT;
		DISPLAY_WIDTH_ORTHO_HALF = DISPLAY_WIDTH_ORTHO / 2;
		DISPLAY_HEIGHT_ORTHO_HALF = DISPLAY_HEIGHT_ORTHO / 2;

		
		float ratio = Screen.dpi*1.0f / 160;
		if (ratio == 0) ratio = 1;
		//ADMOD_WIDTH *= ratio;
		//ADMOD_HEIGHT *= ratio;

		//Debug.Log("SIZE " + DISPLAY_WIDTH+" " +DISPLAY_HEIGHT);
		//Debug.Log("OTHOR " +DISPLAY_WIDTH_ORTHO +" " +DISPLAY_HEIGHT_ORTHO);
		//NGUI_WIDTH = 320 * DISPLAY_RATIO;
		IS_INIT = true;

	}
	public static Vector3 Pixel2Othor(Vector3 vec)
	{
		return new Vector3 (vec.x *DISPLAY_RATIO_ORTHOR_X,vec.y *DISPLAY_RATIO_ORTHOR_Y,vec.z );
	}
	//public static Vector3 Pixel2Othor(float x,float y)
	//{
		//return new Vector3 (x *DISPLAY_RATIO_ORTHOR_X,y *DISPLAY_RATIO_ORTHOR_Y,0 );
	//}
	//public static Vector2 Pixel2OthorVec2(float x,float y)
	//{
		//return new Vector2 (x *DISPLAY_RATIO_ORTHOR_X,y *DISPLAY_RATIO_ORTHOR_Y );
	//}
	public static Vector3 Pixel2Othor(float x,float y,float z)
	{
		return new Vector3 (x *DISPLAY_RATIO_ORTHOR_X,y *DISPLAY_RATIO_ORTHOR_Y,z );
	}
	public static Vector3 Pixel2OthorAmDuong(Vector3 vec)
	{
		return new Vector3 (vec.x *DISPLAY_RATIO_ORTHOR_X - DISPLAY_WIDTH_ORTHO_HALF,vec.y *DISPLAY_RATIO_ORTHOR_Y-DISPLAY_HEIGHT_ORTHO_HALF,vec.z );
	}
	public static Vector2 Pixel2OthorAmDuongVec2(float x,float y)
	{
		return new Vector2 (x *DISPLAY_RATIO_ORTHOR_X - DISPLAY_WIDTH_ORTHO_HALF,y *DISPLAY_RATIO_ORTHOR_Y-DISPLAY_HEIGHT_ORTHO_HALF );
	}
	public static Vector2 Pixel2PixelAmDuongVec2(float x,float y)
	{
		return new Vector2 (x - DISPLAY_WIDTH_HALF,y -DISPLAY_HEIGHT_HALF );
	}
}

