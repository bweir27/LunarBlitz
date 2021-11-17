using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// we need these namespaces for serialization
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


public class GameModel : MonoBehaviour
{
    // reference to this game model as a singleton--never use this, just use GameModel.Instance
    private static GameModel mSingleton;

    // the (static) property to grab the instance with, 
    // notice that it's read-only
    public static GameModel Instance { get { return mSingleton; } }

    // public variable that can be dragged in the editor over here
    public GameObject player;

	// name of the file to read from and write to
	public string saveFileName;

	void Awake()
	{
		// set up the Singleton
		if (mSingleton == null)
		{
			mSingleton = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			// if this isn't the first copy, get rid of it
			Destroy(gameObject);
		}
	}

	// this can be passed to a button that calls this method when clicked
	public void OnSaveClick()
	{
		Debug.Log("Save Game Clicked");
		SaveGame saveGame = new SaveGame();

		// then write it out
		SaveGameModel(saveGame, saveFileName);
	}

	// this can be passed to a button that calls this method when clicked
	public void OnLoadClick()
	{
		Debug.Log("Load Game Clicked");
		// load game does all the work of calling loaddata
		LoadGame(saveFileName);
	}


	public void SaveGameModel(SaveGame save, string filename)
	{
		BinaryFormatter bf = new BinaryFormatter();
		// then create a file stream that can be opened or created, with write access to it
		FileStream fs = File.OpenWrite(Application.persistentDataPath + "/" + filename + ".dat");

		// note that we store the data from our game model (this object)
		// first in the SaveGame instance, think of SaveGame like a file
		save.StoreData(this);

		// then we can serialize it to the disk using Serialize and
		// we serialize the SaveGame object. 
		bf.Serialize(fs, save);

		// close the file stream
		fs.Close();
		Debug.Log("Game Saved");
	}

	public void LoadGame(string filename)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream fs = File.OpenRead(Application.persistentDataPath + "/" + filename + ".dat");

		// deserialize the save game--this will throw an exception if we can't
		// deserialize from the file stream
		SaveGame saveGame = (SaveGame)bf.Deserialize(fs);

		// we assume we have access to the game model
		saveGame.LoadData(this);
		

		// close the file stream
		fs.Close();

		SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
		if (sceneLoader != null)
		{
			int numScenes = sceneLoader.getNumScenes();
			Debug.Log("NumScenes: " + numScenes);
			Debug.Log("LastCompletedLevel: " + saveGame.LastCompletedLevel);
			if(saveGame.LastCompletedLevel < 1)
            {
				// Load Level 1 by default
				Debug.Log("Default - Loading Level 1...");
				sceneLoader.startSpecifiedLevelTransition(1);
			}
			else if (saveGame.LastCompletedLevel + 1 < numScenes)
            {
				// Load next unseen level
				sceneLoader.startSpecifiedLevelTransition(saveGame.LastCompletedLevel + 1);
			}
            else
            {
				// replay most recent level if it was the final one
				sceneLoader.startSpecifiedLevelTransition(numScenes - 1);
			}
			
        }
	}
}
