using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MediaPlayUI : UI
{
    [HideInInspector]
    public BaseViewer viewer;
    [SerializeField] public ImageViewer imageViewer;
    [SerializeField] public PDFViewer pDFViewer;
    [SerializeField] public VideoViewer videoViewer;
    [SerializeField] public CanvasGroup canvasBar;
    private Button hideBtn;
    public Button HideBtn
    {
        get
        {
            if (hideBtn == null) hideBtn = transform.Find("Hub/PlayBar/Return").GetComponent<Button>();
            return hideBtn;
        }
    }

    private Button lastBtn;
    public Button LastBtn
    {
        get
        {
            if (lastBtn == null) hideBtn = transform.Find("Hub/PlayBar/Left").GetComponent<Button>();
            return lastBtn;
        }
    }

    private Button nextBtn;
    public Button NextBtn
    {
        get
        {
            if (nextBtn == null) hideBtn = transform.Find("Hub/PlayBar/Left").GetComponent<Button>();
            return nextBtn;
        }
    }



    private void OnEnable()
    {
        HideBtn.onClick.AddListener(HideClick);
        LastBtn.onClick.AddListener(LastClick);
        NextBtn.onClick.AddListener(NextCleck);
    }
    private void LastClick()
    {

    }
    private void NextCleck()
    {

    }


    private void HideClick()
    {
        HideUI();
        MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
        ui.ShowUI();
    }

    private void OnDisable()
    {
        HideBtn.onClick.RemoveListener(HideClick);
    }
    private MediaData currentMediaData;
    private int currentIndex = 0;
    public void Init(MediaData data, int index)
    {
        currentMediaData = data;
        currentIndex = index;

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
        // Debug.Log("ShowMedia:" + currentMediaData.MediaType);
        // Debug.Log("ShowMedia:" + currentMediaData.title);
        // Debug.Log("ShowMedia:" + currentMediaData.MediaPathList.Count);
        switch (currentMediaData.medias[currentIndex].mediaType)
        {
            case MediaType.pdf:
                viewer = pDFViewer;
                break;
            case MediaType.video:
                viewer = videoViewer;
                break;
            case MediaType.picture:
                viewer = imageViewer;
                break;
        }

        viewer.LoadMedias(currentMediaData, currentIndex);
        ShowUI();
    }

    override public void ShowUI()
    {
        base.ShowUI();
        viewer.gameObject.SetActive(true);
    }
    public override void HideUI()
    {
        base.HideUI();
        if (viewer != null)
            viewer.Hide();
    }

    private void HidePlayBar()
    {
        canvasBar.DOFade(0, 0.2f).OnComplete(() =>
        {
            canvasBar.gameObject.SetActive(false);
        });
    }

    private void ShowPlayBar()
    {
        canvasBar.gameObject.SetActive(true);
        canvasBar.DOFade(1, 0.2f).OnComplete(() =>
        {
            canvasBar.gameObject.SetActive(false);
        });
    }

    //定义一个队列来存储待播放数据
    // private Queue<MediaData> mediaDataQueue = new Queue<MediaData>();
    // public void InitPlayQueue(MediaData data)
    // {
    //     bool isLoop = false;
    //     switch (loopType)
    //     {
    //         case LoopType.SinglePlay:
    //             mediaDataQueue.Enqueue(data);
    //             break;
    //         case LoopType.SingleLoop:
    //             isLoop = true;
    //             mediaDataQueue.Enqueue(data);
    //             break;
    //         case LoopType.OrderPlay:
    //             MediaManager.Instance.SortMediaList(data, (v) =>
    //             {
    //                 //把v导入队列
    //                 foreach (var i in v)
    //                     mediaDataQueue.Enqueue(i);
    //             });
    //             break;
    //         case LoopType.AllLoop:
    //             MediaManager.Instance.SortMediaList(data, (v) =>
    //             {
    //                 isLoop = true;
    //                 //把v导入队列
    //                 foreach (var i in v)
    //                     mediaDataQueue.Enqueue(i);
    //             });
    //             break;
    //         default:
    //             break;
    //     }
    //     StartCoroutine(PlayImages(isLoop));

    // }

    // private IEnumerator PlayImages(bool isLoop = false)
    // {
    //     while (mediaDataQueue.Count > 0)
    //     {
    //         var temp = mediaDataQueue.Dequeue();
    //         List<Texture2D> textures = new List<Texture2D>();
    //         ResourceManager.Instance.GetTextureList(temp.MediaPathList, (v) =>
    //         {
    //             textures = v;
    //         });
    //         //直到textures加载完成
    //         yield return new WaitUntil(() => textures.Count == temp.MediaPathList.Count);
    //         for (int i = 0; i < currentMediaData.MediaPathList.Count; i++)
    //         {
    //             ResourceManager.Instance.GetTexture(currentMediaData.MediaPathList[i], (v) =>
    //             {
    //                 // mediaImage.texture = v;
    //                 // mediaImage.DOFade(1, 0.2f);
    //             });
    //             yield return new WaitForSeconds(innerDelay);
    //         }
    //         if (isLoop)//如果循环的话，把temp再放回队列
    //         {
    //             mediaDataQueue.Enqueue(temp);
    //             yield return new WaitForSeconds(outerDelay);
    //         }
    //     }
    //     //播放完毕
    //     HideUI();
    // }


}
