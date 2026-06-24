using UnityEngine;

public class RelicManager : MonoBehaviour
{
    [SerializeField] private int _relicsNeeded;
    
    void Awake()
    {
        Relic.OnRelicAdded.AddListener(CheckRelicCount);
    }

    void CheckRelicCount(int relicCount)
    {
        if (relicCount >= _relicsNeeded)
        {
            Debug.Log("OMGOSH HIIIIIIIIIIIIII BENBIL IN AM THE WIN CONDITION THAT WORKS");
        }
    }
    
}
