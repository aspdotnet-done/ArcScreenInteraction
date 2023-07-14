using System;
public class ItemData
{
    public Action<Media> ClickAction = null;
    public string Message { get; }
    public Media MediaData { get; }
    public ItemData(string message)
    {
        Message = message;
    }
    public ItemData(Media mediaData, Action<Media> action)
    {
        MediaData = mediaData;
        ClickAction = action;
    }


}