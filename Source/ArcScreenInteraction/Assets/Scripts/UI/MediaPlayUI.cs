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
                InitPlayQueue(currentMediaData);
                break;
        }
        ShowUI();
    }

    //定义一个队列来存储待播放数据
    private Queue<MediaData> mediaDataQueue = new Queue<MediaData>();
    public void InitPlayQueue(MediaData data)
    {
        bool isLoop = false;
        switch (loopType)
        {
            case LoopType.SinglePlay:
                mediaDataQueue.Enqueue(data);
                break;
            case LoopType.SingleLoop:
                isLoop = true;
                mediaDataQueue.Enqueue(data);
                break;
            case LoopType.OrderPlay:
                MediaManager.Instance.SortMediaList(data, (v) =>
                {
                    //把v导入队列
                    foreach (var i in v)
                        mediaDataQueue.Enqueue(i);
                });
                break;
            case LoopType.AllLoop:
                MediaManager.Instance.SortMediaList(data, (v) =>
                {
                    isLoop = true;
                    //把v导入队列
                    foreach (var i in v)
                        mediaDataQueue.Enqueue(i);
                });
                break;
            default:
                break;
        }
        StartCoroutine(PlayImages(isLoop));

    }

    private IEnumerator PlayImages(bool isLoop = false)
    {
        while (mediaDataQueue.Count > 0)
        {
            var temp = mediaDataQueue.Dequeue();
            List<Texture2D> textures = new List<Texture2D>();
            ResourceManager.Instance.GetTextureList(temp.MediaPathList, (v) =>
            {
                textures = v;
            });
            //直到textures加载完成
            yield return new WaitUntil(() => textures.Count == temp.MediaPathList.Count);
            for (int i = 0; i < currentMediaData.MediaPathList.Count; i++)
            {
                ResourceManager.Instance.GetTexture(currentMediaData.MediaPathList[i], (v) =>
                {
                    mediaImage.texture = v;
                    mediaImage.DOFade(1, 0.2f);
                });
                yield return new WaitForSeconds(innerDelay);
            }
            if (isLoop)//如果循环的话，把temp再放回队列
            {
                mediaDataQueue.Enqueue(temp);
                yield return new WaitForSeconds(outerDelay);
            }
        }
        //播放完毕
        HideUI();


    }


}
