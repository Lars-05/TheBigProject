using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _Ui;
    public static bool gameEnded = false;

    private void Start()
    {
        gameEnded = false;
    }
    public void EndGame()
    {
        gameEnded = true;
        StartCoroutine(OnGameEnd());
    }

    IEnumerator OnGameEnd()
    {
        _Ui.SetActive(false);
        CameraZoomOut.Play();
        yield break;
    }
}
