using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public GameObject tileObject;
    Tile [,] tile = new Tile[GlobalSettings.width, GlobalSettings.height];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GlobalSettings.width; i++)
        {
            for (int j = 0; j < GlobalSettings.height; j++)
            {
                tile[i,j] = new Tile(i, j, tileObject);
            }
        }

        for (int i = 1; i < GlobalSettings.width -1; i++)
        {
            for (int j = 1; j < GlobalSettings.height -1; j++)
            {
                tile[i, j].addNeighbor(tile[i-1, j]);
                tile[i, j].addNeighbor(tile[i+1, j]);
                tile[i, j].addNeighbor(tile[i, j-1]);
                tile[i, j].addNeighbor(tile[i, j+1]);

                tile[i, j].addNeighbor(tile[i - 1, j-1]);
                tile[i, j].addNeighbor(tile[i + 1, j-1]);
                tile[i, j].addNeighbor(tile[i-1, j + 1]);
                tile[i, j].addNeighbor(tile[i+1, j + 1]);
            }
        }
        for (int j = 1; j < GlobalSettings.height - 1; j++)//leftmost column
        {
            tile[0, j].addNeighbor(tile[1, j]);
            tile[0, j].addNeighbor(tile[0, j - 1]);
            tile[0, j].addNeighbor(tile[0, j + 1]);

            tile[0, j].addNeighbor(tile[1, j - 1]);
            tile[0, j].addNeighbor(tile[1, j + 1]);
        }
        for (int j = 1; j < GlobalSettings.height - 1; j++)//rightmostmost column
        {
            tile[0, j].addNeighbor(tile[GlobalSettings.width -2, j]);
            tile[0, j].addNeighbor(tile[GlobalSettings.width - 1, j - 1]);
            tile[0, j].addNeighbor(tile[GlobalSettings.width - 1, j + 1]);

            tile[0, j].addNeighbor(tile[GlobalSettings.width - 2, j - 1]);
            tile[0, j].addNeighbor(tile[GlobalSettings.width - 2, j + 1]);
        }
        for (int i = 1; i < GlobalSettings.width - 1; i++)//topmost row
        {
                tile[i, 0].addNeighbor(tile[i - 1, 0]);
                tile[i, 0].addNeighbor(tile[i + 1, 0]);
                tile[i, 0].addNeighbor(tile[i, 1]);

                tile[i, 0].addNeighbor(tile[i - 1, 1]);
                tile[i, 0].addNeighbor(tile[i + 1, 1]);
            
        }
        for (int i = 1; i < GlobalSettings.width - 1; i++)//bottom row
        {
            tile[i, 0].addNeighbor(tile[i - 1, GlobalSettings.height -1]);
            tile[i, 0].addNeighbor(tile[i + 1, GlobalSettings.height - 1]);
            tile[i, 0].addNeighbor(tile[i, GlobalSettings.height - 2]);

            tile[i, 0].addNeighbor(tile[i - 1, GlobalSettings.height - 2]);
            tile[i, 0].addNeighbor(tile[i + 1, GlobalSettings.height - 2]);

        }


        tile[0, 0].addNeighbor(tile[1, 0]);
        tile[0, 0].addNeighbor(tile[0, 1]);
        tile[0, 0].addNeighbor(tile[1, 1]);

        tile[GlobalSettings.width -1, 0].addNeighbor(tile[GlobalSettings.width -2, 0]);
        tile[GlobalSettings.width -1, 0].addNeighbor(tile[GlobalSettings.width -1, 1]);
        tile[GlobalSettings.width -1, 0].addNeighbor(tile[GlobalSettings.width -2, 1]);

        tile[0, GlobalSettings.height -1].addNeighbor(tile[1, GlobalSettings.height - 1]);
        tile[0, GlobalSettings.height -1].addNeighbor(tile[0, GlobalSettings.height - 2]);
        tile[0, GlobalSettings.height -1].addNeighbor(tile[1, GlobalSettings.height - 2]);

        tile[GlobalSettings.width - 1, GlobalSettings.height - 1].addNeighbor(tile[GlobalSettings.width - 2, GlobalSettings.height - 1]);
        tile[GlobalSettings.width - 1, GlobalSettings.height - 1].addNeighbor(tile[GlobalSettings.width - 1, GlobalSettings.height - 2]);
        tile[GlobalSettings.width - 1, GlobalSettings.height - 1].addNeighbor(tile[GlobalSettings.width - 2, GlobalSettings.height - 2]);


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < GlobalSettings.width; i++)
        {
            for (int j = 0; j < GlobalSettings.height; j++)
            {
                tile[i, j].fireTick();
            }
        }
        for (int i = 0; i < GlobalSettings.width; i++)
        {
                for (int j = 0; j < GlobalSettings.height; j++)
                {
                    tile[i, j].Update();
                }
        }

    }

    public void Clear()
    {
        for (int i = 0; i < GlobalSettings.width; i++)
        {
            for (int j = 0; j < GlobalSettings.height; j++)
            {
                tile[i, j].ClearFire();
            }
        }
    }
};
