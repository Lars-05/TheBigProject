using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class Teleporter : MonoBehaviour
{
    Animator anim;
    Transform player;
    Vector3 tpLoc;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    public void StartTeleportSequence(Vector3 tpPoint)
    {
        anim.Play("FadeEffect",0,0.01f);
        tpLoc = tpPoint;
    }

    public void TeleportPlayer()
    {
        player.transform.position = tpLoc;
    }

}
