using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayState
{
    Playing,
    Pause,
    Init
}
public abstract class BaseViewer : MonoBehaviour
{
    public bool isPlayComplete;
    [SerializeField] public CanvasGroup canvasGroup;
    protected MediaData currentData;
    protected int index;
    public PlayState currentState;
    public void LoadMedias(MediaData data, int index)
    {
        currentData = data;
        this.index = index;
        Show();
        Debug.Log("BaseViewer LoadMedias");
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
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
