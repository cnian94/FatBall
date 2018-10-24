using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{

    public static AdsManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);
    }


    public void ShowRewardedAd()
    {

        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleRewardedAdResult });
        }
    }

    public void ShowRandomdAd()
    {

        if (Advertisement.IsReady())
        {
            Advertisement.Show("video", new ShowOptions() { resultCallback = HandleRandomAd });
        }


    }

    private void HandleRewardedAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Finished !!");
                NetworkManager.instance.playerModel.coins = NetworkManager.instance.playerModel.coins + GameMaster.gm.finalScore;
                NetworkManager.instance.playerModel.weekly_coins = NetworkManager.instance.playerModel.weekly_coins + GameMaster.gm.finalScore;
                NetworkManager.instance.StartCoroutine(NetworkManager.instance.SetHighScore(true));
                break;


            case ShowResult.Skipped:
                Debug.Log("Skipped !!");
                break;


            case ShowResult.Failed:
                Debug.Log("Failed !!");
                break;
        }
    }
    private void HandleRandomAd(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Finished !!");
                break;


            case ShowResult.Skipped:
                Debug.Log("Skipped !!");
                break;


            case ShowResult.Failed:
                Debug.Log("Failed !!");
                break;
        }
    }

}
