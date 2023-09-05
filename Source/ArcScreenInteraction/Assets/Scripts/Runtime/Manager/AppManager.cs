using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;
    [SerializeField] private GameObject SettingPanel;
    public Action EnterAction;
    public Action BackAction;
    public Action BackAction2;
    public Action SettingAction;
    public Action HomeAction;
    public Action UpAction;
    public Action DownAction;
    public Action LeftAction;
    public Action RightAction;
    private void Awake()
    {
        Instance = this;
    }
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
        //输出当前选择的UI
        if (EventSystem.current.currentSelectedGameObject != null)
            Debug.Log(EventSystem.current.currentSelectedGameObject?.name);
        else
            Debug.Log("null");
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
