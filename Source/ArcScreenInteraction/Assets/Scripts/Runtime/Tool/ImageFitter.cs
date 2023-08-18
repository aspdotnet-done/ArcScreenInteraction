using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ImageUIType
{
    None,
    Image,
    RawImage
}
[RequireComponent(typeof(RectTransform))]
public class ImageFitter : MonoBehaviour
{
    public RectTransform targetRect;
    private Image _image;
    private Image image
    {
        get
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }
            return _image;
        }
    }
    private RawImage _rawImage;
    private RawImage rawImage
    {
        get
        {
            if (_rawImage == null)
            {
                _rawImage = GetComponent<RawImage>();
            }
            return _rawImage;
        }
    }
    private ImageUIType _imageUIType = ImageUIType.None;
    private RectTransform _rectTransform;
    private RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
    [ContextMenu("Fit")]
    public void Fit()
    {
        if (targetRect == null)
        {
            Debug.LogWarning("ImageFitter: No targetRect set.");
            return;
        }
        if (_imageUIType == ImageUIType.None)
        {
            Debug.LogWarning("ImageFitter: No Image or RawImage component found on this GameObject.");
            return;
        }
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        float targetWidth = targetRect.rect.width;
        float height = targetRect.rect.height;
        float targetRatio = targetWidth / height;
        // Debug.Log($"target width:{targetWidth} height:{height} targetRatio: " + targetRatio);

        (int textureWidth, int textureHeight) = GetTextureSize();
        float textureRatio = (float)textureWidth / textureHeight;
        // Debug.Log($"texture width:{textureWidth} height:{textureHeight} textureRatio: " + textureRatio);
        if (textureRatio < targetRatio)
        {
            float scaleFactor = targetWidth / textureWidth;
            rectTransform.sizeDelta = new Vector2(targetWidth, textureHeight * scaleFactor);
        }
        else
        {
            float scaleFactor = height / textureHeight;
            rectTransform.sizeDelta = new Vector2(textureWidth * scaleFactor, height);
        }
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }
    private (int, int) GetTextureSize()
    {
        int textureWidth = 0;
        int textureHeight = 0;
        if (_imageUIType == ImageUIType.Image)
        {
            textureWidth = image.sprite.texture.width;
            textureHeight = image.sprite.texture.height;
        }
        else if (_imageUIType == ImageUIType.RawImage)
        {
            textureWidth = rawImage.texture.width;
            textureHeight = rawImage.texture.height;
        }
        return (textureWidth, textureHeight);
    }
    private void OnValidate()
    {
        GetImageType();
    }
    private void GetImageType()
    {
        if (image != null)
        {
            _imageUIType = ImageUIType.Image;
        }
        else if (rawImage != null)
        {
            _imageUIType = ImageUIType.RawImage;
        }
        else
        {
            _imageUIType = ImageUIType.None;
            Debug.LogWarning("ImageFitter: No Image or RawImage component found on this GameObject.");
        }
    }
}
