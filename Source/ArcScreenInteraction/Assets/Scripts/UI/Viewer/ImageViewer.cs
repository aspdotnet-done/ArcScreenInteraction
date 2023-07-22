using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ImageViewer : BaseViewer
{

    [SerializeField] private RawImage image;

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
        ResourceManager.Instance.GetTexture(currentData.mediaPath, (v) =>
        {
            image.texture = v;
            canvasGroup.DOFade(1, 0.2f);
            currentPlayState = PlayState.Playing;
            if (playImageCoroutine != null)
                StopCoroutine(playImageCoroutine);
            playImageCoroutine = StartCoroutine(PlayImage());
        });
        Debug.Log("ImageViewer Show");
        gameObject.SetActive(true);
    }


    public override void Hide()
    {
        canvasGroup.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
        gameObject.SetActive(false);
    }



    public float imagePlayCompleteTime
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.innerDelay;
        }
    }
    Coroutine playImageCoroutine;
    IEnumerator PlayImage()
    {
        yield return new WaitForSeconds(imagePlayCompleteTime);
        currentPlayState = PlayState.Complete;
    }


}
