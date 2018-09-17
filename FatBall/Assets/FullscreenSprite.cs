using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenSprite : MonoBehaviour {
    public Camera cam;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = cam.orthographicSize * 2f;
        Vector2 cameraSize = new Vector2(cam.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        // Portrait
        scale *= cameraSize.y / spriteSize.y;
      

        //transform.position = Vector2.zero; // Optional
        transform.localScale = scale;
    }
}
