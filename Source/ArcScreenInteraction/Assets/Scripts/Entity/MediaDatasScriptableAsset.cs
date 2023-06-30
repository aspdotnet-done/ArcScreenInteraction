using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MediaDatasScriptableAsset", menuName = "ScriptableAssets/MediaDatasScriptableAsset")]
public class MediaDatasScriptableAsset : ScriptableObject
{
    //所有数据
    public AllData data;
}
[System.Serializable]
public class AllData
{
    //安防数据
    public List<MediaData> anfangDatas;
    //消防数据
    public List<MediaData> xiaofangDatas;
    //人防数据
    public List<MediaData> renfangDatas;
}

