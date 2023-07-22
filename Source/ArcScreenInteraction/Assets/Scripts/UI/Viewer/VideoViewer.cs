using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using DG.Tweening;
using RenderHeads.Media.AVProVideo.Demos;
using UnityEngine.UI;

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
        gameObject.SetActive(true);
        MediaPath mp = new MediaPath(currentData.mediaPath, MediaPathType.AbsolutePathOrURL);
        videoPlayer.OpenMedia(mp, false);
        videoPlayer.Play();
        canvasGroup.DOFade(1, 0.2f);
        currentPlayState = PlayState.Playing;
        videoPlayer.Events.AddListener(OnVideoEvent);

    }
    private void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
    {
        if (MediaPlayerEvent.EventType.FinishedPlaying == et)
        {
            currentPlayState = PlayState.Complete;
            Debug.Log("ViedoComplete");
        }

    }

    private void LateUpdate()

    {
        if (videoPlayer != null)
        {
            //videoPlayer的视频是否暂停且非播放完毕


            if (videoPlayer.Control.IsPaused() && !videoPlayer.Control.IsFinished())
            {
                Debug.Log("Pause");
                currentPlayState = PlayState.Pause;
            }
            else if (videoPlayer.Control.IsPlaying())
            {
                currentPlayState = PlayState.Playing;
            }
        }
    }

    public override void Hide()
    {
        canvasGroup.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
        gameObject.SetActive(false);
    }
}
