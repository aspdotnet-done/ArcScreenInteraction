using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using RenderHeads.Media.AVProVideo.Demos;
using Button = UnityEngine.UI.Button;

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

    public bool isOncePlay
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.loopType == LoopType.LoopOnce;
        }
    }

    public bool isSingle
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.loopType == LoopType.Single;
        }
    }

    [SerializeField]
    private MediaPlayerUI mediaPlayerUI;

    private void Awake()
    {
        HideBtn.onClick.AddListener(HideClick);
        LastBtn.onClick.AddListener(LastClick);
        NextBtn.onClick.AddListener(NextCleck);
        StartCoroutine(WaitForInit());

    }
    IEnumerator WaitForInit()
    {
        yield return new WaitForSeconds(0.1f);
        AppManager.Instance.EnterAction += mediaPlayerUI.TogglePlayPause;
        AppManager.Instance.LeftAction += QuickForward;
        AppManager.Instance.RightAction += QuickBack;
    }


    void QuickForward()
    {
        mediaPlayerUI.SeekRelative(mediaPlayerUI._jumpDeltaTime);
    }
    void QuickBack()
    {
        mediaPlayerUI.SeekRelative(-mediaPlayerUI._jumpDeltaTime);
    }


    float timer = 0;
    private float lastClickTime;
    //点击后自动播放的激活时间
    public float activeTime = 5f;
    private void Update()
    {
        if (isSingle)
            return;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
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
                    if (CheckIsLast())//最后一个
                    {
                        if (!isLoopPlay)
                        {
                            //结束 返回首页
                            HideClick();
                        }
                        else
                        {
                            NextCleck();
                        }
                        //结束 返回首页
                        //HideClick();
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
    [HideInInspector]
    public bool canReturn = true;
    public void SetCanReturn()
    {
        StartCoroutine(WaitForCanReturn());
    }

    IEnumerator WaitForCanReturn()
    {
        yield return new WaitForSeconds(0.1f);
        canReturn = true;
    }
    public override void OnBack()
    {
        if (!canReturn) return;

        if (!gameObject.activeSelf) return;
        HideUI();
        MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
        ui.ShowUI();
    }


    private List<Media> currentDatas;
    private int currentIndex = 0;
    public void Init(List<Media> datas, Media media)
    {
        lastClickTime = 0;
        timer = 0;
        Debug.Log("InitMedia");
        //获取media在datas中的索引
        int index = datas.FindIndex((item) =>
        {
            return item.mediaName == media.mediaName;
        });
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
        EventSystem.current.SetSelectedGameObject(null);
    }

    public override void HideUI()
    {
        base.HideUI();
        pDFViewer.pdfVideoView.canvasGroup.alpha = 0;
        pDFViewer.pdfVideoView.canvasGroup.gameObject.SetActive(false);
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
