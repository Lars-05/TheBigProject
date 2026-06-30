using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _Ui;
    [SerializeField] private CassetteManager _cassetteManager;
    public static bool gameEnded = false;

    private void OnEnable()
    {
        Time.timeScale = 1f;
        gameEnded = false;
    }

    public void EndGame()
    {
        gameEnded = true;
        StartCoroutine(OnGameEnd());
    }

    private IEnumerator OnGameEnd()
    {
        _Ui.SetActive(false);
        _cassetteManager.StopPlaying();
        CameraZoomOut.Play();

        yield break;
    }
}