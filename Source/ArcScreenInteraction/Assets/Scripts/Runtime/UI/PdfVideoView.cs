using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class PdfVideoView : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] ImageFitter imageFitter;
    [SerializeField] RawImage videoImage;
    public CanvasGroup canvasGroup;

    void Start()
    {
        Invoke("DelayToRuning", 0.1f);
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private async void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        Debug.Log("VideoPlayer_loopPointReached");
        await UniTask.Delay(1000);
        BackAction();
    }

    void DelayToRuning()
    {
        AppManager.Instance.EnterAction += ConfirmAction;
        AppManager.Instance.BackAction += BackAction;
        Debug.Log("初始化");
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
        Debug.Log("BackAction");
        canvasGroup.alpha = 0;
        videoPlayer.Stop();
        //gameObject.SetActive(false);
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.SetCanReturn();
        ui.pDFViewer.SelectPage(currentData.PageIndex - 1);
    }


}
