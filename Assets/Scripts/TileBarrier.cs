using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TileBarrierType
{
    NONE,
    WALL,
    DOOR
}

public class TileBarrier
{
    public TileBarrierType barrierType;
    public bool canWalkThrough;
    public List<GameObject> barrierVisualList = new List<GameObject>();
}

public class Wall : TileBarrier
{
    public Wall()
    {
        barrierType = TileBarrierType.WALL;
        canWalkThrough = false;
    }

    private float hitpoints = 20;
    public float getHitpoints()
    {
        return this.hitpoints;
    }

    public void takeDamage(float damage)
    {
        this.hitpoints -= damage;
        if (this.hitpoints <= 0)
        {
            this.hitpoints = 0;
            this.canWalkThrough = true;
        }
    }

}

public class Door : TileBarrier
{
    public Door()
    {
        barrierType = TileBarrierType.DOOR;
        canWalkThrough = false;
    }

    private float hitpoints = 15;
    private bool open = false;
    private bool jammed = false;
    private bool lockRemoved = false;


    public float getHitpoints()
    {
        return this.hitpoints;
    }

    public void takeDamage(float damage)
    {
        this.hitpoints -= damage;
        if (this.hitpoints <= 0)
        {
            this.hitpoints = 0;
            this.canWalkThrough = true;
        }
    }

    public bool isOpen()
    {
        return this.open;
    }

    public void openDoor()
    {
        this.open = true;
        this.canWalkThrough = true;
    }

    public void closeDoor()
    {
        this.open = false;
        this.canWalkThrough = false;
    }

    public bool isJammed()
    {
        return this.jammed;
    }

    public void jamDoor()
    {
        this.jammed = true;
        this.open = false;
        this.canWalkThrough = false;
    }

    public bool isLockRemoved()
    {
        return this.lockRemoved;
    }

    public void removeLock()
    {
        this.lockRemoved = true;
        this.open = true;
        this.canWalkThrough = true;
    }

}

public class NoBarrier : TileBarrier
{
    public NoBarrier()
    {
        this.barrierType = TileBarrierType.NONE;
        this.canWalkThrough = true;
    }

}
