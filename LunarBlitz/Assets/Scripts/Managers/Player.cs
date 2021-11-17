using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player mSingleton;
    public int lastCompletedLevel = 0;
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

    public void UpdateHighestLevelCompleted(int lvlNum)
    {
        // safety check to both prevent negative numbers
        //  and to prevent accidental lose of progress
        lastCompletedLevel = Mathf.Max(lastCompletedLevel, lvlNum);
        Debug.Log("Player.UpdateHighestLevelCompleted, lastCompletedLevel: " + lastCompletedLevel);
    }
}
