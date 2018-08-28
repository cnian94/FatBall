using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawnerControl : MonoBehaviour
{

    //public bool spawnAllowed;
    public Vector3 lastSpikePos;

    public static int big_side_limit = 11;  //isimler ters ama kim takar :D
    public static int small_side_limit = 18;



    public float widthOfSpike;
    public float heightOfSpike;
    public float distance_x;
    public float distance_y;
    public float start_x;
    public float start_y;

    public int num_of_corners = 4;
    private Vector3 movement;

    public GameObject spike;
    public GameObject[] spikes;


    void Awake()
    {
        start_x = 0;
        start_y = Screen.height;
        widthOfSpike = spike.GetComponent<SpriteRenderer>().bounds.size.x;  //spike'ın widthini bulur sayı olarak yazar
        heightOfSpike = spike.GetComponent<SpriteRenderer>().bounds.size.y;  //spike'ın heightını bulur sayı olarak yazar

    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SpawnASpike());
    }

    public IEnumerator SpawnASpike()
    {
        for (int i = 0; i < this.num_of_corners; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < big_side_limit; j++) //big side sayısına gelene kadar teker teker loop olarak yapar.
                {
                    if (j == 0) //sol köşedeki tek spike için.
                    {
                        //Debug.Log("startX: " + start_x);
                        //Debug.Log("startY: " + start_y);
                        lastSpikePos = new Vector3(start_x - heightOfSpike / 3, start_y + heightOfSpike / 3, 10); // sayfa noyutuna göre ilk spkie'ın pozisyonunu belirler
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        Vector3 newPos = new Vector3(start_x - spike.GetComponent<SpriteRenderer>().bounds.size.y / 3, start_y + spike.GetComponent<SpriteRenderer>().bounds.size.y / 3, 10);
                        spike.transform.position = newPos;
                        distance_x = Screen.width / 10;
                        distance_y = Screen.height / 19;

                        spike.name = "TopLeftCornerSpike";
                        spike.transform.Rotate(0, 0, 45);
                        start_x += Screen.width / 10;
                    }


                    if (j == big_side_limit - 1) // sağ köşedeki spkike için
                    {
                        lastSpikePos = new Vector3(Screen.width + spike.GetComponent<SpriteRenderer>().bounds.size.y/ 3, start_y + spike.GetComponent<SpriteRenderer>().bounds.size.y / 3, 10);
                        spike.transform.position = lastSpikePos;
                        spike.transform.Rotate(0, 0, -45);
                        spike.name = "TopRightCornerSpike";
                        start_x = Screen.width;
                    }
                    else // tepe spikelar için
                    {
                        lastSpikePos = new Vector3(start_x, start_y , 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        Vector3 newPos = new Vector3(start_x, start_y + spike.GetComponent<SpriteRenderer>().bounds.size.y/2, 10);
                        spike.transform.position = newPos;
                        spike.name = "TopSpike";
                        start_x += distance_x;
                    }
                }
            }

            if (i == 1)
            {
                yield return new WaitForSeconds(1f); //bir saniye bekliyoruz. AMA SESİ Nasıl koyduk. Saymaya mı koyduk acaba ? Bakeceğim.

                for (int j = 0; j < small_side_limit; j++) //small side limitine ulaşana kadar devam. Köşeler üstte ve allta olduğu için yok.
                {

                    if (j == 0) //sağ üstteki ilk normal spike'ın yeri bulup bir tane koyuyoruz. Sonrası da aşağıda geliyor.
                    {
                        start_y -= Screen.height / 19;
                        lastSpikePos = new Vector3(start_x + heightOfSpike / 2, start_y, 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        Vector3 newPos = new Vector3(start_x + spike.GetComponent<SpriteRenderer>().bounds.size.y / 2, start_y , 10);
                        spike.transform.position = newPos;
                        spike.name = "RightSpike";
                        spike.transform.Rotate(0, 0, -90);
                    }

                    else
                    {
                        start_y -= distance_y;
                        lastSpikePos = new Vector3(start_x + heightOfSpike / 2, start_y, 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        Vector3 newPos = new Vector3(start_x + spike.GetComponent<SpriteRenderer>().bounds.size.y / 2, start_y , 10);
                        spike.transform.position = newPos;
                        spike.name = "RightSpike";
                        spike.transform.Rotate(0, 0, -90);
                    }

                }
            }

            if (i == 2)
            {
                yield return new WaitForSeconds(1f);

                for (int j = 0; j < big_side_limit; j++)
                {

                    if (j == 0)
                    {
                        start_x = Screen.width;
                        start_y = 0;
                        lastSpikePos = new Vector3(start_x + heightOfSpike / 3, start_y - heightOfSpike / 3, 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        Vector3 newPos = new Vector3(start_x + spike.GetComponent<SpriteRenderer>().bounds.size.y / 3, start_y - spike.GetComponent<SpriteRenderer>().bounds.size.y / 3, 10);
                        spike.transform.position = newPos;
                        start_x -= Screen.width / 10;
                        spike.name = "BottomRightCornerSpike";
                        spike.transform.Rotate(0, 0, -135);
                    }

                    if (j == big_side_limit - 1)
                    {
                        lastSpikePos = new Vector3(0 - spike.GetComponent<SpriteRenderer>().bounds.size.y / 3, start_y - spike.GetComponent<SpriteRenderer>().bounds.size.y / 3, 10);
                        spike.transform.position = lastSpikePos;
                        spike.name = "BottomLeftCornerSpike";
                        start_x = 0;
                        spike.transform.Rotate(0, 0, -45);
                    }

                    else
                    {
                        lastSpikePos = new Vector3(start_x, start_y - heightOfSpike / 2, 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        Vector3 newPos = new Vector3(start_x, -spike.GetComponent<SpriteRenderer>().bounds.size.y / 2, 10);
                        spike.transform.position = newPos;
                        spike.name = "BottomSpike";
                        start_x -= distance_x;
                        spike.transform.Rotate(0, 0, 180);

                    }
                }
            }


            if (i == 3)
            {
                yield return new WaitForSeconds(1f);

                for (int j = 0; j < small_side_limit; j++)
                {

                    if (j == 0)
                    {
                        start_y += Screen.height / 19;
                        lastSpikePos = new Vector3(start_x - heightOfSpike / 2, start_y, 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        Vector3 newPos = new Vector3(start_x - spike.GetComponent<SpriteRenderer>().bounds.size.y / 2, start_y , 10);
                        spike.transform.position = newPos;
                        spike.name = "LeftSpike";
                        spike.transform.Rotate(0, 0, 90);
                    }

                    else
                    {
                        start_y += distance_y;
                        lastSpikePos = new Vector3(start_x - heightOfSpike / 2, start_y, 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        Vector3 newPos = new Vector3(start_x - spike.GetComponent<SpriteRenderer>().bounds.size.y / 2, start_y , 10);
                        spike.transform.position = newPos;
                        spike.name = "LeftSpike";
                        spike.transform.Rotate(0, 0, 90);
                    }
                }
            }
        }
        spikes = GameObject.FindGameObjectsWithTag("Spike");
    }

}
