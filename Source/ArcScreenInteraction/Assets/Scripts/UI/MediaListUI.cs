using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaListUI : UI
{
    private Button hideBtn;
    public Button HideBtn
    {
        get
        {
            if (hideBtn == null) hideBtn = transform.Find("Hide").GetComponent<Button>();
            return hideBtn;
        }
    }

    private void OnEnable()
    {
        HideBtn.onClick.AddListener(HideClick);
    }

    private void HideClick()
    {
        HideUI();
    }

    private void OnDisable()
    {
        HideBtn.onClick.RemoveListener(HideClick);
    }

}
