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
        base.Show();
        ResourceManager.Instance.GetTexture(currentData.MediaPathList[index], (v) =>
                {
                    image.texture = v;
                    image.DOFade(1, 0.2f);
                });
        Debug.Log("ImageViewer Show");
    }
    public override void Hide()
    {
        base.Hide();
        image.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
    }
}
