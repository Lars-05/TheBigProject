using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DialogDisplayer : MonoBehaviour
{
    [SerializeField] private Vector3 punch;
    [SerializeField] private TextMeshProUGUI textField;
    public float typeSpeed;

    void Start()
    {
        DisplayText("wawa wawa wawa wawawawa              wawa wawawawawawawawawawawawawawawawa");
    }
    
    void DisplayText(string text)
    {
        textField.text = string.Empty;
        StartCoroutine(TypeText(text));
    }

    IEnumerator TypeText(string text)
    {
        foreach (char c in text)
        {
            textField.text += c;
            if(char.IsWhiteSpace(c))
                continue;
            this.gameObject.transform.DOPunchRotation(punch, typeSpeed, 10, 1);
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
