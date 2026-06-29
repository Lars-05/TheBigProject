using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void EndGame()
    {
        StartCoroutine(OnGameEnd());
    }

    IEnumerator OnGameEnd()
    {
        CameraZoomOut.Play();

        yield break;
    }
}
