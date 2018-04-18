using UnityEngine;
using System.Collections;
using System;

public class TerrainChunkGenerator : MonoBehaviour {

    public int width = 128;
    public int length = 128;
    public int seed = 1000;
    
    public GameObject grass;
    public GameObject sand;
    public GameObject snow;
    public GameObject chest;

    // Use this for initialization
    static int EMPTY_SPACE = 0;
    static int BLOCK_GENERATED = 1;
    static int BLOCK_INVISIBLE = 2;
    private Vector3 chest_location;
    public bool chest_generation;
    private bool chest_generated;
    private System.Random rnd;

    private int[,,] allBlocks;
    public int startX, startZ;
    private bool chest_located;

    private Vector3 lowest_pos;
    private GameObject lowest_block;

    void Start () {
        //chest_generation = false;
        chest_generated = false;
        startX = (int) transform.position.x - width / 2;
        startZ = (int) transform.position.z - length / 2;

        lowest_pos = new Vector3();
        lowest_pos.y = 100;
        rnd = new System.Random();
        chest_located = false;

        allBlocks = new int[width, 50, length];

        for (int x = 0; x < width; x++ )
        {
            for (int z = 0; z < length; z++)
            {
                int adjustedX = seed + startX + x;
                int adjustedZ = seed + startZ + z;
                int y = 10 + (int) (Mathf.PerlinNoise(adjustedX/35f, adjustedZ/35f) * 20f);

                GenerateRow(new Vector3(x, y, z));
                
            }
        }
	}

    void Update()
    {
        if (chest_generation == true)
        {
            //Debug.Log("chest_generation get");
            if (chest_generated == false)
            {
                Debug.Log("Chest Generated");
                GenerateChest();
                chest_generated = true;
            }
            
        }
        Debug.Log(lowest_pos.y);
       if (lowest_pos.y < 17)
       {
            Debug.Log("sound");
            lowest_block.GetComponent<AudioSource>().enabled = true;
       }
        
    }

    void GenerateChest()
    {
        Vector3 chest_loc = new Vector3(); ;
        while (chest_located == false)
        {
            int i = rnd.Next(0, width);
            int j = rnd.Next(0, 50);
            int k = rnd.Next(0, length);
            if (allBlocks[i,j,k] == BLOCK_GENERATED)
            {
                chest_located = true;
                chest_loc.x = i;
                chest_loc.y = j;
                chest_loc.z = k;
            }
        }

        Vector3 adjustedPosition = new Vector3(startX + chest_loc.x -0.5f,
                                               chest_loc.y +0.5f,
                                               startZ + chest_loc.z - 0.5f) ;
        GameObject newBlock = (GameObject)Instantiate(chest, adjustedPosition, Quaternion.identity);
        newBlock.transform.parent = gameObject.transform;

        int x = (int)(chest_loc.x);
        int y = (int)(chest_loc.y+1);
        int z = (int)(chest_loc.z);
        allBlocks[x, y, z] = BLOCK_GENERATED;
    }

    void GenerateRow(Vector3 position)
    {
        GenerateBlock(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;
        for (int lowerY = y - 1; lowerY >= 0; lowerY--)
            allBlocks[x, lowerY, z] = BLOCK_INVISIBLE;
    }

    public void GenerateBlock(Vector3 position)
    {
        if (position.y >= 50)
            return;

        GameObject prefab;
        if (position.y < 17)
            prefab = sand;
        else if (position.y > 26)
            prefab = snow;
        else
            prefab = grass;

        Vector3 adjustedPosition = new Vector3(startX + position.x,
                                               position.y,
                                               startZ + position.z);
        GameObject newBlock = (GameObject) Instantiate(prefab, adjustedPosition, Quaternion.identity);
        newBlock.transform.parent = gameObject.transform;

        if (lowest_pos.y > position.y)
        {
            lowest_pos.y = position.y;
            lowest_block = newBlock;
        }

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;
        allBlocks[x, y, z] = BLOCK_GENERATED;
    }

    public void DestroyedBlock(Vector3 position)
    {
        int positionX = (int) position.x;
        int positionY = (int) position.y;
        int positionZ = (int) position.z;

        for (int x = positionX-1; x <= positionX +1; x++)
        {
            for (int y = positionY-1; y <= positionY + 1; y++)
            {
                for (int z = positionZ - 1; z <= positionZ + 1; z++)
                {
                    if (allBlocks[x, y, z] == BLOCK_INVISIBLE)
                        GenerateBlock(new Vector3(x, y, z));
                }
            }
        }

        allBlocks[positionX, positionY, positionZ] = EMPTY_SPACE;
    }
	

}
