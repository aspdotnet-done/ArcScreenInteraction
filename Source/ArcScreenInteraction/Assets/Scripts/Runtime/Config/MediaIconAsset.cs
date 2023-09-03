using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "MediaIconAsset", menuName = "ArcScreenInteraction/MediaIconAsset", order = 0)]
public class MediaIconAsset : ScriptableObject
{
    public List<MediaIcon> mediaIcons = new List<MediaIcon>();
    public Sprite GetIcon(MediaType mediaType)
    {
        foreach (var i in mediaIcons)
        {
            if (i.mediaType == mediaType)
            {
                return i.icon;
            }
        }
        return null;
    }
    private void OnValidate()
    {
        foreach (var i in mediaIcons)
        {
            i.Name = i.mediaType.ToString();
        }
    }
}
[Serializable]
public class MediaIcon
{
    public string Name;
    public MediaType mediaType;
    public Sprite icon;
}