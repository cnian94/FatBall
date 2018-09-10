using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class InventoryItem
{

    public CharModel character;
    public bool purchased;


    /*public InventoryItem(CharModel charModel, bool purchased)
    {
        @char = charModel;
        this.purchased = purchased;

    }*/




    public static InventoryItem CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<InventoryItem>(jsonString);
    }


}


[Serializable]
public class InventoryList
{
    public InventoryItem[] inventory;

    public static InventoryList CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<InventoryList>(jsonString);
    }
}
