using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using DG.Tweening;

public class VideoViewer : BaseViewer
{
    [SerializeField] private MediaPlayer videoPlayer;
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
        MediaPath mp = new MediaPath(currentData.mediaPath, MediaPathType.AbsolutePathOrURL);
        videoPlayer.OpenMedia(mp, true);
        Debug.Log("PdfViewer Show");
        canvasGroup.DOFade(1, 0.2f);
        currentPlayState = PlayState.Playing;
    }
    public override void Hide()
    {
        base.Hide();
        canvasGroup.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
    }
}
