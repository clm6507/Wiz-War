using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardTracker : MonoBehaviour
{
    public Camera mainCamera;
    public Player currentPlayer;
    public Player[] players;
    public GameObject boardPrefab;
    public LayerMask playerMask;
    public LayerMask tileAndPlayerMask;

    public int numPlayers;
    public Tile[][] masterBoard;
    public GameObject[][] boardLayout;

    //variables for when we are moving the player
    public Vector3 correctionVector;
    public Vector3 playerCurrentTargetPosition;
    public Vector3 playerTargetPosition;
    public List<Tile> movementPath;
    //for the smoothdamp function
    private Vector3 velocity;
    private float moveTime;


    // Start is called before the first frame update
    void Start()
    {
        numPlayers = 2;
        //initializes all boards
        StartGame();
        //links all of the boards together
        fixBoards();

        //variables for pathing/moving the player around
        correctionVector = new Vector3(-0.5f, 0.0625f, -0.5f);

        currentPlayer.currentTile = masterBoard[2][2];
        currentPlayer.transform.position = currentPlayer.currentTile.transform.position + correctionVector;
        playerCurrentTargetPosition = currentPlayer.currentTile.transform.position;
        playerTargetPosition = currentPlayer.currentTile.transform.position;
        movementPath = new List<Tile>();

        //Initiallizing velocity to the zero vector for the smoothdamp fuction
        velocity = Vector3.zero;
        moveTime = 1 / 25f;
    }

    // Update is called once per frame
    void Update()
    {
        //FOR MOVING THE PLAYER
        //if there are still spaces we need to move to 
        if (movementPath.Count > 0)
        {
            //if the player is not at the desired location
            if (currentPlayer.transform.position != playerCurrentTargetPosition)
            {
                currentPlayer.transform.position = Vector3.SmoothDamp(currentPlayer.transform.position, playerCurrentTargetPosition, ref velocity, moveTime);

            }
            else
            {
                //if the current target position is the final target position
                if (playerCurrentTargetPosition == playerTargetPosition)
                {
                    movementPath.RemoveAt(0);
                    currentPlayer.isMoving = false;
                }
                else
                {
                    movementPath.RemoveAt(0);
                    playerCurrentTargetPosition = movementPath[0].transform.position + correctionVector;
                    velocity = Vector3.zero;
                }
            }


        }

        //For checking mouse clicks
        //Toggles player selection if a player is clicked and the current player isn't moving
        //Finds a path if the player is stationary and a tile is clicked and the player is selected
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //Debug.Log("Click");
            if (!currentPlayer.isMoving)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileAndPlayerMask))
                {
                    Player hitPlayer = hit.collider.GetComponentInParent<Player>();
                    //Debug.Log(hitPlayer);
                    Tile hitTile = hit.collider.GetComponent<Tile>();
                    //Debug.Log(hitTile);
                    if (hitPlayer != null && hitPlayer == currentPlayer)
                    {
                        //Debug.Log("this is the current player");
                        hitPlayer.selectPlayer();
                    }
                    else if (hitTile != null && currentPlayer.IsSelected)
                    {
                        //Debug.Log("this is a tile");
                        //Debug.Log("Finding path");
                        List<Tile> path = findPath(currentPlayer.currentTile, hitTile);
                        if (path[0] == currentPlayer.currentTile)
                        {
                            if (hitTile != path[path.Count - 1] )
                            {
                                Debug.Log("No path to selected tile");
                            }
                        }
                        if (path.Count > 1)
                        {
                            currentPlayer.isMoving = true;
                            path.RemoveAt(0);
                            playerCurrentTargetPosition = path[0].transform.position + correctionVector;
                            playerTargetPosition = path[path.Count - 1].transform.position + correctionVector;
                            movementPath = path;
                        }
                        currentPlayer.currentTile = path[path.Count - 1];
                    }
                    else if(hitTile != null && !currentPlayer.IsSelected)
                    {
                        bool inLOS = inLineOfSight(currentPlayer.currentTile, hitTile);
                        Debug.Log(hitTile + " is in LOS of the current player: " + inLOS);
                    }
                    
                }

            }

        }
    }

    //Simple BFS to find the path between 2 tiles
    public List<Tile> findPath(Tile currentTile, Tile targetTile)
    {
        List<Tile> explored = new List<Tile>();
        Queue<List<Tile>> queue = new Queue<List<Tile>>();

        //adding first path to the queue
        List<Tile> path = new List<Tile>();
        path.Add(currentTile);

        queue.Enqueue(path);
        explored.Add(currentTile);
        List<Tile> currentPath = path;



        while (queue.Count > 0)
        {
            currentPath = queue.Dequeue();
            Tile latestElement = currentPath[currentPath.Count - 1];
            explored.Add(latestElement);
            if (latestElement == targetTile)
            {
                break;
            }



            //checking if the north neighbor is not explored and you can get past the barrier
            if (!explored.Contains(latestElement.NorthNeighbor) && latestElement.NorthBarrier.canWalkThrough)
            {
                List<Tile> newpath = new List<Tile>();
                foreach (Tile tile in currentPath)
                {
                    newpath.Add(tile);
                }
                newpath.Add(latestElement.NorthNeighbor);
                queue.Enqueue(newpath);
            }

            //checking if the east neighbor is not explored and you can get past the barrier
            if (!explored.Contains(latestElement.EastNeighbor) && latestElement.EastBarrier.canWalkThrough)
            {
                List<Tile> newpath = new List<Tile>();
                foreach (Tile tile in currentPath)
                {
                    newpath.Add(tile);
                }
                newpath.Add(latestElement.EastNeighbor);
                queue.Enqueue(newpath);
            }

            //checking if the south neighbor is not explored and you can get past the barrier
            if (!explored.Contains(latestElement.SouthNeighbor) && latestElement.SouthBarrier.canWalkThrough)
            {
                List<Tile> newpath = new List<Tile>();
                foreach (Tile tile in currentPath)
                {
                    newpath.Add(tile);
                }
                newpath.Add(latestElement.SouthNeighbor);
                queue.Enqueue(newpath);
            }

            //checking if the west neighbor is not explored and you can get past the barrier
            if (!explored.Contains(latestElement.WestNeighbor) && latestElement.WestBarrier.canWalkThrough)
            {
                List<Tile> newpath = new List<Tile>();
                foreach (Tile tile in currentPath)
                {
                    newpath.Add(tile);
                }
                newpath.Add(latestElement.WestNeighbor);
                queue.Enqueue(newpath);
            }
        }

        //if the tile was not found, return a list with only the current tile to signify no path being found
        //the only other reason a path with only 1 tile would be returned is if the targetTile == curTile
        if (!explored.Contains(targetTile))
        {
            currentPath.Clear();
            currentPath.Add(currentTile);
        }

        return currentPath;
    }


    void StartGame()
    {
        if (numPlayers == 1)
        {
            boardLayout = new GameObject[1][];
            boardLayout[0] = new GameObject[1];
            GameObject board_1 = Instantiate(boardPrefab, new Vector3(0f,0f,0f), Quaternion.identity, this.transform);
            TileTracker board1TileTracker = board_1.GetComponentInChildren<TileTracker>();
            boardLayout[0][0] = board_1;
            masterBoard = new Tile[5][];
            for (int i = 0; i < masterBoard.Length; i++)
            {
                masterBoard[i] = new Tile[5];
                for (int j = 0; j < 5; j++)
                {
                    masterBoard[i][j] = board1TileTracker.tiles[i][j];
                }
            }
        }
        else if (numPlayers == 2)
        {
            boardLayout = new GameObject[1][];
            boardLayout[0] = new GameObject[2];
            GameObject board_1 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_2 = Instantiate(boardPrefab, new Vector3(0f, 0f, 5f), Quaternion.identity, this.transform);
            TileTracker board1TileTracker = board_1.GetComponentInChildren<TileTracker>();
            TileTracker board2TileTracker = board_2.GetComponentInChildren<TileTracker>();
            boardLayout[0][0] = board_1;
            boardLayout[0][1] = board_2;
            masterBoard = new Tile[5][];
            for (int i = 0; i < masterBoard.Length; i++)
            {
                masterBoard[i] = new Tile[10];
                for (int j = 0; j < masterBoard[i].Length; j++)
                {
                    if(j < 5)
                    {
                        masterBoard[i][j] = board1TileTracker.tiles[i][j];
                    }
                    else
                    {
                        masterBoard[i][j] = board2TileTracker.tiles[i][j - 5];
                    }
                }
            }
            
        }
        else if(numPlayers == 3)
        {
            GameObject board_1 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_2 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_3 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
        }
        else if(numPlayers == 4)
        {
            GameObject board_1 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_2 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_3 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_4 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
        }
        else if (numPlayers == 5)
        {
            GameObject board_1 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_2 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_3 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_4 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_5 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
        }
        else
        {
            GameObject board_1 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_2 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_3 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_4 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_5 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
            GameObject board_6 = Instantiate(boardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, this.transform);
        }
    }

    public bool inLineOfSight(Tile tile1,  Tile tile2)
    {
        //if the player is on the tile they are trying to see
        if(tile1 == tile2)
        {
            return true;
        }
        //check north
        if(tile1.NorthBarrier.barrierType == TileBarrierType.NONE)
        {
            Tile curTile = tile1.NorthNeighbor;
            while (curTile != tile1 && curTile != tile2)
            {
                if (curTile.NorthBarrier.barrierType == TileBarrierType.NONE)
                {
                    curTile = curTile.NorthNeighbor;
                }
                else
                {
                    break;
                }
            }
            if (curTile == tile2)
            {
                return true;
            }
        }

        //check south
        if (tile1.SouthBarrier.barrierType == TileBarrierType.NONE)
        {
            Tile curTile = tile1.SouthNeighbor;
            while (curTile != tile1 && curTile != tile2)
            {
                if (curTile.SouthBarrier.barrierType == TileBarrierType.NONE)
                {
                    curTile = curTile.SouthNeighbor;
                }
                else
                {
                    break;
                }
            }
            if (curTile == tile2)
            {
                return true;
            }
        }

        //check east
        if (tile1.EastBarrier.barrierType == TileBarrierType.NONE)
        {
            Tile curTile = tile1.EastNeighbor;
            while (curTile != tile1 && curTile != tile2)
            {
                if (curTile.EastBarrier.barrierType == TileBarrierType.NONE)
                {
                    curTile = curTile.EastNeighbor;
                }
                else
                {
                    break;
                }
            }
            if (curTile == tile2)
            {
                return true;
            }
        }

        //check west
        if (tile1.WestBarrier.barrierType == TileBarrierType.NONE)
        {
            Tile curTile = tile1.WestNeighbor;
            while (curTile != tile1 && curTile != tile2)
            {
                if (curTile.WestBarrier.barrierType == TileBarrierType.NONE)
                {
                    curTile = curTile.WestNeighbor;
                }
                else
                {
                    break;
                }
            }
            if (curTile == tile2)
            {
                return true;
            }
        }


        return false;
    }

    public void fixBoards()
    {
        if (numPlayers == 6)
        {

        }
        else if (numPlayers == 5)
        {

        }
        else if (numPlayers == 4)
        {

        }
        else if (numPlayers == 3)
        {

        }
        else if (numPlayers == 2)
        {
            for (int i = 0; i < masterBoard.Length; i++)
            {
                //link the edges and the middles of the boards together
                masterBoard[i][0].SouthNeighbor = masterBoard[i][9];
                masterBoard[i][9].NorthNeighbor = masterBoard[i][0];
                masterBoard[i][4].NorthNeighbor = masterBoard[i][5];
                masterBoard[i][5].SouthNeighbor = masterBoard[i][4];
                //delete the redundent barrier visuals
                List<GameObject> duplicateBarrierVisuals = masterBoard[i][5].SouthBarrier.barrierVisualList;
                for (int j = 0; j < duplicateBarrierVisuals.Count; j++)
                {
                    Destroy(duplicateBarrierVisuals[j]);
                }
                //reassign barrier objects so the tiles share a wall
                masterBoard[i][5].SouthBarrier = masterBoard[i][4].NorthBarrier;
            }
        }
    }
}
