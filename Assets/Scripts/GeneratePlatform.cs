using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneratePlatform : MonoBehaviour
{
    float time_remaining = 0;

    public GameObject player;   //Used to get info on where to spawn platforms based on player Y
    public GameObject platform;
    public float secondsuntilDestroy;

    private int platformTypesLength;    //The number of possible platform types
    private float playerY;
    private float playerZ;
    private float greatestPlayerY;

    public Vector2 platformSpawnDistanceRange;  //The min and max vertical distance platforms can
                                                // spawn from the player
    private float PlatformSpawnDistance
    {
        get
        {
            return Random.Range(platformSpawnDistanceRange.x, platformSpawnDistanceRange.y);
        }
    }

    private GameObject lastPlatform;
    private Dictionary<GameObject, int> platformList;
    private List<KeyValuePair<GameObject, int>> platformsToAge;
    private GameObject platformToRemove;

    public GameObject mushroom;
    public float mushroomSpawnChance;
    public float mushroomYPosition;

    public GameObject corn;
    public float cornSpawnChance;
    public float cornYPosition;

    private enum PlatformTypes
    {
        LeftNormal,
        MiddleNormal,
        RightNormal/*,
        LeftBM,         //Bounce Mushroom platform with low and high sections
        RightBM,        //Bounce Mushroom platform with low and high sections
        LeftLP,         //Leaf Pile platforms, with the left platform 1/3 of the screen and the
                        // right platform 2/3 of the screen
        MiddleLP,       //Leaf Pile platforms, with each platform 1/2 of the screen
        RightLP,        //Leaf Pile platforms, with the left platform 2/3 of the screen and the
                        // right platform 1/3 of the screen*/
    }

    private void Start()
    {
        //platformTypesLength = System.Enum.GetNames(typeof(PlatformTypes)).Length;
        platformTypesLength = 3;    //Temp

        greatestPlayerY = 0f;
        playerY = player.transform.position.y;
        platformList = new Dictionary<GameObject, int>();
        platformsToAge = new List<KeyValuePair<GameObject, int>>();

        //Spawn three starting platforms on-screen
        for(int i = 0; i < 3; i++)
        {
            //CreatePlatform(PlatformSpawnDistance / 4 * (i + 1));
        }
    }

    private void Update()
    {
        playerY = player.transform.position.y;
        /*
        if (playerY < greatestPlayerY)  // Player is currently the lowest they have been, generation is now enabled
        {
            greatestPlayerY = playerY;

            if (lastPlatform != null && (lastPlatform.transform.position.y > playerY - 60))
            {
                CreatePlatform(PlatformSpawnDistance);
            }
            else if (lastPlatform == null)
            {
                CreatePlatform(PlatformSpawnDistance);
            }
        }*/
        
        System.Random rnd_gen = new System.Random();
        int rnd_time = rnd_gen.Next(2, 6);
        bool is_lowest = false;
        if ((playerY) < (greatestPlayerY - 10))
        {
            is_lowest = true;
            greatestPlayerY = playerY;
        }
        if (time_remaining <= 0 && is_lowest) //Generate platforms at random times so that the next platform is generated 3-12
                  // normal platform heights below the prior platform, with 3-6 being favored. If
                  // the height is greater than 6, generate a pumpkin
        {
            time_remaining = rnd_time;
            CreatePlatform(PlatformSpawnDistance);
        }
        else
        {
            time_remaining -= Time.deltaTime;
        }
    }

    private void CreatePlatform(float spawnDistance)
    {
        int rand = Random.Range(0, platformTypesLength);

        //Create the platform according to the index randomly generated


        if(rand == 0 || rand == 1 || rand == 2) //Normal platforms
        {
            //If a normal platform is created, randomly generate 0-2 Mushrooms and/or Corns and
            // place them in random segmented places along the platform

            if(rand == 0) //A left platform
            {
                //Randomly create a 1/3 length spider web or tree branch in the gap next to the
                // platform

                lastPlatform = Instantiate(platform, new Vector3(-10.6f, playerY - spawnDistance, 0f), Quaternion.identity);

                Object.Destroy(lastPlatform, secondsuntilDestroy);
            }
            else if (rand == 1) //A right platform
            {
                //Randomly create a 1/3 length spider web or tree branch in the gap next to the
                // platform

                lastPlatform = Instantiate(platform, new Vector3(0, playerY - spawnDistance, 0f), Quaternion.identity);

                Object.Destroy(lastPlatform, secondsuntilDestroy);
            }
            else //A middle platform
            {
                //Randomly create a 1/4 length spider web or tree branch in each gap next to the
                // platform

                lastPlatform = Instantiate(platform, new Vector3(10.6f, playerY - spawnDistance, 0f), Quaternion.identity);

                Object.Destroy(lastPlatform, secondsuntilDestroy);
            }

            bool noEnemy = false;
            for(int i = 2; i < 9; i++)
            {
                if(noEnemy)
                {
                    noEnemy = false;
                    continue;
                }
                else
                {
                    float seed = Random.Range(0f, 100f);
                    if (seed <= mushroomSpawnChance)
                    {
                        Debug.Log("Mushroom created");
                        lastPlatform.transform.GetChild(i).gameObject.SetActive(false);
                        Transform tempMushroom = Instantiate(mushroom, lastPlatform.transform).transform;
                        Vector3 pos = lastPlatform.transform.GetChild(i).localPosition;
                        pos.y = mushroomYPosition;
                        tempMushroom.localPosition = pos;

                        noEnemy = true;
                    }
                    else if (seed <= mushroomSpawnChance + cornSpawnChance)
                    {
                        Debug.Log("Corn created");
                        lastPlatform.transform.GetChild(i).gameObject.SetActive(false);
                        Transform tempCorn = Instantiate(corn, lastPlatform.transform).transform;
                        Vector3 pos = lastPlatform.transform.GetChild(i).localPosition;
                        pos.y = cornYPosition;
                        tempCorn.localPosition = pos;

                        noEnemy = true;
                    }
                }
            }
        }
        else if(rand == 3 || rand == 4) //Bounce Mushroom platforms
        {
            //Create a Bounce Mushroom and place it randomly in the lower section of the platform
            //Randomly create a 1/4 length spider web  or tree branch in the gap next to the top
            // and bottom of the upper section of the platform
        }
        else if(rand == 5 || rand == 5 || rand == 6) //Leaf Pile platforms
        {
            //Randomly generate 0-2 Mushrooms and/or Corns and place them in random segmented
            // places along the platform.
            //Create a Leaf Pile in-between the two platforms
        }
    }
}
