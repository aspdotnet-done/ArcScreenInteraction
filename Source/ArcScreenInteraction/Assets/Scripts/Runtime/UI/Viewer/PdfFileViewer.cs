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

public class PdfFileViewer : BaseViewer
{
    [SerializeField] ScrollViewPDF scrollView = default;
    [SerializeField] Button prevCellButton = default;
    [SerializeField] Button nextCellButton = default;

    [SerializeField] private GameObject pdfPagePrefab;
    [SerializeField] private Transform pdfPageParent;
    [SerializeField] private Button hotPointButton;
    [SerializeField] private PdfVideoView pdfVideoView;
    private PDFDocument pdfDocument;
    public HorizontalScrollSnap scrollSnap;

    [SerializeField] GameEvent nextPageEvent;
    [SerializeField] GameEvent prevPageEvent;

    private List<PDFVideoData> videoDatas;
    void OnSelectionChanged(int index)
    {
        currentPage = index;
        scrollSnap.ChangePage(currentPage);
        if (GetDataFromPage(index + 1) != null)
        {
            hotPointButton.gameObject.SetActive(true);
        }
        else
        {
            hotPointButton.gameObject.SetActive(false);
        }
    }

    private void OnHotPointClick()
    {
        pdfVideoView.gameObject.SetActive(true);
        pdfVideoView.InitVideo(GetDataFromPage(currentPage + 1));
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.canReturn = false;
    }
    bool firstIn = true;
    private void ConfirmAction()
    {
        if (firstIn)
        {
            firstIn = false;
            return;
        }

        Debug.Log("confirm1");
        if (!pdfVideoView.gameObject.activeSelf && GetDataFromPage(currentPage + 1) != null)
        {
            Debug.Log("confirm");
            OnHotPointClick();
        }
    }

    public void SelectPage(int page)
    {
        currentPage = page;
        scrollView.SelectCell(page);
    }

    public override void Show()
    {
        //Debug.Log("showpdf");
        firstIn = true;
        AppManager.Instance.EnterAction += ConfirmAction;
        prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(OnSelectionChanged);
        scrollSnap.OnSelectionPageChangedEvent.AddListener(SelectPage);

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
            page.GetComponentInChildren<RawImage>().SetNativeSize();
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
        if (hasVideo && GetDataFromPage(1) != null)
        {
            hotPointButton.gameObject.SetActive(true);
        }
        else
        {
            hotPointButton.gameObject.SetActive(false);
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
        AppManager.Instance.EnterAction -= ConfirmAction;
        //Debug.Log("hidepdf");
        canvasGroup.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
        prevCellButton.onClick.RemoveAllListeners();
        nextCellButton.onClick.RemoveAllListeners();
        scrollView.OnSelectionChanged(null);
        scrollSnap.OnSelectionPageChangedEvent.RemoveAllListeners();
        int cout = pdfPageParent.childCount;
        Debug.Log("cout:" + cout);
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
