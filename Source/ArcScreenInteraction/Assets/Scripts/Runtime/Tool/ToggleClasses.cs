using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UI.ProceduralImage;


public class ToggleClasses : MonoBehaviour
{
    [SerializeField] AutoFit autoFit;
    public event Action<string> SelectAction;
    public ClassData classData;
    private Toggle _toggle;
    private Toggle toggle
    {
        get
        {
            if (_toggle == null) _toggle = GetComponent<Toggle>();
            return _toggle;
        }
        set
        {
            _toggle = value;
        }
    }
    [SerializeField] private Text title;
    [SerializeField] private ProceduralImage bg;
    [SerializeField] private Color selectedBackgroundColor;
    [SerializeField] private Color unSelectedBackgroundColor;
    [SerializeField] private Color selectedTextColor;
    [SerializeField] private Color unSelectedTextColor;
    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(OnValueChanged);
    }
    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(OnValueChanged);
    }
    public void SetData(ClassData classData)
    {
        this.classData = classData;
        autoFit.SetContent(classData.Title);
        bg.color = unSelectedBackgroundColor;
        title.color = unSelectedTextColor;
    }

    private void OnValueChanged(bool value)
    {
        if (value)
        {
            OnSelected();
        }
        else
        {
            OnUnSelected();
        }
    }
    private void OnSelected()
    {
        SelectAction?.Invoke(title.text);
        bg.color = selectedBackgroundColor;
        title.color = selectedTextColor;
    }
    private void OnUnSelected()
    {
        bg.color = unSelectedBackgroundColor;
        title.color = unSelectedTextColor;
    }
}
