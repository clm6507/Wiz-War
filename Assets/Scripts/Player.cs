using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hitpoints;
    public int remaining_movement;

    public Tile currentTile;


    //variables for determining if the player is selected
    public bool IsSelected;
    public bool isMoving;
    public Material unselectedMaterial;
    public Material selectedMaterial;


    // Start is called before the first frame update
    void Start()
    {
        hitpoints = 15;
        remaining_movement = 3;

    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void damagePlayer(int damage)
    {
        hitpoints -= damage;
    }

    public void selectPlayer()
    {
        //Debug.Log("Clicked!");
        if (!IsSelected)
        {
            Renderer childRenderer = this.GetComponentInChildren<Renderer>();
            childRenderer.material = selectedMaterial;
            this.IsSelected = true;
        }
        else
        {
            Renderer childRenderer = this.GetComponentInChildren<Renderer>();
            childRenderer.material = unselectedMaterial;
            this.IsSelected = false;
        }
    }
}
