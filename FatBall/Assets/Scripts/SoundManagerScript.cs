using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public AudioClip GameSound,StartSound, ExplosionSound, EnemySound, RabbitJokerSound, ShieldJokerSound, HalfSizeJokerSound, BeerJokerSound;
    public static AudioSource audioSrc;

    private void Awake() // Audio dosyasında olan isimler turuncu olanlar. onlara da kodda isim veriyoruz. Taglerdeki, sound dosyasındaki isimler değişecek !!!
    {
        GameSound = Resources.Load<AudioClip>("GameSound");
        StartSound = Resources.Load<AudioClip>("Start");
        ExplosionSound = Resources.Load<AudioClip>("Explosion");
        EnemySound = Resources.Load<AudioClip>("Enemy");
        RabbitJokerSound = Resources.Load<AudioClip>("RabbitJoker");
        ShieldJokerSound = Resources.Load<AudioClip>("ShieldJoker");
        HalfSizeJokerSound = Resources.Load<AudioClip>("HalfSizeJoker");
        BeerJokerSound = Resources.Load<AudioClip>("BeerJoker");

        audioSrc = GetComponent<AudioSource>();
    }

    void Start () { 

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(string clip) //Seslerin geldiği yerde JokerControl scriptinde void OnTriggerEnter2D(Collider2D col) altında hangi seslerin çıkacağı var. Scriptleri de birbrine yine JokerControl de private SoundManagerScript soundManager şeklinde bağladık.
    {
        switch (clip)
        {
            case "GameSound":
                audioSrc.clip = GameSound;
                audioSrc.Play();
                break;

            case "Explosion": //explosion'un bir kere oynaması için.
                audioSrc.PlayOneShot(ExplosionSound); //yukarda solda tanımlı olan isim buraya yazılır.
                break;

            case "Enemy":
                audioSrc.PlayOneShot(EnemySound);
                break;

            case "RabbitJoker": 
                audioSrc.PlayOneShot(RabbitJokerSound);
                break;

            case "BeerJoker":
                audioSrc.PlayOneShot(BeerJokerSound);
                break;

            case "HalfSizeJoker":
                audioSrc.PlayOneShot(HalfSizeJokerSound);
                break;

            case "ShieldJoker": //Shield joker belirli bir süre oynayacağı için PlayOneShot olmaz. 
                audioSrc.clip = ShieldJokerSound;
                audioSrc.Play();
                break;

            case "Start":
                audioSrc.PlayOneShot(StartSound);
                break;
        }
    }
}
