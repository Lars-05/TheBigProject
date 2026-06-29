using UnityEngine;

public class Chariot : Tarot
{
    public override void Use(GameObject player)
    {
        player.GetComponent<FirstPersonMovement>().SpeedBoost(_duration);
        StartCooldownAnimation();
    }
}
