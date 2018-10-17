using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharModel {

    public string char_id;
    public string char_name;
    public int price;
    public string attr;
    public string img;


    public CharModel(string char_id, string char_name, int price, string attr, string img)
    {
        this.char_id = char_id;
        this.char_name = char_name;
        this.price = price;
        this.attr = attr;
        this.img = img;
    }

    public override string ToString()
    {
        return "char_id:" + this.char_id + System.Environment.NewLine
             + "char_name:" + this.char_name + System.Environment.NewLine
             + "price:" + this.price + System.Environment.NewLine;
    }
}
