using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OneSignalManager : MonoBehaviour
{
    public static UnityEvent notificationEvent;
    public static NotificationDataModel dataModel;

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
        Dictionary<string, object> additional_data = result.notification.payload.additionalData;
        Debug.Log("ADDITIONAL DATA: " + additional_data);
        Debug.Log("ADDITIONAL DATA STRING: " + additional_data);
        Debug.Log("DATA: " + additional_data["data"]);
        dataModel = JsonUtility.FromJson<NotificationDataModel>(JsonUtility.ToJson(additional_data));
        Debug.Log("DATA MODEL: " + dataModel);
        if(dataModel.type == 0)
        {
            notificationEvent.Invoke();
        }



    }

}
