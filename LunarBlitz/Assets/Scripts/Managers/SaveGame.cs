using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for serialization
using System;
using System.Runtime.Serialization;

[Serializable]
public class SaveGame : ISerializable
{

	// the fields we'll serialize
	public int LastCompletedLevel { get; set; }

	// do nothing constructor
	public SaveGame()
	{
	}

	public SaveGame(SerializationInfo info, StreamingContext context)
	{
		LastCompletedLevel = info.GetInt32("lastCompletedLevel");
	}

	public void StoreData(GameModel model)
	{
		LastCompletedLevel = model.player.GetComponent<Player>().lastCompletedLevel;
	}

	public void LoadData(GameModel model)
	{
		model.player.GetComponent<Player>().lastCompletedLevel = LastCompletedLevel;
	}

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("lastCompletedLevel", LastCompletedLevel);
	}

}