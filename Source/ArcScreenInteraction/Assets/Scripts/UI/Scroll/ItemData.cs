public class ItemData
{
    public string Message { get; }
    public MediaData MediaData { get; }
    public ItemData(string message)
    {
        Message = message;
    }
    public ItemData(MediaData mediaData)
    {
        MediaData = mediaData;
    }

}