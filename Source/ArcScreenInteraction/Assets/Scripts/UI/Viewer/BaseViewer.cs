using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseViewer : MonoBehaviour
{
    protected MediaData currentData;
    protected Media media;
    public void LoadMedias(MediaData data, Media media)
    {
        currentData = data;
        this.media = media;
        Show();
        Debug.Log("BaseViewer LoadMedias");
    }

    public virtual void Show()
    {
        Debug.Log("BaseViewer Show");
    }
    public virtual void Next()
    {
        Debug.Log("BaseViewer Next");
    }
    public virtual void Previous()
    {
        Debug.Log("BaseViewer Previous");
    }
    public virtual void Play()
    {
        Debug.Log("BaseViewer Play");
    }
    public virtual void Pause()
    {
        Debug.Log("BaseViewer Pause");
    }
    public virtual void Return()
    {
        Debug.Log("BaseViewer Return");
    }

    public virtual void Hide()
    {
        Debug.Log("BaseViewer Hide");
        gameObject.SetActive(false);
    }

}
