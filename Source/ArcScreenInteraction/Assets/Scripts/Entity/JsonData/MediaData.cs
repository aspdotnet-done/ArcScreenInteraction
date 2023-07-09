using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MediaData
{
    public int id;
    public string title;
    public string condition;
    public string parentType;//1-安防，2-消防，3-人防
    //1-pdf,2-video,3-pitcure
    public int mediaType;
    public MediaType MediaType
    {
        get
        {
            return (MediaType)mediaType;
        }
    }
    public string cover;
    public string coverPath
    {
        get { return AssetUtility.GetCover(cover); }
    }
    public string[] medias;
    private List<string> mediaPathList;
    public List<string> MediaPathList
    {
        get
        {
            if (mediaPathList == null)
            {
                mediaPathList = new List<string>();
                foreach (var i in medias)
                {
                    mediaPathList.Add(AssetUtility.GetMediaPath(parentType, i));
                }
            }
            return mediaPathList;
        }
    }

}
public enum MediaType
{
    pdf = 1,
    video = 2,
    picture = 3
}