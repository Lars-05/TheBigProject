using UnityEngine;
using UnityEngine.UI;

public class SetSensitivity : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    public void Awake()
    {
        SetMouseSensitivity(slider.value);
    }
    public void SetMouseSensitivity(float sensitivity)
    {
        SenstivityManager.mouseSensitivity = sensitivity;
    }
}
