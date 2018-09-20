using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour {
    private void Awake()
    {
        Vector3 pos = transform.localPosition;
        pos.x = Screen.width / 2;
        pos.y = Screen.height / 2;

        transform.localPosition = pos;

        Vector3 scale = transform.localScale;
        scale.x = Screen.width + 10;
        scale.y = Screen.height;

        transform.localScale = scale;
    }
}
