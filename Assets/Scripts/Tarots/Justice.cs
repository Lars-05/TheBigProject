using System;
using UnityEngine;

public class Justice : Tarot
{
    [SerializeField] private BroadcastEffect broadcastEffect;
    [SerializeField] private String effectText; 
    private BurningManAI _burningManAI;
    private float _startSpeed = 3.5f;
    
    private void Awake()
    {
        _burningManAI = FindAnyObjectByType<BurningManAI>();
    }
    
    public override void Use(GameObject player)
    {
        _burningManAI.SetStalkSpeed(0);
        Invoke(nameof(ResetStalkSpeed), 5);
        Debug.Log("!");
        broadcastEffect.ChangeText(effectText);
        StartCooldownAnimation();
    }

    private void ResetStalkSpeed()
    {
        _burningManAI.SetStalkSpeed(_startSpeed);
    }
}
