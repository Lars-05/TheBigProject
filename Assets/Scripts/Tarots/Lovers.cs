using System;
using UnityEngine;

public class Lovers : Tarot
{
    [SerializeField] private BroadcastEffect broadcastEffect;
    [SerializeField] private String effectText; 
    public override void Use(GameObject player)
    {
        SanityManager.stopDrain = true;
        StartCooldownAnimation();
        Invoke(nameof(StartDrain), _duration);
        broadcastEffect.ChangeText( effectText);
    }

    private void StartDrain()
    {
        SanityManager.stopDrain = false;
    }
}
