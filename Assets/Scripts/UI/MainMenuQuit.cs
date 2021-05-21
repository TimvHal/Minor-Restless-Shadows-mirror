using System.Collections;
using System.Collections.Generic;
using Transitions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuQuit : MonoBehaviour, IPointerClickHandler
{
    public bool retry;
    public void OnPointerClick(PointerEventData eventData)
    {
        Time.timeScale = 1;
        
        SoundManager.PlaySound(SoundManager.Sound.buttonPress);
        Destroy(GameAssets.i.gameObject);
            var state = GameState.Instance;
            state.SetEnd(true);
        if (retry)
        {
            FindObjectOfType<LevelLoader>().LoadFloorOne();
        }
        else
        {
            FindObjectOfType<LevelLoader>().LoadMainMenu();
        }
    }
}
