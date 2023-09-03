using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MediaData
{
    public int id;
    public string title;
    public string folder;
    public string condition;
    public string bgPath;
    public List<Media> medias = new List<Media>();
    public List<string> classes = new List<string>();

    public string MediaPathFolder
    {
        get
        {

            return AssetUtility.GetMediaFolderPath(folder, title);
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
