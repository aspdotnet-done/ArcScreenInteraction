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
using System.Net.NetworkInformation;

public class PdfFileViewer : BaseViewer
{
    [SerializeField] private RawImage pdfPageRawImage;
    [SerializeField] private AutoFit hotPointTitle;
    [SerializeField] public PdfVideoView pdfVideoView;
    private PDFDocument pdfDocument;
    public int currentPage = 0;
    private int totalPage;
    private List<ItemDataPDF> items;

    public List<PDFVideoData> videoDatas;
    private bool pageHasHotPoint = false;
    private bool hotpointIsPlaying = false;

    #region override functions
    public override void Show()
    {
        currentPage = 0;
        pageHasHotPoint = false;
        hotpointIsPlaying = false;
        pdfVideoView.OnBack += PdfVideoView_OnBack;
        ResourceManager.Instance.GetPDFData(currentData.mediaPath, LoadPdfComplete);
    }
    public override void Hide()
    {
        currentPage = 999;
        canvasGroup.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
        gameObject.SetActive(false);
    }
    public override void Next()
    {
        base.Next();
        if (hotpointIsPlaying) return;
        NextPage();
    }
    public override void Previous()
    {
        base.Previous();
        if (hotpointIsPlaying) return;
        PrevPage();
    }
    public override void Play()
    {
        base.Play();
        OnHotPointClick();
    }
    public override void Return()
    {
        base.Return();
    }
    #endregion
    private void OnHotPointClick()
    {
        if (pageHasHotPoint && !hotpointIsPlaying)
        {
            hotpointIsPlaying = true;
            Debug.Log("OnHotPointClick");
            pdfVideoView.gameObject.SetActive(true);
            pdfVideoView.InitVideo(GetDataFromPage(currentPage + 1));
            MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
            ui.canReturn = false;
        }
    }
    private void PdfVideoView_OnBack()
    {
        hotpointIsPlaying = false;
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.SetCanReturn();
    }
    private void LoadPdfComplete(PDFDocument doc)
    {
        currentPage = 0;
        pdfDocument = doc;
        totalPage = doc.GetPageCount();
        PreloadPDFData();
        LoadHotpointData();
        DisplayPage(currentPage);
        canvasGroup.alpha = 0;
        gameObject.SetActive(true);
        canvasGroup.DOFade(1, 1f);
    }
    private void PreloadPDFData()
    {
        items = new List<ItemDataPDF>();
        for (int i = 0; i < totalPage; i++)
        {
            Texture2D tex = GetPageTexture(i);
            items.Add(new ItemDataPDF((i + 1).ToString(), tex));
        }
    }
    private void LoadHotpointData()
    {
        (bool hasVideo, string jsonPath) = ResourceManager.Instance.CheckPdfVideoJsonExist(currentData.mediaPath);
        if (hasVideo)
        {
            videoDatas = ResourceManager.Instance.GetPDFVideoDatasFromJson(jsonPath);
        }
    }
    private void CheckHoitPoint(int page)
    {
        var data = GetDataFromPage(page);
        if (data == null)
        {
            pageHasHotPoint = false;
            hotPointTitle.gameObject.SetActive(false);
            return;
        }
        pageHasHotPoint = true;
        hotPointTitle.gameObject.SetActive(true);
        hotPointTitle.SetContent(data.Title);
        hotPointTitle.GetComponent<CanvasGroup>().alpha = 0;
        hotPointTitle.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
        if (!string.IsNullOrEmpty(data.Cover))
        {
            ResourceManager.Instance.GetTexture(data.Cover, (t) =>
            {
                hotPointTitle.rawImage.texture = t;
            });
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
    private void DisplayPage(int index)
    {
        pdfPageRawImage.texture = items[index].texture;
        var fitter = pdfPageRawImage.GetComponent<ImageFitter>();
        fitter.Fit();
        CheckHoitPoint(index + 1);
    }


    public void NextPage()
    {
        if (currentPage >= totalPage - 1) return;
        currentPage++;
        DisplayPage(currentPage);
    }
    public void PrevPage()
    {
        if (currentPage <= 0) return;
        currentPage--;
        DisplayPage(currentPage);
    }

    private Texture2D GetPageTexture(int index, float scale = 2f)
    {
        PDFRenderer renderer = new PDFRenderer();
        Texture2D tex = renderer.RenderPageToTexture(pdfDocument.GetPage(index), (int)(pdfDocument.GetPage(index).GetPageSize().x * scale), (int)(pdfDocument.GetPage(index).GetPageSize().y * scale));
        tex.filterMode = FilterMode.Bilinear;
        renderer.Dispose();
        return tex;
    }

}
