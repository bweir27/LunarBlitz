using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{
    public Transform pivot;
    public Transform barrel;
    public GameObject bullet;

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
            //FIXME: hover over tower to show range
            if (hit.collider.gameObject.name.Equals(gameObject.name))
            {
                //Debug.Log("Hit Tower!");
                rangeRenderer.color = rangeDisplayColor;
                //Debug.Log("Display Color: (" + rangeRenderer.color.r + ", " + rangeRenderer.color.g + ", " + rangeRenderer.color.b + ", " + rangeRenderer.color.a + ")");
            }
            else
            {
                Color invis = rangeDisplayColor;
                invis.a = 0.0f;
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

        //create Bullet
        GameObject newBullet = Instantiate(
            bullet,
            barrel.position,
            pivot.rotation);
    }
}
