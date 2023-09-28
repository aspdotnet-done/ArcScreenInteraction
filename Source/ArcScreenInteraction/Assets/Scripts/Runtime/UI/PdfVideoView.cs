using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using ScriptableObjectArchitecture;
using System;
using UnityEngine.Windows;

public class PdfVideoView : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] ImageFitter imageFitter;
    [SerializeField] GameEvent backEvent;
    [SerializeField] GameEvent enterEvent;
    public event Action OnBack;
    public CanvasGroup canvasGroup;
    [SerializeField] private Image playImage;
    private CanvasGroup _playImageCanvasGroup;
    private CanvasGroup playImageCanvasGroup
    {
        get
        {
            if (_playImageCanvasGroup == null) _playImageCanvasGroup = playImage.GetComponent<CanvasGroup>();
            return _playImageCanvasGroup;
        }
    }
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite pauseIcon;
    private void OnEnable()
    {
        backEvent.AddListener(BackAction);
        enterEvent.AddListener(ConfirmAction);
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }
    private void OnDisable()
    {
        backEvent.RemoveListener(BackAction);
        enterEvent.RemoveListener(ConfirmAction);
        videoPlayer.loopPointReached -= VideoPlayer_loopPointReached;
    }

    private async void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        Debug.Log("VideoPlayer_loopPointReached");
        await UniTask.Delay(1000);
        BackAction();
    }


    PDFVideoData currentData;
    public async void InitVideo(PDFVideoData data)
    {
        if (data == null || !File.Exists(data.VideoName))
        {
            BackAction();
        }
        currentData = data;
        Debug.Log("InitVideo");
        //gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        videoPlayer.url = data.VideoName;
        videoPlayer.Play();

        await UniTask.WaitUntil(() => videoPlayer.isPlaying);
        imageFitter.Fit();
    }

    void ConfirmAction()
    {
        if (videoPlayer.isPaused)
        {
            videoPlayer.Play();
            SetIconAndHideAsync(pauseIcon);
        }
        else
        {
            videoPlayer.Pause();
            SetIconAndHideAsync(playIcon);
        }
    }
    private async void SetIconAndHideAsync(Sprite icon)
    {
        playImage.sprite = icon;
        playImageCanvasGroup.alpha = 1;
        await UniTask.Delay(1500);
        playImageCanvasGroup.DOFade(0, 0.5f);
    }

    void BackAction()
    {
        if (canvasGroup.alpha == 0) return;
        canvasGroup.alpha = 0;
        playImageCanvasGroup.alpha = 0;
        videoPlayer.Stop();
        OnBack?.Invoke();
    }


}
