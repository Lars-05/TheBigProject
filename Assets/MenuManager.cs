using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    private bool menuOpen;
    [SerializeField] private GameObject menuContent;
    void Awake()
    {
        CloseMenu();
    }

    public void OpenMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        menuContent.SetActive(true);
        Time.timeScale = 0;
        menuOpen = true;
    }

    public void CloseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        menuContent.SetActive(false);
        Time.timeScale = 1;
        menuOpen = false;

    }
    
    private void OpenMenu(InputAction.CallbackContext _)
    {
        if (menuOpen)
        {
            CloseMenu();
            return;
        }

        OpenMenu();
    }

    private IDisposable disposable; // null
    private void OnEnable()
    {
        disposable = InputManager.Instance.BindPerformed("OpenMenu",OpenMenu);
    }
    private void OnDisable()
    {
        disposable.Dispose();
    }
}
