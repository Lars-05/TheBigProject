using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    public delegate void SanityDisplayerEvent(int amount);

    public static SanityDisplayerEvent GainSanity;
    public static SanityDisplayerEvent LoseSanity;
    public static bool isDead;
    public static bool stopDrain;

    [SerializeField] private SanityDisplayer _sanityDisplayer;
    [SerializeField] private int sanityDrainPS = 1;
    [SerializeField] private int huntSanityDrainPS = 1;

    
    private int sanity;
    private Coroutine drainRoutine;

    private void OnEnable()
    {
        isDead = false;
        GainSanity += IncreaseSanity;
        LoseSanity += DecreaseSanity;

        drainRoutine = StartCoroutine(DrainSanity());
    }
    
    private void Start()
    {
        sanity = Mathf.RoundToInt(_sanityDisplayer.GetComponent<SanityDisplayer>()
            .GetComponentInChildren<Slider>().maxValue);
    }

    public void ResetSanity()
    {
        sanity = Mathf.RoundToInt(_sanityDisplayer.GetComponent<SanityDisplayer>()
            .GetComponentInChildren<Slider>().maxValue);
    }
    
    private void OnDisable()
    {
        GainSanity -= IncreaseSanity;
        LoseSanity -= DecreaseSanity;

        if (drainRoutine != null)
        {
            StopCoroutine(drainRoutine);
            drainRoutine = null;
        }
    }

    private IEnumerator DrainSanity()
    {
        while (true)
        {
            int sanityDiff = BurningManAI.isHunting? huntSanityDrainPS: sanityDrainPS;
            yield return new WaitForSeconds(1f);
            
            Debug.Log(stopDrain);
            if(!stopDrain)
                DecreaseSanity(sanityDiff);
        }
    }

    public void IncreaseSanity(int amount)
    {
        sanity += amount;
        _sanityDisplayer.UpdateSlider(sanity);
    }

    public void DecreaseSanity(int amount)
    {
        if(isDead)
            return;
        
        sanity -= amount;
        sanity = Mathf.Max(0, sanity);
        _sanityDisplayer.UpdateSlider(sanity);
        if (sanity == 0)
        {
            EventBus.RaiseOnPlayerPassedOut();
            isDead = true;
        }
    }
}