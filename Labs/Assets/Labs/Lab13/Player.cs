using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player mSingleton;
    public int score = 0;
    private Vector2 mousePosInWorldCoords;
    private Camera camera;

    // method to allow you to return the singleton for this class
    public static Player Instance { get { return mSingleton; } }

    // set your singleton if it hasn't been created, otherwise destroy the object
    void Awake()
    {
        // if the singleton is null, it means it's the first time we created this class
        if (mSingleton == null)
        {
            mSingleton = this;

            // the whole point of making a singleton is to create something that can't be destroyed
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // otherwise, we've already set it, so destroy this object
            Destroy(gameObject);
        }
        camera = Camera.main;
    }

    private void Update()
    {
        bool click = Input.GetButtonDown("Fire1");
        if (click)
        {
            // get the mouse coordinates (which are in screen coords)
            // and convert them to world coordinates
            mousePosInWorldCoords = camera.ScreenToWorldPoint(Input.mousePosition);

            // get a ray from the mouse coordinates
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            //do a raycast into the scene
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit && hit.collider != null)
            {
                if (hit.collider.gameObject.name == mSingleton.name)
                {
                    score++;
                }
            }
        }
    }
}
