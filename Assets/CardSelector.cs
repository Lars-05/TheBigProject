using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    [SerializeField] private int _selectedCardIndex;
    [SerializeField] private GameObject _player;
    private IUsable[] _usables => transform.parent.GetComponentsInChildren<IUsable>();
    private IDisposable _sub;
    private IDisposable _sub2;
    private float _scrollValue;

    private void Start()
    {
        RefreshCards();
    }
    
    private void OnEnable()
    {
        _sub = InputManager.Instance.BindPerformed("UseCard",UseCard);
        _sub2 = InputManager.Instance.BindPerformed("NextCard",NextCard);
    }

    private void OnDisable()
    {
        _sub.Dispose();
        _sub2.Dispose();
    }
    
    private void UseCard(InputAction.CallbackContext _)
    {
        _usables[_selectedCardIndex].Use(_player);
        RefreshCards();
    }
    
    private void NextCard(InputAction.CallbackContext context)
    {
        _scrollValue += context.ReadValue<float>();

        if (_scrollValue >= 2)
        {
            _scrollValue = 0;
            
            transform.GetChild(_selectedCardIndex).GetComponent<Outline>().enabled = false;
            _selectedCardIndex++;
            
            if(_selectedCardIndex >= _usables.Length)
                _selectedCardIndex = 0;
            
            transform.GetChild(_selectedCardIndex).GetComponent<Outline>().enabled = true;

        }
        else if (_scrollValue <= -2)
        {
            _scrollValue = 0;
            
            transform.GetChild(_selectedCardIndex).GetComponent<Outline>().enabled = false;
            _selectedCardIndex--;
            
            if(_selectedCardIndex < 0)
                _selectedCardIndex = _usables.Length-1;
            
            transform.GetChild(_selectedCardIndex).GetComponent<Outline>().enabled = true;

        }
    }

    private void RefreshCards()
    {
        _selectedCardIndex = 0;

        transform.GetChild(_selectedCardIndex).GetComponent<Outline>().enabled = true;
    }
}
