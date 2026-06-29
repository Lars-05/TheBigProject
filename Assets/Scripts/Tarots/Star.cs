using System;
using UnityEngine;

public class Star : Tarot
{
    private SanityManager _sanityManager;

    private void Awake()
    {
        _sanityManager = FindAnyObjectByType<SanityManager>();
    }

    public override void Use(GameObject player)
    {
        _sanityManager.ResetSanity();
        Invoke(nameof(StartDrain), _duration);
    }

    private void StartDrain()
    {
        SanityManager.stopDrain = false;
        Destroy(gameObject);
    }
}
