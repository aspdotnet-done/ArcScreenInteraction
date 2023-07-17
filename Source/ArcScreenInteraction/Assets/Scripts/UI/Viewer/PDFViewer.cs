using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Paroxe.PdfRenderer;
using DG.Tweening;

public class PdfViewer : BaseViewer
{
    [SerializeField] private PDFViewer pdfReader;
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
    private int totalPage;
    public override void Show()
    {
        base.Show();
        pdfReader.FilePath = currentData.medias[index].mediaPath;
        pdfReader.LoadDocument(0);
        totalPage = pdfReader.Document.GetPageCount();
        Debug.Log("PdfViewer Show");
        canvasGroup.DOFade(1, 0.2f);
        //pdfReader.ZoomIn();
        pdfReader.GoToPage(0);
        currentState = PlayState.Playing;
    }
    public override void Hide()
    {
        base.Hide();
        canvasGroup.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
    }
}
