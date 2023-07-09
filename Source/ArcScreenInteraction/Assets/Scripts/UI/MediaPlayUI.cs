using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        ShowMedia();
    }

    private float innerDelay
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.innerDelay;
        }
    }
    private float outerDelay
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.outerDelay;
        }
    }
    private LoopType loopType
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.loopType;
        }
    }

    public void ShowMedia()
    {
        Debug.Log("ShowMedia:" + currentMediaData.MediaType);
        Debug.Log("ShowMedia:" + currentMediaData.title);
        Debug.Log("ShowMedia:" + currentMediaData.MediaPathList.Count);
        switch (currentMediaData.MediaType)
        {
            case MediaType.pdf:
                break;
            case MediaType.video:
                break;
            case MediaType.picture:
                ShowImageMedia();
                break;
        }
        ShowUI();
    }

    public void ShowImageMedia()
    {
        ResourceManager.Instance.GetTextureList(currentMediaData.MediaPathList, (data) =>
        {
            Debug.Log("图片加载完成" + data.Count);
            StartCoroutine(PlayImages(data));
        });
    }

    private IEnumerator PlayImages(List<Texture2D> textures)
    {
        for (int i = 0; i < textures.Count; i++)
        {
            mediaImage.texture = textures[i];
            mediaImage.DOFade(1, 0.2f);
            yield return new WaitForSeconds(innerDelay);
            mediaImage.DOFade(0, 0.2f);
            yield return new WaitForSeconds(0.2f);

        }
    }

}
