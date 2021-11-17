using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameBtn : MonoBehaviour
{
    private GameModel gameModel;
    // Start is called before the first frame update
    void Start()
    {
        gameModel = FindObjectOfType<GameModel>();
    }

    public void onLoadBtnClick()
    {
        Debug.Log("Load Game Btn Clicked");
        if (gameModel != null)
        {
            gameModel.OnLoadClick();
        }
        else
        {
            Debug.LogError("GameModel not found!");
        }
    }
}
