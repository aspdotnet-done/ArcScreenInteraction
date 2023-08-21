using System;
using UnityEngine;

public class ItemData
{
    public Action<string> ClickAction = null;
    public string Message { get; }
    public ItemData(string message)
    {
        Message = message;
    }



    public ItemData(string title, Action<string> action)
    {
        Message = title;
        ClickAction = action;
    }


}