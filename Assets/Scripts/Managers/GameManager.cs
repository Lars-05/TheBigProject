using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        CameraZoomOut.Play();

        yield break;
    }
}
