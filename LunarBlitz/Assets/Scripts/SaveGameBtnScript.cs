using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameBtnScript : MonoBehaviour
{
    private GameModel gameModel;
    // Start is called before the first frame update
    void Start()
    {
        gameModel = FindObjectOfType<GameModel>();
    }

    public void onSaveBtnClick()
    {
        Debug.Log("Save Game Btn Clicked");
        if(gameModel != null)
        {
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            if(sceneLoader != null)
            {
                Player player = FindObjectOfType<Player>();
                if(player != null)
                {
                    player.UpdateHighestLevelCompleted(sceneLoader.getCurrentSceneNum());
                }
            }
            gameModel.OnSaveClick();
        }
        else
        {
            Debug.LogError("GameModel not found!");
        }
    }
}
