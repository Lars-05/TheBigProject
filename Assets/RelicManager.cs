using System;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    [SerializeField] private int _relicsNeeded;
    [SerializeField] private GameManager _gameManager;
    public static int relicsNeeded {private set; get;}
    void Awake()
    {
        Relic.OnRelicAdded.AddListener(CheckRelicCount);
        relicsNeeded = _relicsNeeded;

    }
    void CheckRelicCount(int relicCount)
    {
        if (relicCount >= _relicsNeeded)
        {
            _gameManager.EndGame();
        }
    }
    
}
