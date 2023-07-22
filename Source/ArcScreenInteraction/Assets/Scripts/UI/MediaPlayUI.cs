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
    [SerializeField] public PdfFileViewer pDFViewer;
    [SerializeField] public VideoViewer videoViewer;
    [SerializeField] public CanvasGroup canvasBar;
    [SerializeField] public Text mideaTitle;
    private Button hideBtn;
    public Button HideBtn
    {
        get
        {
            if (hideBtn == null) hideBtn = transform.Find("PlayBar/Return").GetComponent<Button>();
            return hideBtn;
        }
    }

    private Button lastBtn;
    public Button LastBtn
    {
        get
        {
            if (lastBtn == null) lastBtn = transform.Find("PlayBar/Left").GetComponent<Button>();
            return lastBtn;
        }
    }

    private Button nextBtn;
    public Button NextBtn
    {
        get
        {
            if (nextBtn == null) nextBtn = transform.Find("PlayBar/Right").GetComponent<Button>();
            return nextBtn;
        }
    }
    public bool isLoopPlay
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.loopType == LoopType.Loop;
        }
    }



    private void Start()
    {
        HideBtn.onClick.AddListener(HideClick);
        LastBtn.onClick.AddListener(LastClick);
        NextBtn.onClick.AddListener(NextCleck);
    }
    private void LastClick()
    {
        if (CheckIsFirst())
        {
            if (isLoopPlay)
            {
                currentIndex = currentDatas.Count - 1;
                ShowMedia();
            }
            else
            {
                Debug.Log("前面没有了");
                return;
            }
        }
        else
        {
            currentIndex -= 1;
            ShowMedia();
        }
    }
    private void NextCleck()
    {
        if (CheckIsLast())
        {
            if (isLoopPlay)
            {
                currentIndex = 0;
                ShowMedia();
            }
            else
            {
                Debug.Log("后面没有了");
                return;
            }
        }
        else
        {
            currentIndex += 1;
            ShowMedia();
        }
    }

    private bool CheckIsLast()
    {
        return currentDatas.Count == currentIndex + 1;
    }
    private bool CheckIsFirst()
    {
        return currentIndex == 0;
    }


    private void HideClick()
    {
        HideUI();
        MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
        ui.ShowUI();
    }

    // private void OnDisable()
    // {
    //     HideBtn.onClick.RemoveListener(HideClick);
    //     LastBtn.onClick.RemoveListener(LastClick);
    //     NextBtn.onClick.RemoveListener(NextCleck);
    // }
    private List<Media> currentDatas;
    private int currentIndex = 0;
    public void Init(List<Media> datas, int index)
    {
        Debug.Log("InitMedia");
        currentDatas = datas;
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
        Debug.Log("ShowMedia");
        pDFViewer.Hide();
        imageViewer.Hide();
        videoViewer.Hide();
        switch (currentDatas[currentIndex].mediaType)
        {
            case MediaType.pdf:
                viewer = pDFViewer;
                pDFViewer.LoadMedias(currentDatas[currentIndex]);
                pDFViewer.Show();
                break;
            case MediaType.video:
                viewer = videoViewer;
                videoViewer.LoadMedias(currentDatas[currentIndex]);
                videoViewer.Show();
                break;
            case MediaType.picture:
                viewer = imageViewer;
                imageViewer.LoadMedias(currentDatas[currentIndex]);
                imageViewer.Show();
                break;
        }
        mideaTitle.text = currentDatas[currentIndex].mediaName;
        //viewer.LoadMedias(currentDatas[currentIndex]);
        ShowUI();
    }

    // override public void ShowUI()
    // {
    //     base.ShowUI();
    //     if (viewer != null)
    //         viewer.Show();
    // }
    public override void HideUI()
    {
        base.HideUI();
        // if (viewer != null)
        //     viewer.Hide();
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



}
