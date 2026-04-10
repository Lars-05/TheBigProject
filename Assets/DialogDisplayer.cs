using System.Collections;
using TMPro;
using UnityEngine;

public class DialogDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    public float typeSpeed;

    void Start()
    {
        DisplayText("wawa wawa wawa wawawawa wawa wawawawawawawawawawawawawawawawa");
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
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
