using System;
using UnityEngine;

public class Chariot : Tarot
{
    [SerializeField] private BroadcastEffect broadcastEffect;
    [SerializeField] private String effectText; 
    public override void Use(GameObject player)
    {
        player.GetComponent<FirstPersonMovement>().SpeedBoost(_duration);
        StartCooldownAnimation();
        broadcastEffect.ChangeText( effectText);
    }
}
