using UnityEngine;

public class TeleporterPoint : MonoBehaviour
{
    Teleporter tpM;
    [SerializeField] Transform tpPoint;

    void Start()
    {
        tpM = GameObject.Find("FadeoutPanel").GetComponent<Teleporter>();
        //tpPoint = GetComponentInChildren<Transform>();
        //I wann change thnis to smm better
        //tpM = GetComponent<Teleporter>();
    }
    void OnCollisionEnter(Collision collision)
    {
        AudioManager.PlaySound("DoorCreak");
        tpM.StartTeleportSequence(tpPoint.position);
    }
}
