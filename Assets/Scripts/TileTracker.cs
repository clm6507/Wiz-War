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
    public BarrierTracker barrierTracker;
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
                    tile.EastNeighbor = tiles[i + 1][j];
                    tile.WestNeighbor = tiles[tiles.Length - 1][j];
                }
                else if (i == 4)
                {
                    tile.EastNeighbor = tiles[0][i];
                    tile.WestNeighbor = tiles[i - 1][j];
                }
                else
                {
                    tile.EastNeighbor = tiles[i + 1][j];
                    tile.WestNeighbor = tiles[i - 1][j];
                }

                if (j == 0)
                {
                    tile.NorthNeighbor = tiles[i][j + 1];
                    tile.SouthNeighbor = tiles[i][tiles.Length - 1];
                }
                else if (j == 4)
                {
                    tile.NorthNeighbor = tiles[i][0];
                    tile.SouthNeighbor = tiles[i][j - 1];

                }
                else
                {
                    tile.NorthNeighbor = tiles[i][j + 1];
                    tile.SouthNeighbor = tiles[i][j - 1];
                }
            }
        }
        
    }


    void buildBarriers()
    {
        //for correcting positions of the barriers that are created
        Vector3 vertCorrectionVector = new Vector3(-0.5f, 1/16f, -0.5f);
        Vector3 vertExtraCorrectionVector = new Vector3(-0.5f, 1 / 16f, 0.5f);

        Vector3 horizontalCorrectionVector = new Vector3(-0.5f, 1 / 16f, 0.5f);
        Vector3 horizontalExtraCorrectionVector = new Vector3(0.5f, 1 / 16f, 0.5f);

        char[][][] barriers;
        char[][] horizontalBarriers;
        char[][] verticalBarriers;
        barriers = barrierTracker.getBarrierRepresentation(1);
        verticalBarriers = barriers[0];
        horizontalBarriers = barriers[1];
        
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                Tile currentTile = tiles[i][j];


                char vertChar = verticalBarriers[i][j];
                TileBarrier newVertBarrier;
                GameObject newVertBarrierObject;
                if (vertChar == 'w')
                {
                    newVertBarrier = new Wall();
                    newVertBarrierObject = Instantiate(barrierTracker.wallPrefab, currentTile.transform.position + vertCorrectionVector, Quaternion.identity);
                }
                else if(vertChar == 'd')
                {
                    newVertBarrier= new Door();
                    newVertBarrierObject = Instantiate(barrierTracker.doorPrefab, currentTile.transform.position + vertCorrectionVector, Quaternion.identity);
                }
                else
                {
                    newVertBarrier = new NoBarrier();
                    newVertBarrierObject = Instantiate(barrierTracker.noBarrierPrefab, currentTile.transform.position + vertCorrectionVector, Quaternion.identity);
                }
                newVertBarrierObject.transform.parent = barriersParentObject.transform;
                newVertBarrier.barrierVisualList.Add(newVertBarrierObject);

                Tile previousVerticalTile;
                if (j == 0)
                {
                    previousVerticalTile = tiles[i][tiles.Length - 1];
                    GameObject extraVertBarrierObject;
                    if (vertChar == 'w')
                    {
                        extraVertBarrierObject = Instantiate(barrierTracker.wallPrefab, previousVerticalTile.transform.position + vertExtraCorrectionVector, Quaternion.identity);
                    }
                    else if (vertChar == 'd')
                    {
                        extraVertBarrierObject = Instantiate(barrierTracker.doorPrefab, previousVerticalTile.transform.position + vertExtraCorrectionVector, Quaternion.identity);
                    }
                    else
                    {
                        extraVertBarrierObject = Instantiate(barrierTracker.noBarrierPrefab, previousVerticalTile.transform.position + vertExtraCorrectionVector, Quaternion.identity);
                    }
                    extraVertBarrierObject.transform.parent = barriersParentObject.transform;
                    newVertBarrier.barrierVisualList.Add(extraVertBarrierObject);
                }
                else
                {
                    previousVerticalTile = tiles[i][j - 1];
                }
                
                currentTile.SouthBarrier = newVertBarrier;
                previousVerticalTile.NorthBarrier = newVertBarrier;



                char horizontalChar = horizontalBarriers[i][j];
                TileBarrier newHorizontalBarrier; 
                GameObject newHorizontalBarrierObject;
                if (horizontalChar == 'w')
                {
                    newHorizontalBarrier = new Wall();
                    newHorizontalBarrierObject = Instantiate(barrierTracker.wallPrefab, currentTile.transform.position + horizontalCorrectionVector, Quaternion.Euler(0,90,0));

                }
                else if (horizontalChar == 'd')
                {
                    newHorizontalBarrier = new Door();
                    newHorizontalBarrierObject = Instantiate(barrierTracker.doorPrefab, currentTile.transform.position + horizontalCorrectionVector, Quaternion.Euler(0, 90, 0));
                }
                else
                {
                    newHorizontalBarrier = new NoBarrier();
                    newHorizontalBarrierObject = Instantiate(barrierTracker.noBarrierPrefab, currentTile.transform.position + horizontalCorrectionVector, Quaternion.Euler(0, 90, 0));
                }
                newHorizontalBarrierObject.transform.parent = barriersParentObject.transform;
                newHorizontalBarrier.barrierVisualList.Add(newHorizontalBarrierObject);

                Tile previousHorizontalTile;
                if (i == 0)
                {
                    previousHorizontalTile = tiles[tiles.Length - 1][j];
                    GameObject extraHorizontalBarrierObject;
                    if (horizontalChar == 'w')
                    {
                        extraHorizontalBarrierObject = Instantiate(barrierTracker.wallPrefab, previousHorizontalTile.transform.position + horizontalExtraCorrectionVector, Quaternion.Euler(0, 90, 0));
                    }
                    else if (horizontalChar == 'd')
                    {
                        extraHorizontalBarrierObject = Instantiate(barrierTracker.doorPrefab, previousHorizontalTile.transform.position + horizontalExtraCorrectionVector, Quaternion.Euler(0, 90, 0));
                    }
                    else
                    {
                        extraHorizontalBarrierObject = Instantiate(barrierTracker.noBarrierPrefab, previousHorizontalTile.transform.position + horizontalExtraCorrectionVector, Quaternion.Euler(0, 90, 0));
                    }
                    extraHorizontalBarrierObject.transform.parent = barriersParentObject.transform;
                    newHorizontalBarrier.barrierVisualList.Add(extraHorizontalBarrierObject);
                }
                else
                {
                    previousHorizontalTile = tiles[i - 1][j];
                }
                currentTile.WestBarrier = newHorizontalBarrier;
                previousHorizontalTile.EastBarrier = newHorizontalBarrier;



            }
        }
    }
}
