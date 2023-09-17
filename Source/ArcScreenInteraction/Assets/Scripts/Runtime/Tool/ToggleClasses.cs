using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ToggleClasses : MonoBehaviour
{
    public Action SelectAction;
    public Text title;
    private Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnValueChanged);
    }
    private void OnValueChanged(bool value)
    {
        title.color = !value ? Color.white : Color.black;
        if (value)
        {
            SelectAction?.Invoke();
        }

    }
}
