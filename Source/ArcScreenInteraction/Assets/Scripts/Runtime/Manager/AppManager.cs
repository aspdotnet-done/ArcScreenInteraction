using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : Singleton<AppManager>
{
    [SerializeField] private GameObject SettingPanel;
    public Action EnterAction;
    public Action BackAction;
    public Action SettingAction;
    public Action HomeAction;
    public Action UpAction;
    public Action DownAction;
    public Action LeftAction;
    public Action RightAction;
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.GetUI(UIType.Main).ShowUI();
        HomeAction += Home;
        SettingAction += Setting;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Home()
    {
        SettingPanel.SetActive(false);
        UIManager.Instance.GetUI(UIType.MediaListUI).HideUI();
        UIManager.Instance.GetUI(UIType.MediaPlayUI).HideUI();
        UIManager.Instance.GetUI(UIType.Main).ShowUI();
    }
    void Setting()
    {
        if (!SettingPanel.activeSelf)
            (UIManager.Instance.GetUI(UIType.Main) as MainUI).SettingShow();
        else
            SettingPanel.SetActive(false);
    }
}
