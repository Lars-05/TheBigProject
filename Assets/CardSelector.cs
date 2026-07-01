using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    [SerializeField] private int _selectedCardIndex;
    [SerializeField] private GameObject _player;
    private List<IUsable> _usables;
    private IDisposable _sub;
    private IDisposable _sub2;
    private float _scrollValue;
    private bool _using;

    private void Start()
    {
        _usables = transform.parent.GetComponentsInChildren<IUsable>().ToList();
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
        if (_using)
            return;
        
        _using = true;
        Invoke(nameof(Wait), 10.25f);
        
        _usables[_selectedCardIndex].Use(_player);
        _usables.RemoveAt(_selectedCardIndex);
    }
    
    private void NextCard(InputAction.CallbackContext context)
    {
        if (_using)
            return;
        
        _scrollValue += context.ReadValue<float>();

        if (_scrollValue >= 2)
        {
            _scrollValue = 0;
            
            transform.GetChild(_selectedCardIndex).GetComponent<Outline>().enabled = false;
            _selectedCardIndex++;
            
            if(_selectedCardIndex >= _usables.Count)
                _selectedCardIndex = 0;
            
            transform.GetChild(_selectedCardIndex).GetComponent<Outline>().enabled = true;

        }
        else if (_scrollValue <= -2)
        {
            _scrollValue = 0;
            
            transform.GetChild(_selectedCardIndex).GetComponent<Outline>().enabled = false;
            _selectedCardIndex--;
            
            if(_selectedCardIndex < 0)
                _selectedCardIndex = _usables.Count-1;
            
            transform.GetChild(_selectedCardIndex).GetComponent<Outline>().enabled = true;

        }
    }

    private void RefreshCards()
    {
        _selectedCardIndex = 0;
        transform.GetChild(_selectedCardIndex).GetComponent<Outline>().enabled = true;
    }

    private void Wait()
    {
        _using = false;
        RefreshCards();
    }
}
