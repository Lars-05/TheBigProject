using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public delegate void SanityDisplayerEvent(int amount);
    public static SanityDisplayerEvent GainSanity;
    public static SanityDisplayerEvent LoseSanity;
    
    [SerializeField] private SanityDisplayer _sanityDisplayer;
    private int sanity;
    
    
    private void OnEnable()
    {
        GainSanity += IncreaseSanity;
        LoseSanity += DecreaseSanity;
    }
    
    private void OnDisable()
    {
        GainSanity -= IncreaseSanity;
        LoseSanity -= DecreaseSanity;
    }

    public void IncreaseSanity(int amount)
    {
        sanity += amount;
        _sanityDisplayer.UpdateSlider(sanity);
    }

    public void DecreaseSanity(int amount)
    {
        sanity -= amount;
        _sanityDisplayer.UpdateSlider(sanity);
    }
    
}
