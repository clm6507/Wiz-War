using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Tile : MonoBehaviour 
{
    public Tile NorthNeighbor, EastNeighbor, SouthNeighbor, WestNeighbor;
    public TileBarrier NorthBarrier, EastBarrier, SouthBarrier, WestBarrier;
}
