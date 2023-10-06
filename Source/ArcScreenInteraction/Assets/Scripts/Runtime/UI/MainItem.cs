using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.EventSystems;

public class MainItem : MonoBehaviour, ISelectHandler
{
    [SerializeField] RawImage image = default;
    [SerializeField] Text message = default;
    [SerializeField] Button button = default;
    [SerializeField] private GameObject selectDotImage;
    [SerializeField] private SelectHandleOverride selectHandleOverride;

    private void OnEnable()
    {
        button.onClick.AddListener(Click);
        selectHandleOverride.OnSelectEvent += OnSelect;
        selectHandleOverride.OnDeselectEvent += OnDeselect;
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(Click);
        selectHandleOverride.OnSelectEvent -= OnSelect;
        selectHandleOverride.OnDeselectEvent -= OnDeselect;
    }
    private Action<string> ClickAction;

    public void OnSelect()
    {
        Debug.Log($"OnSelect:{message.text}");
        //selectDotImage.SetActive(true);
    }
    public void OnDeselect()
    {
        Debug.Log($"OnDeselect:{message.text}");
        //selectDotImage.SetActive(false);
    }
    void Click()
    {
        ClickAction?.Invoke(message.text);
    }
    public void Init(string itemName, Action<string> action)
    {
        selectDotImage.SetActive(false);
        ClickAction = action;
        message.text = itemName;
        string logoPath = AssetUtility.GetDetailDataFolder(itemName) + "icon.jpg";

        //判断文件路径是否存在
        if (File.Exists(logoPath))
        {
            ResourceManager.Instance.GetTexture(logoPath, (t) =>
            {
                image.texture = t;
            });
        }
        else
        {
            logoPath = AssetUtility.GetDetailDataFolder(itemName) + "icon.png";
            if (File.Exists(logoPath))
            {
                ResourceManager.Instance.GetTexture(logoPath, (t) =>
                {
                    image.texture = t;
                });
            }
        }
    }


    public void Select()
    {
        button.Select();
        button.transform.localScale = Vector3.one * 1.1f;
    }
    public Action<MainItem> SelectAction;
    public void OnSelect(BaseEventData eventData)
    {
        SelectAction?.Invoke(this);
    }

}
