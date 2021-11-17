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

    protected override void Start()
    {
        base.Start();
        fireSound.volume = 0.025f;
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
            }
            else
            {
                Color invis = rangeDisplayColor;
                invis.a = 0;
                rangeRenderer.color = invis;
            }
        }
    }

    protected override void shoot()
    {
        base.shoot();

        //create Bullet
        GameObject newBullet = Instantiate(
            bullet,
            barrel.position,
            pivot.rotation);

        // pass the damage property onto the bullet itself
        Bullet b = newBullet.GetComponent<Bullet>();
        b.damage = base.damage;
    }
}
