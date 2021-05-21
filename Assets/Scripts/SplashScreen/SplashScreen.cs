using System.Collections;
using Transitions;
using UnityEngine;

namespace SplashScreen
{
    public class SplashScreen : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LoadMenu());
        }

        IEnumerator LoadMenu()
        {
            GameState.Instance.SetEnd(true);
            yield return new WaitForSeconds(2f);
            FindObjectOfType<LevelLoader>().LoadMainMenu();
        }
    }
}
