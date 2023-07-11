using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseViewer : MonoBehaviour
{
    private MediaData currentData;
    public virtual void LoadMedias(MediaData data)
    {
        Debug.Log("BaseViewer LoadMedias");
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

}
