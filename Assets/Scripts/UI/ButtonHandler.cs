using System.Collections;
using System.Collections.Generic;
using Transitions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public string SceneName;

    private int cheatcheck = 0;
    //onClick method for the flashlight button.
    public void OnFlashlightButtonClick()
    {
        Flashlight.OnToggleFlashlight();
    }

    public void OnRetryButtonClick()
    {
        GameState.Instance.SetEnd(true);
        FindObjectOfType<LevelLoader>().LoadFloorOne();
    }

    public void OnQuitGameOverButtonClick()
    {
        GameState.Instance.SetEnd(true);
        FindObjectOfType<LevelLoader>().LoadMainMenu();
    }

    public void OnSceneSelection()
    {
        LevelLoader.instance.LoadLevelByName(SceneName);
    }

    public void OnCheatAttempt()
    {
        cheatcheck += 1;
        if (cheatcheck >= 5) LevelLoader.instance.LoadCheatMenu();
    }

    public void OnPlayButtonClick()
    {
        GameState.Instance.SetEnd(true);
        FindObjectOfType<LevelLoader>().LoadNextLevel();
        SoundManager.PlaySound(SoundManager.Sound.buttonPress);
    }
}
