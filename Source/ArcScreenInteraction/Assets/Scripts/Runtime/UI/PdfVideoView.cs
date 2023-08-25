using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PdfVideoView : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] ImageFitter imageFitter;
    [SerializeField] RawImage videoImage;

    void Start()
    {
        AppManager.Instance.EnterAction += ConfirmAction;
        AppManager.Instance.BackAction += BackAction;
    }

    public void InitVideo(PDFVideoData data)
    {
        videoPlayer.url = data.VideoName;
        videoPlayer.Play();

        StartCoroutine(WaitforPlay());
    }
    IEnumerator WaitforPlay()
    {
        yield return new WaitForSeconds(0.1f);
        float width = videoPlayer.width;
        float height = videoPlayer.height;
        RenderTexture r = new RenderTexture((int)width, (int)height, 0);
        videoPlayer.targetTexture = r;
        videoImage.texture = r;
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
        videoPlayer.Stop();
        gameObject.SetActive(false);
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.SetCanReturn();
    }


}
