using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Paroxe.PdfRenderer;
using DG.Tweening;
using UnityEngine.UI;
using ItemDataPDF = UnityEngine.UI.Extensions.Examples.FancyScrollViewExample02.ItemData;
using ScrollViewPDF = UnityEngine.UI.Extensions.Examples.FancyScrollViewExample02.ScrollView;
using HorizontalScrollSnap = UnityEngine.UI.Extensions.HorizontalScrollSnap;
public class PdfFileViewer : BaseViewer
{
    [SerializeField] ScrollViewPDF scrollView = default;
    [SerializeField] Button prevCellButton = default;
    [SerializeField] Button nextCellButton = default;

    [SerializeField] private GameObject pdfPagePrefab;
    [SerializeField] private Transform pdfPageParent;
    private PDFDocument pdfDocument;
    public HorizontalScrollSnap scrollSnap;


    void OnSelectionChanged(int index)
    {
        currentPage = index;
        scrollSnap.ChangePage(currentPage);
    }

    public void SelectPage(int page)
    {
        currentPage = page;
        scrollView.SelectCell(page);
    }

    public override void Show()
    {
        //Debug.Log("showpdf");
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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            lastClickTime = 0;
            timer = 0;
        }
        if (Time.time - lastClickTime > activeTime)
        {

            timer += Time.deltaTime;
            if (timer > innerDelay)
            {
                timer = 0;
                if (currentPage < totalPage - 1)
                {
                    scrollView.SelectCell(currentPage + 1);
                }
                else
                {
                    currentPlayState = PlayState.Complete;
                }
            }

        }
    }


    private int currentPage = 0;
    private int totalPage;
    private List<ItemDataPDF> items;
    private void LoadPdfComplete(PDFDocument doc)
    {
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
