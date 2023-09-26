using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
public enum PlayState
{
    Playing,
    Pause,
    Init,
    Complete
}
public abstract class BaseViewer : MonoBehaviour
{
    [SerializeField] public CanvasGroup canvasGroup;
    public Media currentData;
    public PlayState currentPlayState;
    [SerializeField] protected GameEvent nextPageEvent;
    [SerializeField] protected GameEvent prevPageEvent;
    [SerializeField] protected GameEvent submitEvent;
    [SerializeField] protected GameEvent backEvent;
    protected virtual void OnEnable()
    {
        nextPageEvent.AddListener(Next);
        prevPageEvent.AddListener(Previous);
        submitEvent.AddListener(Play);
        backEvent.AddListener(Return);
    }
    protected virtual void OnDisable()
    {
        nextPageEvent.RemoveListener(Next);
        prevPageEvent.RemoveListener(Previous);
        submitEvent.RemoveListener(Play);
        backEvent.RemoveListener(Return);
    }
    public void LoadMedia(Media data)
    {
        currentData = data;
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
    }

}
