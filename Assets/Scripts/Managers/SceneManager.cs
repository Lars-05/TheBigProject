using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    
    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
    
    public void QuitApplication()
    {
        Application.Quit();
    }
    
    public void GotoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}