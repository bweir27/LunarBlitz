using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator crossFade;

    private void Start()
    {
        if(crossFade == null)
        {
            crossFade = GameObject.FindGameObjectWithTag("CrossFade").GetComponent<Animator>();
        }
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void startSceneTransition()
    {
        Debug.Log("Tranistion Started!");
        StartCoroutine(LoadNextLevel());
    }

    public void startSpecifiedLevelTransition(int levelNum)
    {
        Debug.Log("startSpecifiedLevelTransition: " + levelNum);
        StartCoroutine(LoadSpecificLevel(levelNum));
    }

    public void startLoadNextLevelTransition()
    {
        Debug.Log("startLoadNextLevelTransition");
        StartCoroutine(LoadNextLevel());
    }

    public void startLoadSameLevelTransition()
    {
        Debug.Log("startLoadSameLevelTransition");
        StartCoroutine(LoadSameLevel());
    }

    public void startMainMenuTransition()
    {
        Debug.Log("startMainMenuSceneTransition");
        StartCoroutine(ReturnToMainMenu());
    }

    public IEnumerator LoadSpecificLevel(int levelNum)
    {
        crossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        if (levelNum < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelNum);
        }
        else
        {
            Debug.LogError("No level " + levelNum + "!");
            // Fallback to Main menu
            SceneManager.LoadScene(0);
        }
    }

    public IEnumerator LoadNextLevel()
    {
        crossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        Scene currScene = SceneManager.GetActiveScene();
        int nextLevelBuildIndex = currScene.buildIndex + 1;
        if(nextLevelBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelBuildIndex);
        } else
        {
            Debug.LogError("No Next level!");
            // Fallback to Main menu
            SceneManager.LoadScene(0);
        }
    }

    public IEnumerator LoadSameLevel()
    {
        crossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        
    }

    public IEnumerator ReturnToMainMenu()
    {
        crossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        int nextLevelBuildIndex = 0;
        SceneManager.LoadScene(nextLevelBuildIndex);
    }

    public int getNumScenes()
    {
        return SceneManager.sceneCountInBuildSettings;
    }
}
