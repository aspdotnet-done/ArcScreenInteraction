using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class MediaData
{
    public int ID;
    public string Title;
    public string Folder;
    public string Condition;
    public string BGPath;
    public List<Media> Medias = new List<Media>();
    public List<ClassData> ClassesData = new List<ClassData>();

    public string MediaPathFolder
    {
        get
        {

            return AssetUtility.GetMediaFolderPath(Folder, Title);
        }
    }

}
[System.Serializable]
public class Media
{
    public MediaType mediaType;
    public string mediaName;
    public string mediaPath;
    public string coverPath;
    public string mediaClass;

}
public enum MediaType//1-pdf,2-video,3-pitcure
{
    PDF = 1,
    VIDEO = 2,
    PICTURE = 3,
    MONITOR = 4

}
[Serializable]
public class ClassData
{
    public string Title;
    public Sprite Icon = null;
}