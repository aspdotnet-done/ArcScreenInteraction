using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ToggleCheck : MonoBehaviour
{
    public Action<int> CheckAction;
    public int index = 0;
    private Toggle t;
    private void Start()
    {
        t = GetComponent<Toggle>();
        t.onValueChanged.AddListener(Select);
    }

    void Select(bool isOn)
    {
        if (isOn)
        {
            CheckAction?.Invoke(index);
        }
    }
}
