using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class TileTracker : MonoBehaviour
{
    public Tile[][] tiles;
    public GameObject barriersParentObject;
    // Start is called before the first frame update
    void Awake()
    {

        buildBoard();
        buildBarriers();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //grabs all the tiles of the board and puts them in the tiles[][] as well as sets all the neighbors
    void buildBoard()
    {
        tiles = new Tile[5][];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new Tile[5];
        }

        Tile[] temp = this.GetComponentsInChildren<Tile>();
        for (int i = 0; i < temp.Length; i++)
        {
            tiles[i / 5][i % 5] = temp[i];
        }

        //set all the neighbors for the tiles in tilearray
        for (int i = 0; i < tiles.Length; i++)
        {

            for (int j = 0; j < tiles[i].Length; j++)
            {
                Tile tile = tiles[i][j];

                if (i == 0)
                {
                    tile.NorthNeighbor = tiles[i + 1][j];
                    tile.SouthNeighbor = tiles[tiles.Length - 1][j];
                }
                else if (i == 4)
                {
                    tile.NorthNeighbor = tiles[0][j];
                    tile.SouthNeighbor = tiles[i - 1][j];
                }
                else
                {
                    tile.NorthNeighbor = tiles[i + 1][j];
                    tile.SouthNeighbor = tiles[i - 1][j];
                }

                if (j == 0)
                {
                    tile.EastNeighbor = tiles[i][j + 1];
                    tile.WestNeighbor = tiles[i][tiles.Length - 1];
                }
                else if (j == 4)
                {
                    tile.EastNeighbor = tiles[i][0];
                    tile.WestNeighbor = tiles[i][j - 1];

                }
                else
                {
                    tile.EastNeighbor = tiles[i][j + 1];
                    tile.WestNeighbor = tiles[i][j - 1];
                }
            }
        }
    }
    

    void buildBarriers()
    {
        //for correcting positions of the barriers that are created
        Vector3 vertCorrectionVector = new Vector3(-0.5f, 1/16f, 0.5f);
        Vector3 horizontalCorrectionVector = new Vector3(0.5f, 1/16f, 0.5f);

        char[][][] barriers;
        char[][] horizontalBarriers;
        char[][] verticalBarriers;
        BarrierTracker barrierTracker = this.transform.parent.GetComponentInChildren<BarrierTracker>();
        barriers = barrierTracker.getBarrierRepresentation(1);
        verticalBarriers = barriers[0];
        horizontalBarriers = barriers[1];
        
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                Tile currentTile = tiles[i][j];


                if(j < 4)
                {
                    char horizontalChar = horizontalBarriers[i][j];
                    TileBarrier newHorizontalBarrier;
                    GameObject newHorizontalBarrierObject;

                    if (horizontalChar == 'w')
                    {
                        newHorizontalBarrier = new Wall();
                        newHorizontalBarrierObject = Instantiate(barrierTracker.wallPrefab, currentTile.transform.position + horizontalCorrectionVector, Quaternion.Euler(0, 90, 0), barriersParentObject.transform);
                    }
                    else if (horizontalChar == 'd')
                    {
                        newHorizontalBarrier = new Door();
                        newHorizontalBarrierObject = Instantiate(barrierTracker.doorPrefab, currentTile.transform.position + horizontalCorrectionVector, Quaternion.Euler(0, 90, 0), barriersParentObject.transform);
                    }
                    else
                    {
                        newHorizontalBarrier = new NoBarrier();
                        newHorizontalBarrierObject = Instantiate(barrierTracker.noBarrierPrefab, currentTile.transform.position + horizontalCorrectionVector, Quaternion.Euler(0, 90, 0), barriersParentObject.transform);
                    }
                    newHorizontalBarrier.barrierVisualList.Add(newHorizontalBarrierObject);

                    currentTile.EastBarrier = newHorizontalBarrier;
                    currentTile.EastNeighbor.WestBarrier = newHorizontalBarrier;
                }

                if(i < 4)
                {
                    TileBarrier newVertBarrier;
                    GameObject newVertBarrierObject;
                    char vertChar = verticalBarriers[i][j];
                    if (vertChar == 'w')
                    {
                        newVertBarrier = new Wall();
                        newVertBarrierObject = Instantiate(barrierTracker.wallPrefab, currentTile.transform.position + vertCorrectionVector, Quaternion.identity, barriersParentObject.transform);
                    }
                    else if (vertChar == 'd')
                    {
                        newVertBarrier = new Door();
                        newVertBarrierObject = Instantiate(barrierTracker.doorPrefab, currentTile.transform.position + vertCorrectionVector, Quaternion.identity, barriersParentObject.transform);
                    }
                    else
                    {
                        newVertBarrier = new NoBarrier();
                        newVertBarrierObject = Instantiate(barrierTracker.noBarrierPrefab, currentTile.transform.position + vertCorrectionVector, Quaternion.identity, barriersParentObject.transform);
                    }
                    newVertBarrier.barrierVisualList.Add(newVertBarrierObject);

                    currentTile.NorthBarrier = newVertBarrier;
                    currentTile.NorthNeighbor.SouthBarrier = newVertBarrier;
                }

            }
        }
    }
}
