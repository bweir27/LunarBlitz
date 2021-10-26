using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship1Tower : Tower
{
    public Transform pivot;
    public Transform barrel;
    public GameObject bullet;

    private void Awake()
    {
        
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        rangeDisplay = this.transform.GetChild(1).gameObject;
        rangeRenderer = rangeDisplay.GetComponent<SpriteRenderer>();

        // Listen for mouseHover to focus
        // get the mouse coordinates (which are in screen coords)
        // and convert them to world coordinates
        mousePosInWorldCoords = camera.ScreenToWorldPoint(Input.mousePosition);

        // get a ray from the mouse coordinates
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        //do a raycast into the scene
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit && hit.collider != null)
        {
            // If hovering over tower
            if (hit.collider.gameObject.name.Equals(gameObject.name))
            {
                rangeRenderer.color = rangeDisplayColor;
                //Debug.Log("Display Color: (" + rangeRenderer.color.r + ", " + rangeRenderer.color.g + ", " + rangeRenderer.color.b + ", " + rangeRenderer.color.a + ")");
            }
            else
            {
                Color invis = rangeDisplayColor;
                invis.a = 0;
                rangeRenderer.color = invis;
            }
        }
        else
        {
            //GetComponent<SpriteRenderer>().color = Color.white;
        }
        //rangeRenderer.color = Color.clear;

    }

    protected override void shoot()
    {
        base.shoot();
        //Debug.Log(bullet);
        //Debug.Log("Fire!");
        //create Bullet
        GameObject newBullet = Instantiate(
            bullet,
            barrel.position,
            pivot.rotation);
    }
}
