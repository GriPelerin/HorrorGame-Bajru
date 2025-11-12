using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PasswordMachine : MonoBehaviour
{
    [SerializeField] private List<MapPieceSO> mapPiecesSO;

    [SerializeField] private TextMeshProUGUI passwordText;

    private string _enterCode = "";
    private string _currentPassword;

    private void Start()
    {
        GenerateRandomPassword();
        AssignNumbersToPieces();
    }
    public void UpdateText()
    {
        passwordText.text = _enterCode;
    }
    public void AddDigit(string digit)
    {
        if(_enterCode.Length < _currentPassword.Length)
        {
            _enterCode += digit;
            UpdateText();
        }
    }

    public void CheckCode()
    {
        if(_enterCode == _currentPassword)
        {
            GameEvents.Instance.TriggerCorrectPassword();
        }
        else
        {
            ClearCode();
        }
    }

    public void ClearCode()
    {
        _enterCode = "";
        Debug.Log("Password Cleared");
        UpdateText();
    }

    public void GenerateRandomPassword()
    {
        List<int> digits = new List<int>();
        while (digits.Count < mapPiecesSO.Count)
        {
            int randomDigit = Random.Range(0, 10);
            if (!digits.Contains(randomDigit))
                digits.Add(randomDigit);
        }
        _currentPassword = string.Join("", digits);
        Debug.Log(_currentPassword);
    }
    private void AssignNumbersToPieces()
    {
        for (int i = 0; i < mapPiecesSO.Count; i++)
        {
            mapPiecesSO[i].SetNumber(int.Parse(_currentPassword[i].ToString()));
        }
    }
}
