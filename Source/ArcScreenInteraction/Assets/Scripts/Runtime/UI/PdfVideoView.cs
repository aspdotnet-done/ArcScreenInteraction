using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class PdfVideoView : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] ImageFitter imageFitter;
    [SerializeField] RawImage videoImage;
    public CanvasGroup canvasGroup;

    void Start()
    {
        Invoke("DelayToRuning", 0.1f);

    }

    void DelayToRuning()
    {
        AppManager.Instance.EnterAction += ConfirmAction;
        AppManager.Instance.BackAction += BackAction;
        Debug.Log("初始化");
    }
    PDFVideoData currentData;
    public void InitVideo(PDFVideoData data)
    {
        currentData = null;
        if (data == null) return;
        currentData = data;
        Debug.Log("InitVideo");
        //gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        videoPlayer.url = data.VideoName;
        videoPlayer.Play();

        Invoke("WaitforPlay", 0.1f);
    }
    void WaitforPlay()
    {
        //yield return new WaitForSeconds(0.1f);
        //float width = videoPlayer.width;
        //float height = videoPlayer.height;
        //RenderTexture r = new RenderTexture((int)width, (int)height, 0);
        //videoPlayer.targetTexture.width = (int)width;
        //videoPlayer.targetTexture.height = (int)height;
        //videoPlayer.targetTexture = r;
        //videoImage.texture = r;
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
        if (!videoPlayer.isPlaying) return;
        Debug.Log("BackAction");
        canvasGroup.alpha = 0;
        videoPlayer.Stop();
        //gameObject.SetActive(false);
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.SetCanReturn();
        ui.pDFViewer.SelectPage(currentData.PageIndex - 1);


    }


}
