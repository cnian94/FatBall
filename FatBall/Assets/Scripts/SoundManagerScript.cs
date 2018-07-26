using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public static AudioClip startSound, explosionSound, enemySound, FastjokerSound, ShieldjokerSound, HalfsizejokerSound;
    public static AudioSource audioSrc;

	// Use this for initialization
	void Start () { // Audio dosyasında olan isimler turuncu olanlar. onlara da kodda isim veriyoruz. Taglerdeki, sound dosyasındaki isimler değişecek !!!
        startSound = Resources.Load<AudioClip>("start");
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

    public void PlaySound(string clip) //Seslerin geldiği yerde JokerControl scriptinde void OnTriggerEnter2D(Collider2D col) altında hangi seslerin çıkacağı var. Scriptleri de birbrine yine JokerControl de private SoundManagerScript soundManager şeklinde bağladık.
    {
        switch (clip)
        {
            case "Explosion": //explosion'un bir kere oynaması için.
                audioSrc.PlayOneShot(explosionSound); //yukarda solda tanımlı olan isim buraya yazılır.
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

            case "Shieldjoker": //Shield joker belirli bir süre oynayacağı için PlayOneShot olmaz. 
                audioSrc.clip = ShieldjokerSound;
                audioSrc.Play();
                break;

            case "Start":
                audioSrc.PlayOneShot(startSound);
                break;
        }
    }
}
