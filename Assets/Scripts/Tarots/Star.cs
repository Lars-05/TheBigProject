using System;
using UnityEngine;

public class Star : Tarot
{
    private SanityManager _sanityManager;
    [SerializeField] private BroadcastEffect broadcastEffect;
    [SerializeField] private String effectText; 
    private void Awake()
    {
        _sanityManager = FindAnyObjectByType<SanityManager>();
    }

    public override void Use(GameObject player)
    {
        _sanityManager.ResetSanity();
        Invoke(nameof(StartDrain), _duration);
        StartCooldownAnimation();
        broadcastEffect.ChangeText( effectText);
    }

    private void StartDrain()
    {
        SanityManager.stopDrain = false;
    }
}
