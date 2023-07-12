using System;
public class ItemData
{
    public Action<MediaData> ClickAction;
    public Action<MediaData, string> ClickDetailAction;
    public string Message { get; }
    public MediaData MediaData { get; }
    public string media;
    public ItemData(string message)
    {
        Message = message;
    }
    public ItemData(MediaData mediaData, Action<MediaData> action)
    {
        MediaData = mediaData;
        ClickAction = action;
    }
    public ItemData(MediaData mediaData, string media, Action<MediaData, string> action)
    {
        MediaData = mediaData;
        this.media = media;
        ClickDetailAction = action;

    }

}