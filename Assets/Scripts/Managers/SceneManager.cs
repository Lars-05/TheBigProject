using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Optional
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            SceneSwapFX.Open();
    }
    

    public static void ResetScene()
    {
        if (_instance != null)
            _instance.StartCoroutine(_instance.ChangeScene(SceneManager.GetActiveScene().name));
    }

    public static void GotoScene(string sceneName)
    {
        if (_instance != null)
            _instance.StartCoroutine(_instance.ChangeScene(sceneName));
    }

    public static void QuitApplication()
    {
        Application.Quit();
    }
    

    public void ResetSceneButton()
    {
        ResetScene();
    }

    public void GotoSceneButton(string sceneName)
    {
        GotoScene(sceneName);
    }

    public void QuitApplicationButton()
    {
        QuitApplication();
    }
    

    private IEnumerator ChangeScene(string sceneName)
    {
        SceneSwapFX.Close();
        SceneManager.LoadScene(sceneName);
        yield break;
    }
}