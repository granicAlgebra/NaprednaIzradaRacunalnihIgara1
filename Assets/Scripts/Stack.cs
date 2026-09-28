using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class Stack : MonoBehaviour
{
    public int StackAmout;
    public int MaxStack;
    public TMP_Text AmoutTxt;
    public UnityEvent OnValueChanged;

    private void Start()
    {
        OnValueChanged.AddListener(SetTextAmout);
    }
    public void SetTextAmout()
    {
        if (MaxStack == 0)
        {
            AmoutTxt.text = $"{StackAmout}";
        }
        else
        {
            AmoutTxt.text = $"{StackAmout}/{MaxStack}";
        }        
    }
}
