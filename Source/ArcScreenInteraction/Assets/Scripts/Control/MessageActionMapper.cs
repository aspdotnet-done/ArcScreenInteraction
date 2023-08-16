using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MessageActionMap
{
    public string message;
    public NetworkInputAction action;
}

[CreateAssetMenu(fileName = "MessageActionMapper", menuName = "ArcScreenInteraction/MessageActionMapper", order = 0)]
public class MessageActionMapper : ScriptableObject
{
    public List<MessageActionMap> messageActionMaps = new List<MessageActionMap>();
    public NetworkInputAction GetAction(string message)
    {
        foreach (MessageActionMap map in messageActionMaps)
        {
            if (map.message == message)
            {
                return map.action;
            }
        }
        return NetworkInputAction.None;
    }
}