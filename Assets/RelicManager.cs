using UnityEngine;

public class RelicManager : MonoBehaviour
{
    [SerializeField] private int _relicsNeeded;
    public static int relicsNeeded {private set; get;}
    void Awake()
    {
        Relic.OnRelicAdded.AddListener(CheckRelicCount);
        relicsNeeded = _relicsNeeded;
    }

    void CheckRelicCount(int relicCount)
    {
        if (relicCount >= _relicsNeeded)
        {
            //win i guess
            
        }
    }
    
}
