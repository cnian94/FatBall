using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSignalManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        // Enable line below to enable logging if you are having issues setting up OneSignal. (logLevel, visualLogLevel)
        // OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.INFO, OneSignal.LOG_LEVEL.INFO);

        OneSignal.StartInit("94f4772c-5b66-4fd5-9b51-26d03e028739").HandleNotificationOpened(HandleNotificationOpened).EndInit();

        OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;

    }

    // Gets called when the player opens the notification.
    private static void HandleNotificationOpened(OSNotificationOpenedResult result)
    {

    }

}
