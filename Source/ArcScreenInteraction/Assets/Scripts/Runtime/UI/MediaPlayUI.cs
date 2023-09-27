using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using RenderHeads.Media.AVProVideo.Demos;
using Button = UnityEngine.UI.Button;
using ScriptableObjectArchitecture;

public class MediaPlayUI : UI
{
    [HideInInspector]
    public BaseViewer viewer;
    [SerializeField] public ImageViewer imageViewer;
    [SerializeField] public PdfFileViewer pDFViewer;
    [SerializeField] public VideoViewer videoViewer;
    [SerializeField] public MonitorViewer monitorViewer;
    [SerializeField] public CanvasGroup canvasBar;
    [SerializeField] public Text mideaTitle;
    [SerializeField] private GameEvent leftEvent;
    [SerializeField] private GameEvent rightEvent;
    [SerializeField] private GameEvent submitEvent;
    public bool isLoopPlay
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.loopType == LoopType.Loop;
        }
    }
    [SerializeField]
    private MediaPlayerUI mediaPlayerUI;
    protected override void OnEnable()
    {
        base.OnEnable();
        leftEvent.AddListener(QuickForward);
        rightEvent.AddListener(QuickBack);
        submitEvent.AddListener(mediaPlayerUI.TogglePlayPause);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        leftEvent.RemoveListener(QuickForward);
        rightEvent.RemoveListener(QuickBack);
        submitEvent.RemoveListener(mediaPlayerUI.TogglePlayPause);
    }
    void QuickForward()
    {
        mediaPlayerUI.SeekRelative(mediaPlayerUI._jumpDeltaTime);
    }
    void QuickBack()
    {
        mediaPlayerUI.SeekRelative(-mediaPlayerUI._jumpDeltaTime);
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
        Debug.Log("OnBack");
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
        int index = datas.FindIndex((item) =>
        {
            return item.mediaName == media.mediaName;
        });
        currentDatas = datas;
        currentIndex = index;
        ShowMedia();
    }
    public void ShowMedia()
    {
        ShowUI();
        Debug.Log("ShowMedia");
        HideAllViewer();
        var selectedMedia = currentDatas[currentIndex];
        viewer = SelectedViewer(selectedMedia.mediaType);
        viewer.currentPlayState = PlayState.Init;
        viewer.LoadMedia(selectedMedia);
        viewer.Show();
        mideaTitle.text = selectedMedia.mediaName;
        EventSystem.current.SetSelectedGameObject(null);
    }
    private BaseViewer SelectedViewer(MediaType mediaType)
    {
        switch (mediaType)
        {
            case MediaType.PDF:
                return pDFViewer;
            case MediaType.VIDEO:
                return videoViewer;
            case MediaType.PICTURE:
                return imageViewer;
            case MediaType.MONITOR:
                return monitorViewer;
        }
        return null;
    }
    public override void HideUI()
    {
        base.HideUI();
        pDFViewer.pdfVideoView.canvasGroup.alpha = 0;
        pDFViewer.pdfVideoView.canvasGroup.gameObject.SetActive(false);
    }
    private void HideAllViewer()
    {
        pDFViewer.Hide();
        imageViewer.Hide();
        videoViewer.Hide();
        monitorViewer.Hide();
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
