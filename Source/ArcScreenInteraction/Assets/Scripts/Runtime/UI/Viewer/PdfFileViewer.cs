using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Paroxe.PdfRenderer;
using DG.Tweening;
using UnityEngine.UI;
using ItemDataPDF = UnityEngine.UI.Extensions.Examples.FancyScrollViewExample02.ItemData;
using ScrollViewPDF = UnityEngine.UI.Extensions.Examples.FancyScrollViewExample02.ScrollView;
using HorizontalScrollSnap = UnityEngine.UI.Extensions.HorizontalScrollSnap;
using ScriptableObjectArchitecture;
using UnityEngine.EventSystems;
using System;

public class PdfFileViewer : BaseViewer
{
    [SerializeField] public ScrollViewPDF scrollView = default;
    [SerializeField] Button prevCellButton = default;
    [SerializeField] Button nextCellButton = default;

    [SerializeField] private GameObject pdfPagePrefab;
    [SerializeField] private Transform pdfPageParent;
    [SerializeField] private AutoFit hotPointTitle;
    [SerializeField] public PdfVideoView pdfVideoView;
    private PDFDocument pdfDocument;
    public HorizontalScrollSnap scrollSnap;

    [SerializeField] GameEvent nextPageEvent;
    [SerializeField] GameEvent prevPageEvent;

    private List<PDFVideoData> videoDatas;
    void OnSelectionChanged(int index)
    {

        currentPage = index;
        Debug.Log("当前页码:" + currentPage);
        scrollSnap.ChangePage(currentPage);
        PDFVideoData d = GetDataFromPage(index + 1);
        if (d != null)
        {
            hotPointTitle.SetContent(d.Title);
            hotPointTitle.GetComponent<CanvasGroup>().alpha = 0;
            hotPointTitle.gameObject.SetActive(true);
            hotPointTitle.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
            if (!string.IsNullOrEmpty(d.Cover))
            {
                ResourceManager.Instance.GetTexture(d.Cover, (t) =>
                {
                    hotPointTitle.rawImage.texture = t;
                });
            }
        }
        else
        {
            hotPointTitle.gameObject.SetActive(false);
        }
    }

    private void OnHotPointClick()
    {
        pdfVideoView.gameObject.SetActive(true);
        pdfVideoView.InitVideo(GetDataFromPage(currentPage + 1));
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.canReturn = false;
    }
    [HideInInspector]
    public bool firstIn = true;
    private void ConfirmAction()
    {
        // if (GetDataFromPage(currentPage + 1) == null || pdfVideoView.canvasGroup.alpha > 0) return;
        // Debug.Log(2);
        // OnHotPointClick();
        // EventSystem.current.SetSelectedGameObject(null);
        try
        {
            Invoke("WaitAFrame", 0.05f);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }
    void WaitAFrame()
    {
        //yield return new WaitForEndOfFrame();
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        if (ui.CurrentState == UIState.Hide) return;
        if (GetDataFromPage(currentPage + 1) != null && pdfVideoView.canvasGroup.alpha == 0)
        {

            Debug.Log(2);

            OnHotPointClick();
            //scrollView.enabled = false;
            //获取当前uinavigation的焦点
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void SelectPage(int page)
    {
        currentPage = page;
        scrollView.SelectCell(page);
        scrollView.transform.GetChild(0).GetChild(page).GetComponentInChildren<Button>().Select();
    }

    public override void Show()
    {
        //Debug.Log("showpdf");
        AppManager.Instance.EnterAction += ConfirmAction;
        prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(OnSelectionChanged);
        scrollSnap.OnSelectionPageChangedEvent.AddListener(SelectPage);
        currentPage = 0;
        ResourceManager.Instance.GetPDFData(currentData.mediaPath, LoadPdfComplete);
    }
    private float lastClickTime;
    //点击后自动播放的激活时间
    public float activeTime = 5f;
    float timer = 0;
    private bool allowAutoPlay
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.allowAutoPlay;
        }
    }
    private void Update()
    {
        if (allowAutoPlay)
        {
            if (GetInput())
            {
                ResetTimer();
            }
            if (Time.time - lastClickTime > activeTime)
            {

                timer += Time.deltaTime;
                if (timer > innerDelay)
                {
                    timer = 0;
                    NextPage();
                }
            }
        }
    }
    private void OnEnable()
    {
        nextPageEvent.AddListener(NextPage);
        prevPageEvent.AddListener(PrevPage);
    }
    private void OnDisable()
    {
        nextPageEvent.RemoveListener(NextPage);
        prevPageEvent.RemoveListener(PrevPage);
    }
    private bool GetInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            return true;
        }
        return false;
    }
    private int currentPage = 0;
    private int totalPage;
    private List<ItemDataPDF> items;
    private void LoadPdfComplete(PDFDocument doc)
    {
        //判断是否有视频
        (bool hasVideo, string jsonPath) = ResourceManager.Instance.CheckPdfVideoJsonExist(currentData.mediaPath);
        if (hasVideo)
        {
            videoDatas = ResourceManager.Instance.GetPDFVideoDatasFromJson(jsonPath);
        }
        //初始化pdf
        currentPage = 0;
        pdfDocument = doc;
        totalPage = doc.GetPageCount();

        items = new List<ItemDataPDF>();
        for (int i = 0; i < totalPage; i++)
        {
            Texture2D tex = GetPageTexture(i);
            GameObject page = Instantiate(pdfPagePrefab, pdfPageParent);
            page.GetComponentInChildren<RawImage>().texture = tex;
            //page.GetComponentInChildren<RawImage>().SetNativeSize();
            page.GetComponentInChildren<ImageFitter>().targetRect = page.GetComponent<RectTransform>();
            page.GetComponentInChildren<ImageFitter>().Fit();
            ResizeImage(page.GetComponentInChildren<RawImage>());
            page.SetActive(true);
            items.Add(new ItemDataPDF((i + 1).ToString(), tex));
        }

        scrollView.UpdateData(items);
        scrollView.SelectCell(0);
        scrollView.transform.GetChild(0).GetChild(0).GetComponentInChildren<Button>().Select();
        gameObject.SetActive(true);
        canvasGroup.DOFade(1, 1f).SetDelay(0.5f);
        currentPlayState = PlayState.Playing;
        PDFVideoData d = GetDataFromPage(1);
        if (hasVideo && d != null)
        {
            hotPointTitle.SetContent(d.Title);
            hotPointTitle.GetComponent<CanvasGroup>().alpha = 0;
            hotPointTitle.gameObject.SetActive(true);
            hotPointTitle.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
            if (!string.IsNullOrEmpty(d.Cover))
            {
                ResourceManager.Instance.GetTexture(d.Cover, (t) =>
                {
                    hotPointTitle.rawImage.texture = t;
                });
            }
        }
        else
        {
            hotPointTitle.gameObject.SetActive(false);
        }
    }

    PDFVideoData GetDataFromPage(int page)
    {
        if (videoDatas == null) return null;
        foreach (var p in videoDatas)
        {
            if (p.PageIndex == page)
            {
                return p;
            }
        }
        return null;
    }

    public void NextPage()
    {
        ResetTimer();
        if (currentPage < totalPage - 1)
        {
            scrollView.SelectCell(currentPage + 1);
        }
        else
        {
            currentPlayState = PlayState.Complete;
        }
    }
    public void PrevPage()
    {
        ResetTimer();
        if (currentPage > 0)
        {
            scrollView.SelectCell(currentPage - 1);
        }
    }
    private void ResetTimer()
    {
        lastClickTime = 0;
        timer = 0;
    }
    private Texture2D GetPageTexture(int index, float scale = 2f)
    {
        PDFRenderer renderer = new PDFRenderer();
        //Debug.Log("Page:" + index);
        Texture2D tex = renderer.RenderPageToTexture(pdfDocument.GetPage(index), (int)(pdfDocument.GetPage(index).GetPageSize().x * scale), (int)(pdfDocument.GetPage(index).GetPageSize().y * scale));

        tex.filterMode = FilterMode.Bilinear;
        tex.anisoLevel = 8;
        renderer.Dispose();
        return tex;
    }
    private PDFPage GetPage(int index)
    {
        return pdfDocument.GetPage(index);
    }


    public override void Hide()
    {
        currentPage = 999;
        AppManager.Instance.EnterAction -= ConfirmAction;
        //Debug.Log("hidepdf");
        canvasGroup.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
        prevCellButton.onClick.RemoveAllListeners();
        nextCellButton.onClick.RemoveAllListeners();
        scrollView.OnSelectionChanged(null);
        scrollSnap.OnSelectionPageChangedEvent.RemoveAllListeners();
        int cout = pdfPageParent.childCount;
        for (int i = 0; i < cout; i++)
        {
            Destroy(pdfPageParent.GetChild(i).gameObject);
        }
        gameObject.SetActive(false);
    }
    public float maxHeight = 1080f;


    private void ResizeImage(RawImage image)
    {

        float aspectRatio = (float)image.texture.width / (float)image.texture.height;
        //float newHeight = Mathf.Min(maxHeight, image.texture.height);
        float newHeight = maxHeight;
        float newWidth = newHeight * aspectRatio;
        // if (newHeight < image.texture.height)
        // {
        image.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        // }
    }
    private float innerDelay
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.innerDelay;
        }
    }

}
