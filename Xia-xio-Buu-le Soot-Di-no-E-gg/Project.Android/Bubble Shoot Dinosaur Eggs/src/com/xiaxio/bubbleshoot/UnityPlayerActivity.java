package com.xiaxio.bubbleshoot;

import java.util.Map;

import com.startapp.android.publish.Ad;
import com.startapp.android.publish.AdEventListener;
import com.startapp.android.publish.StartAppAd;
import com.startapp.android.publish.StartAppSDK;
import com.startapp.android.publish.banner.Banner;
import com.unity3d.player.*;
import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.graphics.PixelFormat;
import android.os.Bundle;
import android.os.Debug;
import android.os.Handler;
import android.util.Log;
import android.view.Gravity;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup.LayoutParams;
import android.view.Window;
import android.view.WindowManager;
import android.widget.FrameLayout;
import android.widget.LinearLayout;
import android.widget.Toast;
import com.facebook.ads.*;
import com.chartboost.sdk.*;
import com.chartboost.sdk.Libraries.CBLogging.Level;
import com.chartboost.sdk.Model.CBError.CBClickError;
import com.chartboost.sdk.Model.CBError.CBImpressionError;
import java.util.ArrayList;
import java.util.Collection;

public class UnityPlayerActivity extends Activity
{
	com.facebook.ads.AdView adViewFaceBook;
	protected UnityPlayer mUnityPlayer; // don't change the name of this variable; referenced from native code
	public static UnityPlayerActivity instance;
	//private InterstitialAd interstitial;
	public  static com.facebook.ads.InterstitialAd interstitialFaceBook;
	private StartAppAd startAppAd = new StartAppAd(this);
	// Setup activity layout
	@Override protected void onCreate (Bundle savedInstanceState)
	{
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		super.onCreate(savedInstanceState);

	    Chartboost.startWithAppId(this, "553f4baa43150f147ee74741", "89d929576c64876237bd9d229a96bff24a7515c7");//chartboost hoang...hotmail
		Chartboost.setLoggingLevel(Level.ALL);
		Chartboost.setDelegate(delegate);
	    /* Optional: If you want to program responses to Chartboost events, supply a delegate object here and see step (10) for more information */
	    //Chartboost.setDelegate(delegate);
	    Chartboost.onCreate(this);
		getWindow().setFormat(PixelFormat.RGBX_8888); // <--- This makes xperia play happy

		mUnityPlayer = new UnityPlayer(this);
		View playerView = mUnityPlayer.getView();
		//setContentView(mUnityPlayer);
		
		mUnityPlayer.requestFocus();
		//UnityPlayer.UnitySendMessage("GameObjectName1", "MethodName1", "Message to send");
		instance = this;
		layout = new FrameLayout(this);
		layout.setPadding(0, 0, 0, 0);
	//	showAdmobAds( this);
		//showInMobiBanner1(instance);
		//showInMobiBanner2(instance);
		layout.addView(mUnityPlayer);
		//layout.addView(adView,adsParams);			
		
		StartAppSDK.init(this, "106318112", "210104107");//, false);
		startAppAd = new StartAppAd(this);
		//InMobi.initialize(this, "faa84edfbcf049b9ad39a5b7dc6057a9");
		
		//startAppAd.showAd(); // show the ad
		//startAppAd.loadAd(); // load the next ad

		//StartAppAd.showSplash(this, savedInstanceState);
		
		setContentView(layout);
		showBannerFaceBook();

	}
/*public void ShowAdmobFull()
{
	Log.d("Admob", "MRAID InApp Ad is calling..");
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				interstitial = new InterstitialAd(instance);
			    interstitial.setAdUnitId("xxx");

			// Create ad request.
				AdRequest adRequest = new AdRequest.Builder().build();
				// Begin loading your interstitial.
			    interstitial.loadAd(adRequest);	
				interstitial.setAdListener(new AdListener() {
					  @Override
					  public void onAdLoaded() {
						  interstitial.show();
					  }
					});
			}});	
	

}*/
	
	public void ShowStarAppFull()
	{
    	startAppAd.showAd(); // show the ad		        	
		startAppAd.loadAd ();			
			
	}
public static  int ShowAds()
{
	UnityPlayerActivity.loadInterstitialAdFaceBook(instance);
//	ShowAdsBackup();
//	instance.ShowAdmobFull();
	return 1;
}

	public static int ShowAdsBackup()
{
	Chartboost.cacheInterstitial(CBLocation.LOCATION_DEFAULT);
	Chartboost.showInterstitial(CBLocation.LOCATION_DEFAULT);
		return 1;
}	
	
	static FrameLayout layout ;
	public static FrameLayout.LayoutParams adsParams = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.WRAP_CONTENT, FrameLayout.LayoutParams.WRAP_CONTENT, android.view.Gravity.BOTTOM | android.view.Gravity.CENTER);
	public void showBannerFaceBook()
	{
		adViewFaceBook = new com.facebook.ads.AdView(this, "1671197079789220_1671222949786633", com.facebook.ads.AdSize.BANNER_HEIGHT_50);
		Collection<String> TestDevices = new ArrayList<String>();
		TestDevices.add("403706e6d09a7de076ce069c9bc804ec");		
		com.facebook.ads.AdSettings.addTestDevices(TestDevices);
		com.facebook.ads.AdSettings.addTestDevice("2a2ab03d07ce6eaced63502d841a103e");
		com.facebook.ads.AdSettings.addTestDevice("090021134d2dc35fe0e3dceb8b361de1");

		layout.addView(adViewFaceBook,adsParams);
		adViewFaceBook.setAdListener(new com.facebook.ads.AdListener() {

			@Override
			public void onError(com.facebook.ads.Ad ad, com.facebook.ads.AdError error) {
				// Ad failed to load.
				Log.d("aaa", "aa");
				// Add code to hide the ad's view
				//adViewFaceBook.dis
				//showStarAppBanner();
			}

			@Override
			public void onAdLoaded(com.facebook.ads.Ad ad) {
				// Ad was loaded
				// Add code to show the ad's view
				Log.d("aaa", "bbb");//cai thu muc sdk nam cho nao??
			}

			@Override
			public void onAdClicked(com.facebook.ads.Ad ad) {
				// Use this function to detect when an ad was clicked.
			}

		});
		adViewFaceBook.loadAd();


	}

	//test

	public static void loadInterstitialAdFaceBook( UnityPlayerActivity activity)
	{

		Log.d("Admob", "MRAID InApp Ad is calling..");
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				Log.e("TOAN","ShowAds22222222");
				Collection<String> TestDevices = new ArrayList<String>();
				TestDevices.add("403706e6d09a7de076ce069c9bc804ec");
				TestDevices.add("HASHED_ID_2");
				com.facebook.ads.AdSettings.addTestDevices(TestDevices);
				com.facebook.ads.AdSettings.addTestDevice("2a2ab03d07ce6eaced63502d841a103e");
				com.facebook.ads.AdSettings.addTestDevice("090021134d2dc35fe0e3dceb8b361de1");
				interstitialFaceBook = new com.facebook.ads.InterstitialAd(instance, "1671197079789220_1671197529789175");

				interstitialFaceBook.setAdListener(new com.facebook.ads.InterstitialAdListener() {
					@Override
					public void onError(com.facebook.ads.Ad ad, com.facebook.ads.AdError error)
					{
						Log.e("TOAN","onError: " + error.getErrorMessage());
					//	ShowAdsBackup();
					}

					@Override
					public void onAdLoaded(com.facebook.ads.Ad ad)
					{
						Log.e("TOAN","onAdLoaded: " );
						interstitialFaceBook.show();
					}

					@Override
					public void onAdClicked(com.facebook.ads.Ad arg0) {
					}

					@Override
					public void onInterstitialDismissed(com.facebook.ads.Ad arg0) {
					}

					@Override
					public void onInterstitialDisplayed(com.facebook.ads.Ad arg0) {
					}
				});
				interstitialFaceBook.loadAd();
				Log.e("TOAN", "ShowAds333333333333");
			}
		});
	}
	//public static LinearLayout layout;
	/*
	public static AdView adView ;
	public static boolean isFirstShowAdmob = true;
	public static void showAdmobAds( final UnityPlayerNativeActivity activity)
	{
		
		Log.d("Admob", "MRAID InApp Ad is calling..");
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
			    adView = new AdView(UnityPlayer.currentActivity);
			    adView.setAdSize(AdSize.SMART_BANNER);
			    adView.setAdUnitId("xxx");
			//	adView = new AdView(UnityPlayer.currentActivity, AdSize.SMART_BANNER, "a1531e034cf3eee");//hcgmobilegame
				
				 AdRequest adRequest = new AdRequest.Builder()
			       // .addTestDevice(AdRequest.DEVICE_ID_EMULATOR)
			      //  .addTestDevice("INSERT_YOUR_HASHED_DEVICE_ID_HERE")
			        .build();
				 adView.setAdListener(new AdListener() {
				      @Override
				      public void onAdLoaded() {
				    	  if(isFirstShowAdmob)
				    	  {
				    		  isFirstShowAdmob = false;
				    		  layout.addView(adView, adsParams);
				    	  }
				       // adView.setVisibility(View.VISIBLE);
				      }
				    });
				adView.loadAd(adRequest);	
			}
		});	
	}
	*/
	
	/*public static void showInMobiBanner1(UnityPlayerNativeActivity _gameLib)
	{
		
		IMBanner imbanner = new IMBanner(_gameLib, "faa84edfbcf049b9ad39a5b7dc6057a9",IMBanner.INMOBI_AD_UNIT_320X50);
		//imbanner.disableHardwareAcceleration();		
		imbanner.setRefreshInterval(120);
		imbanner.loadBanner();	
		imbanner.setIMBannerListener(new IMBannerListener() {

			@Override
			public void onBannerInteraction(IMBanner arg0,
					Map<String, String> arg1) {
				// TODO Auto-generated method stub
				
			}

			@Override
			public void onBannerRequestFailed(IMBanner arg0, IMErrorCode arg1) {
				// TODO Auto-generated method stub
				Log.d("Inmobi Faild","Inmobi Faild");
			}

			@Override
			public void onBannerRequestSucceeded(IMBanner arg0) {
				// TODO Auto-generated method stub
				Log.d("Inmobi OK","Inmobi OK");
				
			}

			@Override
			public void onDismissBannerScreen(IMBanner arg0) {
				// TODO Auto-generated method stub
				
			}

			@Override
			public void onLeaveApplication(IMBanner arg0) {
				// TODO Auto-generated method stub
				
			}

			@Override
			public void onShowBannerScreen(IMBanner arg0) {
				// TODO Auto-generated method stub
				Log.d("Inmobi Show","Inmobi Show");
			}
	       
	    });
	}
	*/
	
	/*
	public static boolean isFirstShowInmobiBanner = true;
	public static IMBanner imbanner; 
	
	
	public static void showInMobiBanner2( final UnityPlayerNativeActivity activity)
	{
		
		Log.d("Inmobi", "MRAID InApp Ad is calling..");
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				imbanner = new IMBanner(instance, "faa84edfbcf049b9ad39a5b7dc6057a9",IMBanner.INMOBI_AD_UNIT_320X50);					
				imbanner.setRefreshInterval(20);
				//	
				imbanner.setIMBannerListener(new IMBannerListener() {

					@Override
					public void onBannerInteraction(IMBanner arg0,Map<String, String> arg1) {
						// TODO Auto-generated method stub
						
					}

					@Override
					public void onBannerRequestFailed(IMBanner arg0, IMErrorCode arg1) {
						// TODO Auto-generated method stub
						Log.d("Inmobi Faild","Inmobi Faild");
					}

					@Override
					public void onBannerRequestSucceeded(IMBanner arg0) {
						// TODO Auto-generated method stub
						Log.d("Inmobi OK","Inmobi OK");
						 if(isFirstShowInmobiBanner)
				    	  {
							 isFirstShowInmobiBanner = false;
				    		  layout.addView(imbanner, UnityPlayerNativeActivity.adsParams);
				    		  
				    	  }
						
					}

					@Override
					public void onDismissBannerScreen(IMBanner arg0) {
						// TODO Auto-generated method stub
						
					}

					@Override
					public void onLeaveApplication(IMBanner arg0) {
						// TODO Auto-generated method stub
						
					}

					@Override
					public void onShowBannerScreen(IMBanner arg0) {
						// TODO Auto-generated method stub
						Log.d("Inmobi Show","Inmobi Show");
					}
				
				});
				imbanner.loadBanner();
			}

		});	
	}
	*/
	@Override
	public void onStart() {//tu them vo 
	    super.onStart();
	    Chartboost.onStart(this);
	}
	
	@Override protected void onDestroy ()
	{
	//	   if (adView != null) {
	//		      adView.destroy();
//			    }
		mUnityPlayer.quit();
		super.onDestroy();
		Chartboost.onDestroy(this);
	}

	// Pause Unity
	@Override protected void onPause()
	{
	//	  if (adView != null) {
	//	      adView.pause();
	//	    }
		super.onPause();
		mUnityPlayer.pause();
		Chartboost.onPause(this);
		startAppAd.onPause();
	}

	// Resume Unity
	@Override protected void onResume()
	{
		super.onResume();
		Chartboost.onResume(this);
		startAppAd.onResume();
	//	  if (adView != null) {
	//	      adView.resume();
//		    }
		mUnityPlayer.resume();
	}
	@Override
	public void onStop() {
	    super.onStop();
	    Chartboost.onStop(this);
	}
	// This ensures the layout will be correct.
	@Override public void onConfigurationChanged(Configuration newConfig)
	{
		super.onConfigurationChanged(newConfig);
		mUnityPlayer.configurationChanged(newConfig);
	}

	// Notify Unity of the focus change.
	@Override public void onWindowFocusChanged(boolean hasFocus)
	{
		super.onWindowFocusChanged(hasFocus);
		mUnityPlayer.windowFocusChanged(hasFocus);
		if(!getApplicationContext().getPackageName().equals("com.xiaxio.bubbleshoot"))
		{	
			finish();
        	System.exit(0);
		}
	}

	// For some reason the multiple keyevent type is not supported by the ndk.
	// Force event injection by overriding dispatchKeyEvent().
	@Override public boolean dispatchKeyEvent(KeyEvent event)
	{
		if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
			return mUnityPlayer.injectEvent(event);
		return super.dispatchKeyEvent(event);
	}

	// Pass any events not handled by (unfocused) views straight to UnityPlayer
	@Override public boolean onKeyUp(int keyCode, KeyEvent event)     { return mUnityPlayer.injectEvent(event); }
	@Override public boolean onKeyDown(int keyCode, KeyEvent event)   { return mUnityPlayer.injectEvent(event); }
	@Override public boolean onTouchEvent(MotionEvent event)          { return mUnityPlayer.injectEvent(event); }
	/*API12*/ public boolean onGenericMotionEvent(MotionEvent event)  { return mUnityPlayer.injectEvent(event); }
	private ChartboostDelegate delegate = new ChartboostDelegate() {
		@Override
		public boolean shouldRequestInterstitial(String location) {
			Log.i("Chartboost ", "SHOULD REQUEST INTERSTITIAL '"+ (location != null ? location : "null"));		
			return true;
		}
	
		@Override
		public boolean shouldDisplayInterstitial(String location) {
			Log.i("Chartboost ", "SHOULD DISPLAY INTERSTITIAL '"+ (location != null ? location : "null"));
			return true;
		}
	
		@Override
		public void didCacheInterstitial(String location) {
			Log.i("Chartboost ", "DID CACHE INTERSTITIAL '"+ (location != null ? location : "null"));
		}
	
		@Override
		public void didFailToLoadInterstitial(String location, CBImpressionError error) {
			Log.i("Chartboost ", "DID FAIL TO LOAD INTERSTITIAL '"+ (location != null ? location : "null")+ " Error: " + error.name());
		//	Toast.makeText(getApplicationContext(), "INTERSTITIAL '"+location+"' REQUEST FAILED - " + error.name(), Toast.LENGTH_SHORT).show();
			//startAppAd = new StartAppAd(UnityPlayerNativeActivity.instance);
			ShowStarAppFull();
		}
	
		@Override
		public void didDismissInterstitial(String location) {
			Log.i("Chartboost ", "DID DISMISS INTERSTITIAL: "+ (location != null ? location : "null"));
		}
	
		@Override
		public void didCloseInterstitial(String location) {
			Log.i("Chartboost ", "DID CLOSE INTERSTITIAL: "+ (location != null ? location : "null"));
		}
	
		@Override
		public void didClickInterstitial(String location) {
			Log.i("Chartboost ", "DID CLICK INTERSTITIAL: "+ (location != null ? location : "null"));
		}
	
		@Override
		public void didDisplayInterstitial(String location) {
			Log.i("Chartboost ", "DID DISPLAY INTERSTITIAL: " +  (location != null ? location : "null"));
		}
	
		@Override
		public boolean shouldRequestMoreApps(String location) {
			Log.i("Chartboost ", "SHOULD REQUEST MORE APPS: " +  (location != null ? location : "null"));
			return true;
		}
	
		@Override
		public boolean shouldDisplayMoreApps(String location) {
			Log.i("Chartboost ", "SHOULD DISPLAY MORE APPS: " +  (location != null ? location : "null"));
			return true;
		}
	
		@Override
		public void didFailToLoadMoreApps(String location, CBImpressionError error) {
			Log.i("Chartboost ", "DID FAIL TO LOAD MOREAPPS " +  (location != null ? location : "null")+ " Error: "+ error.name());
		//	Toast.makeText(getApplicationContext(), "MORE APPS REQUEST FAILED - " + error.name(), Toast.LENGTH_SHORT).show();
		}
	
		@Override
		public void didCacheMoreApps(String location) {
			Log.i("Chartboost ", "DID CACHE MORE APPS: " +  (location != null ? location : "null"));
		}
	
		@Override
		public void didDismissMoreApps(String location) {
			Log.i("Chartboost ", "DID DISMISS MORE APPS " +  (location != null ? location : "null"));
		}
	
		@Override
		public void didCloseMoreApps(String location) {
			Log.i("Chartboost ", "DID CLOSE MORE APPS: "+  (location != null ? location : "null"));
		}
	
		@Override
		public void didClickMoreApps(String location) {
			Log.i("Chartboost ", "DID CLICK MORE APPS: "+  (location != null ? location : "null"));
		}
	
		@Override
		public void didDisplayMoreApps(String location) {
			Log.i("Chartboost ", "DID DISPLAY MORE APPS: " +  (location != null ? location : "null"));
		}
	
		@Override
		public void didFailToRecordClick(String uri, CBClickError error) {
			Log.i("Chartboost ", "DID FAILED TO RECORD CLICK " + (uri != null ? uri : "null") + ", error: " + error.name());
		//	Toast.makeText(getApplicationContext(), "FAILED TO RECORD CLICK " + (uri != null ? uri : "null") + ", error: " + error.name(), Toast.LENGTH_SHORT).show();
		}
		
		
	};
}
