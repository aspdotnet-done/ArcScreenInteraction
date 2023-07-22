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
    float timer = 0;
    private float lastClickTime;
    //点击后自动播放的激活时间
    public float activeTime = 5f;
    public bool isCompleteAll = false;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = 0;
            timer = 0;
        }
        if (Time.time - lastClickTime > activeTime)
        {
            if (viewer.currentPlayState == PlayState.Complete)
            {
                timer += Time.deltaTime;
                if (timer > outerDelay)
                {
                    timer = 0;
                    if (CheckIsLast())
                    {
                        //结束 返回首页
                        HideClick();
                    }
                    else
                    {
                        NextCleck();
                    }
                }
            }
        }

    }
    private void LastClick()
    {
        lastClickTime = 0;
        timer = 0;
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
        lastClickTime = 0;
        timer = 0;
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
        lastClickTime = 0;
        timer = 0;
        ShowUI();
        Debug.Log("ShowMedia");
        pDFViewer.Hide();
        imageViewer.Hide();
        videoViewer.Hide();
        switch (currentDatas[currentIndex].mediaType)
        {
            case MediaType.pdf:
                viewer = pDFViewer;
                viewer.currentPlayState = PlayState.Init;
                pDFViewer.LoadMedias(currentDatas[currentIndex]);
                pDFViewer.Show();
                break;
            case MediaType.video:
                viewer = videoViewer;
                viewer.currentPlayState = PlayState.Init;
                videoViewer.LoadMedias(currentDatas[currentIndex]);
                videoViewer.Show();
                break;
            case MediaType.picture:
                viewer = imageViewer;
                viewer.currentPlayState = PlayState.Init;
                imageViewer.LoadMedias(currentDatas[currentIndex]);
                imageViewer.Show();
                break;
        }
        mideaTitle.text = currentDatas[currentIndex].mediaName;

    }

    public override void HideUI()
    {
        base.HideUI();
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
