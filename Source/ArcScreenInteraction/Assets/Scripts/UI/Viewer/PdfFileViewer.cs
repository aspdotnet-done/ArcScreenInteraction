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
        // pdfImage.texture = items[index].texture;
        // pdfImage.SetNativeSize();
        // Debug.Log("Selected item info: index " + index);
    }

    public void SelectPage(int page)
    {
        scrollView.SelectCell(page);
    }

    public override void Show()
    {

        prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(OnSelectionChanged);
        scrollSnap.OnSelectionPageChangedEvent.AddListener(SelectPage);

        ResourceManager.Instance.GetPDFData(currentData.mediaPath, LoadPdfComplete);

        currentPlayState = PlayState.Playing;
    }
    private int currentPage = 0;
    private int totalPage;
    private List<ItemDataPDF> items;
    private void LoadPdfComplete(PDFDocument doc)
    {
        currentPage = 0;
        pdfDocument = doc;
        totalPage = doc.GetPageCount();
        // pdfImage.texture = GetPageTexture(currentPage);
        // pdfImage.SetNativeSize();

        items = new List<ItemDataPDF>();
        for (int i = 0; i < totalPage; i++)
        {
            Texture2D tex = GetPageTexture(i);
            GameObject page = Instantiate(pdfPagePrefab, pdfPageParent);
            page.GetComponentInChildren<RawImage>().texture = tex;
            page.GetComponentInChildren<RawImage>().SetNativeSize();
            page.SetActive(true);
            items.Add(new ItemDataPDF((i + 1).ToString(), tex));
        }

        scrollView.UpdateData(items);
        scrollView.SelectCell(0);
        base.Show();
        canvasGroup.DOFade(1, 0.2f);
    }

    // public void NextPage()
    // {
    //     if (currentPage < totalPage - 1)
    //     {
    //         currentPage++;
    //         pdfImage.texture = GetPageTexture(currentPage);
    //         pdfImage.SetNativeSize();
    //     }
    // }
    // public void PreviousPage()
    // {
    //     if (currentPage > 0)
    //     {
    //         currentPage--;
    //         pdfImage.texture = GetPageTexture(currentPage);
    //         pdfImage.SetNativeSize();
    //     }
    // }

    private Texture2D GetPageTexture(int index, float scale = 2f)
    {
        PDFRenderer renderer = new PDFRenderer();
        Debug.Log("Page:" + index);
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
        base.Hide();
        canvasGroup.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
        prevCellButton.onClick.RemoveListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.RemoveListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(null);
        scrollSnap.OnSelectionPageChangedEvent.RemoveListener(SelectPage);
        int cout = pdfPageParent.childCount;
        for (int i = 0; i < cout; i++)
        {
            Destroy(pdfPageParent.GetChild(0).gameObject);
        }
    }


}
