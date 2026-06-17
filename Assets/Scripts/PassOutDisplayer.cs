using System;
using UnityEngine;

public class PassOutDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject _passOutUI;

    private void Awake()
    {
        _passOutUI.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        EventBus.OnPlayerPassedOut += SetPassOutUI;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerPassedOut -= SetPassOutUI;
    }

    private void SetPassOutUI()
    {
        _passOutUI.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
}
