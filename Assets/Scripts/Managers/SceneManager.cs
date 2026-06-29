using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public void OnEnable()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
            return;
        SceneSwapFX.Open();

    }
    public void ResetScene()
    {
        StartCoroutine(ChangeScene(SceneManager.GetActiveScene().name));
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void GotoScene(string sceneName)
    {
        StartCoroutine(ChangeScene(sceneName));
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        SceneSwapFX.Close();
        SceneManager.LoadScene(sceneName);
        yield break;
    }
}