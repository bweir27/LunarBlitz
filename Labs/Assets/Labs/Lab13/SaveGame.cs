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
	public int Score { get; set; }
	//public int Health { get; set; }
	//public int Exp { get; set; }
	public Vector3 playerPosition;

	// do nothing constructor
	public SaveGame()
	{
	}

	public SaveGame(SerializationInfo info,
		StreamingContext context)
	{
		Score = info.GetInt32("score");
		//Health = info.GetInt32("health");
		//Exp = info.GetInt32("exp");
		playerPosition = new Vector3(
			info.GetSingle("posx"),
			info.GetSingle("posy"),
			info.GetSingle("posz"));
	}

	public void StoreData(GameModel model)
	{
		//Health = model.player.GetComponent<Player>().health;
		//Exp = model.player.GetComponent<Player>().experience;
		Score = model.player.GetComponent<Player>().score;
		playerPosition = model.player.gameObject.transform.position;

	}

	public void LoadData(GameModel model)
	{
		//model.player.GetComponent<Player>().health = Health;
		//model.player.GetComponent<Player>().experience = Exp;
		model.player.GetComponent<Player>().score = Score;
		model.player.gameObject.transform.position = playerPosition;

	}

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		//info.AddValue("health", Health);
		//info.AddValue("exp", Exp);
		info.AddValue("score", Score);
		info.AddValue("posx", playerPosition.x);
		info.AddValue("posy", playerPosition.y);
		info.AddValue("posz", playerPosition.z);
	}

}