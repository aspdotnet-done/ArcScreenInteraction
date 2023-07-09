using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaPlayUI : UI
{
    [SerializeField] private RawImage mediaImage = default;
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
    private MediaData currentMediaData;
    public void Init(MediaData data)
    {
        currentMediaData = data;
    }

    public void ShowMedia()
    {
        switch (currentMediaData.MediaType)
        {
            case MediaType.pdf:
                break;
            case MediaType.video:
                break;
            case MediaType.picture:
                break;
        }
    }

    public void ShowImageMedia()
    {
        ResourceManager.Instance.GetTextureList(currentMediaData.MediaPathList, (data) =>
        {

        });
    }
}
