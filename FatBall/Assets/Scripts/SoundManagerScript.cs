using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public static AudioClip explosionSound, enemySound, FastjokerSound, ShieldjokerSound, HalfsizejokerSound;
    public static AudioSource audioSrc;

	// Use this for initialization
	void Start () {
        explosionSound = Resources.Load<AudioClip>("Explosion");
        enemySound = Resources.Load<AudioClip>("enemy");
        FastjokerSound = Resources.Load<AudioClip>("Fastjoker");
        ShieldjokerSound = Resources.Load<AudioClip>("Shieldjoker");
        HalfsizejokerSound = Resources.Load<AudioClip>("HalfSize");

        audioSrc = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Explosion":
                audioSrc.PlayOneShot(explosionSound);
                break;

            case "enemy":
                audioSrc.PlayOneShot(enemySound);
                break;

            case "Fastjoker": 
                audioSrc.PlayOneShot(FastjokerSound);
                break;

            case "HalfSize":
                audioSrc.PlayOneShot(HalfsizejokerSound);
                break;

            case "Shieldjoker":
                audioSrc.clip = ShieldjokerSound;
                audioSrc.Play();
                break;
        }
    }
}
