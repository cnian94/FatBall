using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioScript : MonoBehaviour
{
    public GameObject BackgroundImage;
    public float pixelsToUnits;
    //private Camera camera;

    private Vector2 tempScale;


    void Awake()
    {
        pixelsToUnits = 1;
        //camera = Camera.main;
    }

    private void Start()
    {
        //tempScale = BackgroundImage.transform.localScale;
        //tempScale.x = Screen.width / 10;
        //tempScale.y = Screen.height / 18;
        //BackgroundImage.transform.localScale = tempScale;

    }

    void Update()
    {
        gameObject.GetComponent<Camera>().orthographicSize = Screen.height / pixelsToUnits / 2;
    }
}
