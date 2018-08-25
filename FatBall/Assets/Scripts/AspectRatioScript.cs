using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioScript : MonoBehaviour
{

    public float pixelsToUnits;
    private Camera camera;


    void Awake()
    {
        pixelsToUnits = 1;
        camera = Camera.main;
    }

    void Update()
    {
        camera.orthographicSize = Screen.height / pixelsToUnits / 2;
    }
}
