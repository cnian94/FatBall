using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawnerControl : MonoBehaviour {
    public static bool spawnAllowed;
    public static Vector3 lastSpikePos;

    public static int big_side_limit = 19  ;
    public static int small_side_limit = 11 ;

    public static int distance = Screen.width /20;
    public static int distance2 = Screen.height / 12;
    public static int start_x = Screen.width/20;
    public static int start_y = Screen.height;

    public int num_of_corners = 4;
    private Vector3 movement;

    public GameObject spike;


    // Use this for initialization
    void Start () {
        Debug.Log("WİDTH: " + Screen.width);
        spawnAllowed = true;
        InvokeRepeating("SpawnASpike", 0f, 0.05f);
    }


    void SpawnASpike()
    {
        float x = Screen.width;
        float y = Screen.height;

        if (spawnAllowed)
        {
            for(int i = 0; i < this.num_of_corners; i++)
            {
                if(i == 0)
                {
                    for(int j=0; j<big_side_limit; j++)
                    {
                        lastSpikePos = new Vector3(start_x, start_y, 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        spike.name = "TopSpike";
                        start_x += distance;
                    }
                    
                }
                
                if (i == 1)
                {
                    for (int j = 0; j < small_side_limit ; j++)
                    {
                        lastSpikePos = new Vector3(start_x , start_y - distance2, 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        spike.name = "RightSpike";
                        spike.transform.Rotate(0, 0, -90);
                        start_y -= distance2;
                    }
                    
                }

               if (i == 2)
                {
                    for (int j = 0; j < big_side_limit; j++)
                    {
                        lastSpikePos = new Vector3(start_x - distance, start_y - distance, 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        spike.name = "BottomSpike";
                        spike.transform.Rotate(0, 0, 180);
                        start_x -= distance;
                    }
                    
                }

                if (i == 3)
                {
                    for (int j = 0; j < small_side_limit ; j++)
                    {
                        
                        lastSpikePos = new Vector3(start_x - distance, start_y , 10);
                        spike = Instantiate(spike, lastSpikePos, Quaternion.identity);
                        spike.name = "LeftSpike";
                        spike.transform.Rotate(0, 0, 90);
                        start_y += distance2;
                    }
                    spawnAllowed = false;
                }

            }
        }
    }

}
