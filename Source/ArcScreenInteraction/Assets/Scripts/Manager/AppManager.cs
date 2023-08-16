using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : Singleton<AppManager>
{
    private GameObject SettingPanel;
    public Action EnterAction;
    public Action BackAction;
    public Action SettingAction;
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.GetUI(UIType.Main).ShowUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SettingAction?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnterAction?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackAction?.Invoke();
        }
    }
}
