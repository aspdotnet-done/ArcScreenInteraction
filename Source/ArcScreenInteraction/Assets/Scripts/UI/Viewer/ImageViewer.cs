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
        if (playImageCoroutine != null)
            playImageCoroutine = StartCoroutine(PlayImage());

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
        base.Show();
        ResourceManager.Instance.GetTexture(currentData.medias[index].mediaPath, (v) =>
        {
            image.texture = v;
            canvasGroup.DOFade(1, 0.2f);
            currentState = PlayState.Playing;
        });
        Debug.Log("ImageViewer Show");
    }
    public override void Hide()
    {
        base.Hide();
        image.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
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
        isPlayComplete = true;
    }


}
