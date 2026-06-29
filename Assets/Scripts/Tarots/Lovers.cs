using UnityEngine;

public class Lovers : Tarot
{
    public override void Use(GameObject player)
    {
        SanityManager.stopDrain = true;
        Invoke(nameof(StartDrain), _duration);
    }

    private void StartDrain()
    {
        SanityManager.stopDrain = false;
        Destroy(gameObject);
    }
}
