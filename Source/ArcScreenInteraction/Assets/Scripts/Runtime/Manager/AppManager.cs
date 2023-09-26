using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ScriptableObjectArchitecture;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;
    [SerializeField] GameEvent homeEvent;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        UIManager.Instance.GetUI(UIType.Main).ShowUI();
    }
    private void OnEnable()
    {
        homeEvent.AddListener(Home);
    }
    private void OnDisable()
    {
        homeEvent.RemoveListener(Home);
    }
    void Home()
    {
        UIManager.Instance.GetUI(UIType.MediaListUI).HideUI();
        UIManager.Instance.GetUI(UIType.MediaPlayUI).HideUI();
        UIManager.Instance.GetUI(UIType.Main).ShowUI();
    }

}
