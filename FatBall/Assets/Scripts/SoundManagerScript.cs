using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public static AudioClip explosionSound, enemySound, jokerSound;
    static AudioSource audioSrc;

	// Use this for initialization
	void Start () {
        explosionSound = Resources.Load<AudioClip>("explosion");
        enemySound = Resources.Load<AudioClip>("enemy");
        jokerSound = Resources.Load<AudioClip>("joker");

        audioSrc = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "explosion":
                audioSrc.PlayOneShot(explosionSound);
                break;

            case "enemy":
                audioSrc.PlayOneShot(enemySound);
                break;

            case "joker":
                audioSrc.PlayOneShot(jokerSound);
                break;
        }
    }
}
