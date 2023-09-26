using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using ScriptableObjectArchitecture;

public class PdfVideoView : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] ImageFitter imageFitter;
    [SerializeField] GameEvent backEvent;
    [SerializeField] GameEvent enterEvent;

    public CanvasGroup canvasGroup;
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
        currentData = null;
        if (data == null) return;
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
        }
        else
        {
            videoPlayer.Pause();
        }
    }
    void BackAction()
    {
        if (canvasGroup.alpha == 0) return;
        canvasGroup.alpha = 0;
        videoPlayer.Stop();
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.SetCanReturn();
    }


}
