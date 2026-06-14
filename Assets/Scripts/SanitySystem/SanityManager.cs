using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    public delegate void SanityDisplayerEvent(int amount);

    public static SanityDisplayerEvent GainSanity;
    public static SanityDisplayerEvent LoseSanity;

    [SerializeField] private SanityDisplayer _sanityDisplayer;
    [SerializeField] private int sanityDrainPS = 1;

    private int sanity;
    private Coroutine drainRoutine;

    private void OnEnable()
    {
        GainSanity += IncreaseSanity;
        LoseSanity += DecreaseSanity;

        drainRoutine = StartCoroutine(DrainSanity());
    }
    
    private void Start()
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
            yield return new WaitForSeconds(1f);

            DecreaseSanity(sanityDrainPS);
        }
    }

    public void IncreaseSanity(int amount)
    {
        sanity += amount;
        _sanityDisplayer.UpdateSlider(sanity);
    }

    public void DecreaseSanity(int amount)
    {
        sanity -= amount;
        sanity = Mathf.Max(0, sanity);
        _sanityDisplayer.UpdateSlider(sanity);
    }
}