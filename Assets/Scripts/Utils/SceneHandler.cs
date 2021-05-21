using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneHandler: MonoBehaviour
    {
        public static void TransitionToSecondFloor()
        {
            SceneManager.LoadScene("Floor 2", LoadSceneMode.Single);
        }
    }
}