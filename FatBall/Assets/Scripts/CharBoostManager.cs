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

    // Use this for initialization
    void Start()
    {
        //CBExternal.initWithAppId("5bbca73af6c3ac0bae5e662d", "aa99a6bbf9f873e06426d665a243a2907c318873");
        //CBExternal.initWithAppId("5bbc7f78feded60b036af338", "e677d2a6f91e0afeff34f429b69b9f383be6b8c0");
        //CBExternal.initWithAppId("5bc727ce83f7f90b99ed89a5", "1b64a239293835737902d0f2553190b8c241d3fd");
        //Chartboost.setAutoCacheAds(autocache);
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
        CBLocation newLocation = CBLocation.locationFromName(location);
        Chartboost.cacheInterstitial(newLocation);
    }

    public void ShowVideo(string location)
    {
        CBLocation newLocation = CBLocation.locationFromName(location);
        Debug.Log("hasInterstitial: " + Chartboost.hasInterstitial(newLocation));
        Chartboost.showInterstitial(newLocation);

    }

}
