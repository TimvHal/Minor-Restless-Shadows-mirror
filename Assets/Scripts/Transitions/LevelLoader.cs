using System;
using System.Collections;
using System.Collections.Generic;
using Transitions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    private float transitionTime = 1f;

    private static LevelLoader _i;

    public static LevelLoader instance
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        _i = this;
    }
    
    private void Start()
    {
        transition.SetTrigger("Finish");
        GameState.Instance.SetBeginning(false);
        StartCoroutine(Disable());
    }

    private void FixedUpdate()
    {
        if(GameState.Instance.IsEndOfLevel())
        {
            gameObject.GetComponentInChildren<Canvas>().enabled = true;
            GameState.Instance.SetEnd(false);
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));   
    }

    public void LoadLevelByName(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        
        GameState.Instance.SetBeginning(true);
        SceneManager.LoadScene(levelIndex);
    }
    
    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        
        GameState.Instance.SetBeginning(true);
        SceneManager.LoadScene(levelName);
    }

    private bool GetBeginScene()
    {
        return GameState.Instance.IsBeginOfLevel();
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(transitionTime);
        gameObject.GetComponentInChildren<Canvas>().enabled = false;
    }

    public void LoadMainMenu()
    {
        GameState.Instance.SetBeginning(true);
        StartCoroutine(LoadLevel("Main Menu"));
    }
    
    public void LoadFloorOne()
    {
        GameState.Instance.SetBeginning(true);
        StartCoroutine(LoadLevel("Floor 1"));
    }

    public void LoadFloorTwo()
    {
        GameState.Instance.SetBeginning(true);
        StartCoroutine(LoadLevel("Floor 2"));
    }
    
    public void LoadFloorThree()
    {
        GameState.Instance.SetBeginning(true);
        StartCoroutine(LoadLevel("Floor 3"));
    }

    public void LoadCheatMenu()
    {
        GameState.Instance.SetBeginning(true);
        StartCoroutine(LoadLevel("Cheats"));
    }
}
