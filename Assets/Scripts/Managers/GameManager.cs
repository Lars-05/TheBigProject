using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _Ui;
    public static bool gameEnded = false;
    public void EndGame()
    {
        gameEnded = true;
    }

    void Start()
    {
        EndGame();
    }

    IEnumerator OnGameEnd()
    {
        _Ui.SetActive(false);
        CameraZoomOut.Play();

        yield break;
    }
}
