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


    public string cover;
    public string coverPath
    {
        get { return AssetUtility.GetCover(cover); }
    }
    public Media[] medias;

    public string MediaPathFolder
    {
        get
        {

            return AssetUtility.GetMediaFolderPath(folder, title);
        }
    }

}
[SerializeField]
public class Media
{
    public MediaType mediaType;
    public string mediaName;
    public string cover;
    public string mediaClass;

}
public enum MediaType//1-pdf,2-video,3-pitcure
{
    pdf = 1,
    video = 2,
    picture = 3
}
