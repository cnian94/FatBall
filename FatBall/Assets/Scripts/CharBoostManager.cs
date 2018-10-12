using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartboostSDK;

public class CharBoostManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //CBExternal.initWithAppId("5bbca73af6c3ac0bae5e662d", "282b48c0bdc0119747cbc694e25dbe544bfce099");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowVideo()
    {
        //Chartboost.showRewardedVideo(CBLocation.Startup);
        Chartboost.showInterstitial(CBLocation.HomeScreen);
    }
}
