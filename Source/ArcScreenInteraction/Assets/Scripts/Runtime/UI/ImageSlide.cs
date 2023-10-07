using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using Cysharp.Threading.Tasks;
[Serializable]
public class ImageSlideConfig
{
    [Range(0, 5)]
    [Tooltip("图片切换的动画时间")]
    public float Duration = 1;
    [Range(0, 20)]
    [Tooltip("图片切换的间隔时间")]
    public float Interval = 5;
}
public class ImageSlide : MonoBehaviour
{
    public enum SlideState
    {
        None,
        Playing,
        Paused,
        Stopped,
    }
    public ImageSlideConfig Config;

    [SerializeField] private List<RawImage> images = new List<RawImage>();

    public List<Texture2D> Textures = new List<Texture2D>();

    public SlideState State { get; private set; } = SlideState.None;

    public void SetTextures(List<Texture2D> textures)
    {
        Textures = textures;
    }

    #region  图片播放控制
    private int index = -1;
    public void StartSlide()
    {
        State = SlideState.Playing;
        SlideAsync();
    }
    public void PauseSlide()
    {
        State = SlideState.Paused;
    }
    public void StopSlide()
    {
        State = SlideState.Stopped;
        index = -1;
    }
    public void ResumeSlide()
    {
        State = SlideState.Playing;
        SlideAsync();
    }
    #endregion
    private async void SlideAsync()
    {
        while (State == SlideState.Playing)
        {
            await SlideToNextAsync();
            await UniTask.Delay(TimeSpan.FromSeconds(Config.Interval));
        }
    }
    private async UniTask SlideToNextAsync()
    {
        if (Textures.Count <= 2)
        {
            return;
        }
        index = (index + 1) % Textures.Count;
        var texture = Textures[index];
        var backImage = GetBackImage();
        backImage.texture = texture;
        ResizeImage(backImage);
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        var frontImage = GetFrontImage();
        frontImage.DOFade(0, Config.Duration);
        backImage.DOFade(1, Config.Duration);
        await UniTask.Delay(TimeSpan.FromSeconds(Config.Duration));
        backImage.transform.SetAsLastSibling();
    }
    private void ResizeImage(RawImage image)
    {
        var imageFitter = image.GetComponent<ImageFitter>();
        if (imageFitter != null)
        {
            imageFitter.Fit();
        }
    }
    private RawImage GetFrontImage()
    {
        //从images中获取siblingIndex最大的image
        var maxSiblingIndex = -1;
        RawImage frontImage = null;
        foreach (var image in images)
        {
            if (image.transform.GetSiblingIndex() > maxSiblingIndex)
            {
                maxSiblingIndex = image.transform.GetSiblingIndex();
                frontImage = image;
            }
        }
        return frontImage;
    }
    private RawImage GetBackImage()
    {
        //从images中获取siblingIndex最小的image
        var minSiblingIndex = int.MaxValue;
        RawImage backImage = null;
        foreach (var image in images)
        {
            if (image.transform.GetSiblingIndex() < minSiblingIndex)
            {
                minSiblingIndex = image.transform.GetSiblingIndex();
                backImage = image;
            }
        }
        return backImage;
    }

}
