using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private string digitOrAction = "";
    public string InteractableName { get { return digitOrAction; } set => throw new System.NotImplementedException(); }

    private PasswordMachine _passwordMachine;

    private void Awake()
    {
        _passwordMachine = GetComponentInParent<PasswordMachine>();
    }
    public void Interact()
    {
        PressButton();
    }

    private void PressButton()
    {
        if(digitOrAction == "Enter")
        {
            _passwordMachine.CheckCode();
        }
        else if(digitOrAction == "Clear")
        {
            _passwordMachine.ClearCode();
        }
        else
        {
            _passwordMachine.AddDigit(digitOrAction);
        }
        AudioManager.Instance.PlaySound(SoundList.ButtonSound);
    }
}
