using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3 pos = transform.localPosition;
        pos.x = Screen.width / 2;
        pos.y = Screen.height / 2;

        transform.localPosition = pos;

        Vector3 scale = transform.localScale;
        scale.x = Screen.width;
        scale.y = Screen.height;

        transform.localScale = scale;

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
