using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.EventSystems;

public class CellView : MonoBehaviour, ISelectHandler
{
    [SerializeField] RawImage image = default;
    [SerializeField] Text message = default;
    [SerializeField] Button button = default;

    [SerializeField] Image iconImage = default;
    [SerializeField] MediaIconAsset mediaIconAsset;
    private Media currentMedia;
    private void Start()
    {
        button.onClick.AddListener(Click);
    }
    private Action<Media> ClickAction;
    void Click()
    {
        ClickAction?.Invoke(currentMedia);
    }
    public void Select()
    {
        button.Select();
        button.transform.localScale = Vector3.one * 1.1f;
    }


    public void Init(Media media, Action<Media> action)
    {
        currentMedia = media;
        ClickAction = action;
        message.text = currentMedia.mediaName;
        UpdateIcon(currentMedia.mediaType);
        //判断文件路径是否存在
        if (File.Exists(currentMedia.coverPath))
        {
            ResourceManager.Instance.GetTexture(currentMedia.coverPath, (t) =>
            {
                if (t == null) return;
                image.texture = t;
            });
        }
    }

    private void UpdateIcon(MediaType mediaType)
    {
        iconImage.sprite = mediaIconAsset.GetIcon(mediaType);
    }
    public Action<CellView> SelectAction;
    public void OnSelect(BaseEventData eventData)
    {
        SelectAction?.Invoke(this);
    }
}
