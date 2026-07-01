using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BroadcastEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;


    public void Awake()
    {
        textMeshProUGUI.gameObject.SetActive(false);
    }

    public void ChangeText(string newText)
    {
        
        textMeshProUGUI.gameObject.SetActive(true);
        StartCoroutine(ChangeTextCoroutine(newText));
    }

    IEnumerator ChangeTextCoroutine(string newText)
    { ;
        textMeshProUGUI.text = newText;
        yield return new WaitForSeconds(3);
        textMeshProUGUI.gameObject.SetActive(false);
    }
}
