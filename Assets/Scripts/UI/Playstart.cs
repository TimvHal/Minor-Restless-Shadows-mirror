using Transitions;
using UnityEngine;
using UnityEngine.EventSystems;

public class Playstart : MonoBehaviour, IPointerClickHandler
{
    private GameObject _loader;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameState.Instance.SetEnd(true);
        FindObjectOfType<LevelLoader>().LoadNextLevel();
        SoundManager.PlaySound(SoundManager.Sound.buttonPress);
    }
}
