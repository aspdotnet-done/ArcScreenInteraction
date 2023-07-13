using System;
public class ItemData
{
    public Action<MediaData> ClickAction = null;
    public Action<MediaData, Media> ClickDetailAction = null;
    public string Message { get; }
    public MediaData MediaData { get; }
    public Media media;
    public ItemData(string message)
    {
        Message = message;
    }
    public ItemData(MediaData mediaData, Action<MediaData> action)
    {
        MediaData = mediaData;
        ClickAction = action;
    }
    public ItemData(MediaData mediaData, Media media, Action<MediaData, Media> action)
    {
        MediaData = mediaData;
        this.media = media;
        ClickDetailAction = action;

    }

}