#import "UnityAppController.h"
#import "UnityAppController+ViewHandling.h"
#import "UnityAppController+Rendering.h"
#import "iPhone_Sensors.h"

#import <CoreGraphics/CoreGraphics.h>
#import <QuartzCore/QuartzCore.h>
#import <QuartzCore/CADisplayLink.h>
#import <UIKit/UIKit.h>
#import <Availability.h>

#import <OpenGLES/EAGL.h>
#import <OpenGLES/EAGLDrawable.h>
#import <OpenGLES/ES2/gl.h>
#import <OpenGLES/ES2/glext.h>

#include <mach/mach_time.h>

// MSAA_DEFAULT_SAMPLE_COUNT was moved to iPhone_GlesSupport.h
// ENABLE_INTERNAL_PROFILER and related defines were moved to iPhone_Profiler.h
// kFPS define for removed: you can use Application.targetFrameRate (30 fps by default)
// DisplayLink is the only run loop mode now - all others were removed

#include "CrashReporter.h"
#include "iPhone_OrientationSupport.h"
#include "iPhone_Profiler.h"

#include "UI/Keyboard.h"
#include "UI/UnityView.h"
#include "UI/SplashScreen.h"
#include "Unity/DisplayManager.h"
#include "Unity/EAGLContextHelper.h"
#include "Unity/GlesHelper.h"
#include "PluginBase/AppDelegateListener.h"
#import "GADBannerView.h"//here
#import "iAd/ADBannerView.h"


extern "C" void UnityRunUnitTests();

bool	_ios42orNewer			= false;
bool	_ios43orNewer			= false;
bool	_ios50orNewer			= false;
bool	_ios60orNewer			= false;
bool	_ios70orNewer			= false;
bool	_ios80orNewer			= false;

bool	_supportsDiscard		= false;
bool	_supportsMSAA			= false;
bool	_supportsPackedStencil	= false;

bool	_glesContextCreated		= false;
bool	_unityAppReady			= false;
bool	_skipPresent			= false;
bool	_didResignActive		= false;

void UnityInitJoysticks();


@implementation UnityAppController
@synthesize bannerView_;//here
@synthesize bannerIsVisible ;
@synthesize interstitial_;
@synthesize iAdBannerView;
@synthesize isShowAdmob;

@synthesize unityView			= _unityView;
@synthesize unityDisplayLink	= _unityDisplayLink;

@synthesize rootView			= _rootView;
@synthesize rootViewController	= _rootController;
@synthesize mainDisplay			= _mainDisplay;
@synthesize renderDelegate		= _renderDelegate;

- (void)setWindow:(id)object		{}
- (UIWindow*)window					{ return _window; }



- (void)shouldAttachRenderDelegate	{}
- (void)preStartUnity				{}

//admob
- (void)adView:(GADBannerView *)bannerView didFailToReceiveAdWithError:(GADRequestError *)error {
   // NSLog("aA","aa");//(@”aa”);
    int i=0;
    i++;
}


//iad
- (void)bannerViewActionDidFinish:(ADBannerView *)banner{
    NSLog(@"bannerview was selected");
}

- (BOOL)bannerViewActionShouldBegin:(ADBannerView *)banner willLeaveApplication:(BOOL)willLeave
{
    return willLeave;
}

- (void)bannerViewDidLoadAd:(ADBannerView *)baner
{
    if (!self.bannerIsVisible)
    {
        NSLog(@"\nBanner Success");
        [UIView beginAnimations:@"animateAdBannerOn" context:NULL];
        // assumes the banner view is offset 50 pixels so that it is not visible.
        
        baner.frame = CGRectOffset(baner.frame,0,-94);
        [UIView commitAnimations];
        
        self.bannerIsVisible = YES;
    }
}

- (void)bannerView:(ADBannerView *)baner didFailToReceiveAdWithError:(NSError *)error
{
        if (!self.isShowAdmob)
        {
           isShowAdmob = true;
           [self showAdmob];//here
            
        }

    if (self.bannerIsVisible)
    {
        NSLog(@"\nBanner Failed");
        [UIView beginAnimations:@"animateAdBannerOff" context:NULL];
        
        baner.frame = CGRectOffset(baner.frame, 0, 94);
        [UIView commitAnimations];
        
        self.bannerIsVisible = NO;
    }
}
//- (void)bannerView:(ADBannerView *)baner interstitialDidReceiveAd:(NSError *)error
- (void)interstitialDidReceiveAd:(GADInterstitial *)interstitial
{
    [interstitial_ presentFromRootViewController:_rootController];
    
  //      self.bannerIsVisible = NO;
  
}
- (void)interstitial:(GADInterstitial *)interstitial didFailToReceiveAdWithError:(GADRequestError *)error{
    
}

//banner : ca-app-pub-7342700401302892/4468600167
// full : ca-app-pub-7342700401302892/2628816565
-(void) showAdmob
{    
    bannerView_ = [[GADBannerView alloc] initWithAdSize:kGADAdSizeBanner];//here
    bannerView_.adUnitID = @"ca-app-pub-7342700401302892/4468600167";//admob caogiaqn
    // Let the runtime know which UIViewController to restore after taking
    // the user wherever the ad goes and add it to the view hierarchy.
    bannerView_.rootViewController = _rootController;
    [_rootView addSubview:bannerView_];
    [bannerView_ setFrame:CGRectMake(_rootView.bounds.size.width - bannerView_.bounds.size.width,  _rootView.bounds.size.height -  bannerView_.bounds.size.height,  bannerView_.bounds.size.width,bannerView_.bounds.size.height)];
    // Initiate a generic request to load it with an ad.
    [bannerView_ setDelegate:self];
    [bannerView_ loadRequest:[GADRequest request]];
}
-(void) showAdmobFullAds
{
    interstitial_ = [[GADInterstitial alloc] init];
    interstitial_.adUnitID = @"ca-app-pub-7342700401302892/2628816565";

    [interstitial_ setDelegate:self];
   // interstitial_.delegate = self;
    [interstitial_ loadRequest:[GADRequest request]];
}
- (void) showiAd
{
    // Specify the ad unit ID.
    iAdBannerView = [[ADBannerView alloc] initWithFrame:CGRectZero];
    [iAdBannerView setFrame:CGRectMake(_rootView.bounds.size.width - iAdBannerView.bounds.size.width,  _rootView.bounds.size.height -  iAdBannerView.bounds.size.height,  iAdBannerView.bounds.size.width,bannerView_.bounds.size.height)];
    [iAdBannerView setAutoresizingMask:UIViewAutoresizingFlexibleHeight];
    [iAdBannerView setDelegate:self];
    [self.unityView addSubview:iAdBannerView];
    //dich chuyen truoc
    [UIView beginAnimations:@"animateAdBannerOff" context:NULL];
    iAdBannerView.frame = CGRectOffset(iAdBannerView.frame, 0, 94);
    [UIView commitAnimations];
    bannerIsVisible = false;
    
    isShowAdmob = false;//here
}
extern "C" {//here
    
	void NaticeShowAdsBanner (const char* phoneNumber, const char* bodyText)// chua dung vi dung thang trong 
	{

        //[GetAppController() OpenSMS];
      //  [GetAppController() showAdmobFullAds];
		//NSLog(@"Your message here end");
	}
		void NaticeShowAds(const char* phoneNumber, const char* bodyText)
	{

        //[GetAppController() OpenSMS];
        [GetAppController() showAdmobFullAds];
		NSLog(@"Your message here end");
	}
    void NaticeStopAds (const char* phoneNumber, const char* bodyText)
	{
        
    }
    
}

- (void)Open_SMS:(NSString *)phoneNumber second:(NSString *)bodyText
{
   
}
- (void)startUnity:(UIApplication*)application
{
	UnityInitApplicationGraphics();

	// we make sure that first level gets correct display list and orientation
	[[DisplayManager Instance] updateDisplayListInUnity];
	[self updateOrientationFromController:[SplashScreenController Instance]];

	UnityLoadApplication();
	Profiler_InitProfiler();

	[self showGameUI];
	[self showiAd];//here
	[self createDisplayLink];
}

- (void)onForcedOrientation:(ScreenOrientation)orient
{
	[_unityView willRotateTo:orient];
	OrientView(_rootController, _rootView, orient);
	[_rootView layoutSubviews];
	[_unityView didRotate];
}

- (NSUInteger)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window
{
	// UIInterfaceOrientationMaskAll
	// it is the safest way of doing it:
	// - GameCenter and some other services might have portrait-only variant
	//     and will throw exception if portrait is not supported here
	// - When you change allowed orientations if you end up forbidding current one
	//     exception will be thrown
	// Anyway this is intersected with values provided from UIViewController, so we are good
	return   (1 << UIInterfaceOrientationPortrait) | (1 << UIInterfaceOrientationPortraitUpsideDown)
		   | (1 << UIInterfaceOrientationLandscapeRight) | (1 << UIInterfaceOrientationLandscapeLeft);
}

- (void)application:(UIApplication*)application didReceiveLocalNotification:(UILocalNotification*)notification
{
	AppController_SendNotificationWithArg(kUnityDidReceiveLocalNotification, notification);
	UnitySendLocalNotification(notification);
}

- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo
{
	AppController_SendNotificationWithArg(kUnityDidReceiveRemoteNotification, userInfo);
	UnitySendRemoteNotification(userInfo);
}

- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken
{
	AppController_SendNotificationWithArg(kUnityDidRegisterForRemoteNotificationsWithDeviceToken, deviceToken);
	UnitySendDeviceToken(deviceToken);
}

- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error
{
	AppController_SendNotificationWithArg(kUnityDidFailToRegisterForRemoteNotificationsWithError, error);
	UnitySendRemoteNotificationError(error);
}

- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation
{
	NSMutableArray* keys	= [NSMutableArray arrayWithCapacity:3];
	NSMutableArray* values	= [NSMutableArray arrayWithCapacity:3];

	#define ADD_ITEM(item)	do{ if(item) {[keys addObject:@#item]; [values addObject:item];} }while(0)

	ADD_ITEM(url);
	ADD_ITEM(sourceApplication);
	ADD_ITEM(annotation);

	#undef ADD_ITEM

	NSDictionary* notifData = [NSDictionary dictionaryWithObjects:values forKeys:keys];
	AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);
	return YES;
}

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
	printf_console("-> applicationDidFinishLaunching()\n");
	// get local notification
	if (&UIApplicationLaunchOptionsLocalNotificationKey != nil)
	{
		UILocalNotification *notification = [launchOptions objectForKey:UIApplicationLaunchOptionsLocalNotificationKey];
		if (notification)
			UnitySendLocalNotification(notification);
	}

	// get remote notification
	if (&UIApplicationLaunchOptionsRemoteNotificationKey != nil)
	{
		NSDictionary *notification = [launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey];
		if (notification)
			UnitySendRemoteNotification(notification);
	}

	if ([UIDevice currentDevice].generatesDeviceOrientationNotifications == NO)
		[[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];

	[DisplayManager Initialize];

	_mainDisplay	= [[[DisplayManager Instance] mainDisplay] createView:YES showRightAway:NO];
	_window			= _mainDisplay->window;

	[KeyboardDelegate Initialize];

	[self createViewHierarchy];
	[self preStartUnity];
	UnityInitApplicationNoGraphics([[[NSBundle mainBundle] bundlePath]UTF8String]);

	return YES;
}

- (void)applicationDidEnterBackground:(UIApplication *)application
{
	printf_console("-> applicationDidEnterBackground()\n");
}

- (void)applicationWillEnterForeground:(UIApplication *)application
{
	printf_console("-> applicationWillEnterForeground()\n");

	// if we were showing video before going to background - the view size may be changed while we are in background
	[GetAppController().unityView recreateGLESSurfaceIfNeeded];
}

- (void)applicationDidBecomeActive:(UIApplication*)application
{
	printf_console("-> applicationDidBecomeActive()\n");

	if(_unityAppReady)
	{
		if(_didResignActive)
			UnityPause(false);
	}
	else
	{
		[self performSelector:@selector(startUnity:) withObject:application afterDelay:0];
	}

	_didResignActive = false;
}

- (void)applicationWillResignActive:(UIApplication*)application
{
	printf_console("-> applicationWillResignActive()\n");

	if(_unityAppReady)
	{
		UnityPause(true);

		extern void UnityStopVideoIfPlaying();
		UnityStopVideoIfPlaying();

		// Force player to do one more frame, so scripts get a chance to render custom screen for
		// minimized app in task manager.
		UnityForcedPlayerLoop();
		[self repaintDisplayLink];
	}

	_didResignActive = true;
}

- (void)applicationDidReceiveMemoryWarning:(UIApplication*)application
{
	printf_console("WARNING -> applicationDidReceiveMemoryWarning()\n");
}

- (void)applicationWillTerminate:(UIApplication*)application
{
	printf_console("-> applicationWillTerminate()\n");

	Profiler_UninitProfiler();
	UnityCleanup();
}

- (void)dealloc
{
	extern void SensorsCleanup();
	SensorsCleanup();

	[self releaseViewHierarchy];
   bannerView_.delegate = nil;    
    // Don't release the bannerView_ if you are using ARC in your project
    [bannerView_ release];
	[super dealloc];
}
@end


void AppController_RenderPluginMethod(SEL method)
{
	id delegate = GetAppController().renderDelegate;
	if([delegate respondsToSelector:method])
		[delegate performSelector:method];
}
void AppController_RenderPluginMethodWithArg(SEL method, id arg)
{
	id delegate = GetAppController().renderDelegate;
	if([delegate respondsToSelector:method])
		[delegate performSelector:method withObject:arg];
}

void AppController_SendNotification(NSString* name)
{
	[[NSNotificationCenter defaultCenter] postNotificationName:name object:GetAppController()];
}
void AppController_SendNotificationWithArg(NSString* name, id arg)
{
	[[NSNotificationCenter defaultCenter] postNotificationName:name object:GetAppController() userInfo:arg];
}

extern "C" UIWindow*			UnityGetMainWindow()		{ return GetAppController().mainDisplay->window; }
extern "C" UIViewController*	UnityGetGLViewController()	{ return GetAppController().rootViewController; }
extern "C" UIView*				UnityGetGLView()			{ return GetAppController().unityView; }
extern "C" ScreenOrientation	UnityCurrentOrientation()	{ return [GetAppController().unityView contentOrientation]; }



bool LogToNSLogHandler(LogType logType, const char* log, va_list list)
{
	NSLogv([NSString stringWithUTF8String:log], list);
	return true;
}

void UnityInitTrampoline()
{
#if ENABLE_CRASH_REPORT_SUBMISSION
	SubmitCrashReportsAsync();
#endif
	InitCrashHandling();

	_ios42orNewer = [[[UIDevice currentDevice] systemVersion] compare: @"4.2" options: NSNumericSearch] != NSOrderedAscending;
	_ios43orNewer = [[[UIDevice currentDevice] systemVersion] compare: @"4.3" options: NSNumericSearch] != NSOrderedAscending;
	_ios50orNewer = [[[UIDevice currentDevice] systemVersion] compare: @"5.0" options: NSNumericSearch] != NSOrderedAscending;
	_ios60orNewer = [[[UIDevice currentDevice] systemVersion] compare: @"6.0" options: NSNumericSearch] != NSOrderedAscending;
	_ios70orNewer = [[[UIDevice currentDevice] systemVersion] compare: @"7.0" options: NSNumericSearch] != NSOrderedAscending;
	_ios80orNewer = [[[UIDevice currentDevice] systemVersion] compare: @"8.0" options: NSNumericSearch] != NSOrderedAscending;

	// Try writing to console and if it fails switch to NSLog logging
	fprintf(stdout, "\n");
	if (ftell(stdout) < 0)
		SetLogEntryHandler(LogToNSLogHandler);

    // Fix home directory environment variable.
    const char *newHomeDirectory = ([[NSHomeDirectory() stringByAppendingPathComponent:@"Documents"] UTF8String]);
    setenv("XDG_CONFIG_HOME", newHomeDirectory, 1);
    
	UnityInitJoysticks();
}
