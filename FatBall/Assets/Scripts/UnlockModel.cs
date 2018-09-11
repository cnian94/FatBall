using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnlockModel {

    public string device_id;
    public int char_id;

    public UnlockModel(string device_id, int char_id)
    {
        this.device_id = device_id;
        this.char_id = char_id;
    }
}
