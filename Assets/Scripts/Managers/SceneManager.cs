using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    void OnEnable()
    {
        EventBus.OnPlayerPassedOut += ResetScene;
    }

    void OnDisable()
    {
        EventBus.OnPlayerPassedOut -= ResetScene;
    }
    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}