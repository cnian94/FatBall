using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartboostSDK;

public class CharBoostManager : MonoBehaviour
{

    private bool hasInterstitial = false;
    private bool hasRewardedVideo = false;
    private int frameCount = 0;

    private bool ageGate = false;
    private bool autocache = true;
    private bool activeAgeGate = false;
    private bool showInterstitial = true;
    private bool showRewardedVideo = true;


    void OnEnable()
    {
        SetupDelegates();
    }

    // Use this for initialization
    void Start()
    {
        //Chartboost.setAutoCacheAds(autocache);
        Debug.Log("CACHING INTERSTITIAL !!");
        CacheInterstitial("Game Over");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetupDelegates()
    {
        // Listen to all impression-related events
        Chartboost.didInitialize += didInitialize;
        Chartboost.didFailToLoadInterstitial += didFailToLoadInterstitial;
        Chartboost.didDismissInterstitial += didDismissInterstitial;
        Chartboost.didCloseInterstitial += didCloseInterstitial;
        Chartboost.didClickInterstitial += didClickInterstitial;
        Chartboost.didCacheInterstitial += didCacheInterstitial;
        Chartboost.shouldDisplayInterstitial += shouldDisplayInterstitial;
        Chartboost.didDisplayInterstitial += didDisplayInterstitial;
        Chartboost.didFailToRecordClick += didFailToRecordClick;
        Chartboost.didFailToLoadRewardedVideo += didFailToLoadRewardedVideo;
        Chartboost.didDismissRewardedVideo += didDismissRewardedVideo;
        Chartboost.didCloseRewardedVideo += didCloseRewardedVideo;
        Chartboost.didClickRewardedVideo += didClickRewardedVideo;
        Chartboost.didCacheRewardedVideo += didCacheRewardedVideo;
        Chartboost.shouldDisplayRewardedVideo += shouldDisplayRewardedVideo;
        Chartboost.didCompleteRewardedVideo += didCompleteRewardedVideo;
        Chartboost.didDisplayRewardedVideo += didDisplayRewardedVideo;
        Chartboost.didPauseClickForConfirmation += didPauseClickForConfirmation;
        Chartboost.willDisplayVideo += willDisplayVideo;
        /*#if UNITY_IPHONE
		Chartboost.didCompleteAppStoreSheetFlow += didCompleteAppStoreSheetFlow;
        #endif*/
    }

    void OnDisable()
    {
        // Remove event handlers
        Chartboost.didInitialize -= didInitialize;
        Chartboost.didFailToLoadInterstitial -= didFailToLoadInterstitial;
        Chartboost.didDismissInterstitial -= didDismissInterstitial;
        Chartboost.didCloseInterstitial -= didCloseInterstitial;
        Chartboost.didClickInterstitial -= didClickInterstitial;
        Chartboost.didCacheInterstitial -= didCacheInterstitial;
        Chartboost.shouldDisplayInterstitial -= shouldDisplayInterstitial;
        Chartboost.didDisplayInterstitial -= didDisplayInterstitial;
        Chartboost.didFailToRecordClick -= didFailToRecordClick;
        Chartboost.didFailToLoadRewardedVideo -= didFailToLoadRewardedVideo;
        Chartboost.didDismissRewardedVideo -= didDismissRewardedVideo;
        Chartboost.didCloseRewardedVideo -= didCloseRewardedVideo;
        Chartboost.didClickRewardedVideo -= didClickRewardedVideo;
        Chartboost.didCacheRewardedVideo -= didCacheRewardedVideo;
        Chartboost.shouldDisplayRewardedVideo -= shouldDisplayRewardedVideo;
        Chartboost.didCompleteRewardedVideo -= didCompleteRewardedVideo;
        Chartboost.didDisplayRewardedVideo -= didDisplayRewardedVideo;
        Chartboost.didPauseClickForConfirmation -= didPauseClickForConfirmation;
        Chartboost.willDisplayVideo -= willDisplayVideo;
        /*#if UNITY_IPHONE
		Chartboost.didCompleteAppStoreSheetFlow -= didCompleteAppStoreSheetFlow;
        #endif*/
    }

    void didInitialize(bool status)
    {
        Debug.Log(string.Format("didInitialize: {0}", status));
    }

    void didFailToLoadInterstitial(CBLocation location, CBImpressionError error)
    {
        Debug.Log(string.Format("didFailToLoadInterstitial: {0} at location {1}", error, location));
    }

    void didDismissInterstitial(CBLocation location)
    {
        Debug.Log("didDismissInterstitial: " + location);
    }

    void didCloseInterstitial(CBLocation location)
    {
        Debug.Log("didCloseInterstitial: " + location);
    }

    void didClickInterstitial(CBLocation location)
    {
        Debug.Log("didClickInterstitial: " + location);
    }

    void didCacheInterstitial(CBLocation location)
    {
        Debug.Log("HEY BURAYA BAK !!!!");
        Debug.Log("didCacheInterstitial: " + location);
    }

    bool shouldDisplayInterstitial(CBLocation location)
    {
        // return true if you want to allow the interstitial to be displayed
        Debug.Log("shouldDisplayInterstitial @" + location + " : " + showInterstitial);
        return showInterstitial;
    }

    void didDisplayInterstitial(CBLocation location)
    {
        Debug.Log("didDisplayInterstitial: " + location);
    }

    void didFailToRecordClick(CBLocation location, CBClickError error)
    {
        Debug.Log(string.Format("didFailToRecordClick: {0} at location: {1}", error, location));
    }

    void didFailToLoadRewardedVideo(CBLocation location, CBImpressionError error)
    {
        Debug.Log(string.Format("didFailToLoadRewardedVideo: {0} at location {1}", error, location));
    }

    void didDismissRewardedVideo(CBLocation location)
    {
        Debug.Log("didDismissRewardedVideo: " + location);
    }

    void didCloseRewardedVideo(CBLocation location)
    {
        Debug.Log("didCloseRewardedVideo: " + location);
    }

    void didClickRewardedVideo(CBLocation location)
    {
        Debug.Log("didClickRewardedVideo: " + location);
    }

    void didCacheRewardedVideo(CBLocation location)
    {
        Debug.Log("didCacheRewardedVideo: " + location);
    }

    bool shouldDisplayRewardedVideo(CBLocation location)
    {
        Debug.Log("shouldDisplayRewardedVideo @" + location + " : " + showRewardedVideo);
        return showRewardedVideo;
    }

    void didCompleteRewardedVideo(CBLocation location, int reward)
    {
        Debug.Log(string.Format("didCompleteRewardedVideo: reward {0} at location {1}", reward, location));
    }

    void didDisplayRewardedVideo(CBLocation location)
    {
        Debug.Log("didDisplayRewardedVideo: " + location);
    }

    void didPauseClickForConfirmation()
    {
#if UNITY_IPHONE
		Debug.Log("didPauseClickForConfirmation called");
		activeAgeGate = true;
#endif
    }

    void willDisplayVideo(CBLocation location)
    {
        Debug.Log("willDisplayVideo: " + location);
    }

    public void CacheInterstitial(string location)
    {
        //CBLocation newLocation = CBLocation.locationFromName(location);
        Chartboost.cacheInterstitial(CBLocation.GameOver);
    }

    public void ShowVideo(string location)
    {
        //CBLocation newLocation = CBLocation.locationFromName(location);
        Debug.Log("hasInterstitial at " + CBLocation.GameOver + " : " + Chartboost.hasInterstitial(CBLocation.GameOver));
        Chartboost.showInterstitial(CBLocation.GameOver);

    }

}
