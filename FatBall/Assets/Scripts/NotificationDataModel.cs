using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class NotificationDataModel
{
    public int type;

    public NotificationDataModel(int type)
    {
        this.type = type;
    }
}
