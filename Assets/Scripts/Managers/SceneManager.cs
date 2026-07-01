using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;

    [SerializeField] private ScrollTransision scrollTransition;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;


      
            scrollTransition.gameObject.SetActive(true);
        
    }
    
    public static void GotoScene(string sceneName)
    {
        if (_instance != null)
            _instance.StartCoroutine(_instance.ChangeScene(sceneName));
    }
    
    public static void ResetScene()
    {
        if (_instance != null)
            _instance.StartCoroutine(_instance.ChangeScene(SceneManager.GetActiveScene().name));
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        Debug.Log("Loading scene: " + sceneName);
        
        if (scrollTransition != null)
        {
            yield return scrollTransition.Open();
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
            yield return null;
        
    }

    public static void QuitApplication()
    {
        Application.Quit();
    }
}