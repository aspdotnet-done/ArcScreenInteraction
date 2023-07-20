using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Paroxe.PdfRenderer;
using DG.Tweening;
using UnityEngine.UI;
using ItemDataPDF = UnityEngine.UI.Extensions.Examples.FancyScrollViewExample02.ItemData;
using ScrollViewPDF = UnityEngine.UI.Extensions.Examples.FancyScrollViewExample02.ScrollView;
public class PdfFileViewer : BaseViewer
{
    [SerializeField] private RawImage pdfImage;
    [SerializeField] ScrollViewPDF scrollView = default;
    [SerializeField] Button prevCellButton = default;
    [SerializeField] Button nextCellButton = default;
    private PDFDocument pdfDocument;

    private void Start()
    {
        prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(OnSelectionChanged);
    }

    void OnSelectionChanged(int index)
    {
        currentPage = index;
        pdfImage.texture = items[index].texture;
        pdfImage.SetNativeSize();
        Debug.Log("Selected item info: index " + index);
    }
    public override void Next()
    {
        base.Next();
    }
    public override void Previous()
    {
        base.Previous();

    }
    public override void Play()
    {
        base.Play();

    }
    public override void Pause()
    {
        base.Pause();

    }
    public override void Return()
    {
        base.Return();

    }

    public override void Show()
    {
        base.Show();

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
        canvasGroup.DOFade(1, 0.2f);
        items = new List<ItemDataPDF>();
        for (int i = 0; i < totalPage; i++)
        {
            items.Add(new ItemDataPDF((i + 1).ToString(), GetPageTexture(i)));
        }

        scrollView.UpdateData(items);
        scrollView.SelectCell(0);
    }

    public void NextPage()
    {
        if (currentPage < totalPage - 1)
        {
            currentPage++;
            pdfImage.texture = GetPageTexture(currentPage);
            pdfImage.SetNativeSize();
        }
    }
    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            pdfImage.texture = GetPageTexture(currentPage);
            pdfImage.SetNativeSize();
        }
    }

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
    }


}
