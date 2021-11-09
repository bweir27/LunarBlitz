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
        //if (Input.GetMouseButtonDown(0))
        //{
        //    LoadNextScene();
        //}
    }

    public void startSceneTransition()
    {
        Debug.Log("Tranistion Started!");
        StartCoroutine(LoadNextLevel());
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

    public IEnumerator LoadNextLevel()
    {
        crossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        Scene currScene = SceneManager.GetActiveScene();
        int nextLevelBuildIndex = currScene.buildIndex + 1;
        //Debug.Log("SceneLoader: sceneCount - " + SceneManager.sceneCountInBuildSettings);
        if(nextLevelBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelBuildIndex);
        } else
        {
            Debug.LogError("No Next level!");
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
