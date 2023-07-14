using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MediaManager : Singleton<MediaManager>
{
    public MediaDatasScriptableAsset mediaDatasScriptableAsset;
    public SetupDataScriptableAsset setupDataScriptableAsset;
    // Start is called before the first frame update

    private List<MediaData> currentMediaDataList;

    /// <summary>
    /// 以当前打开的mediaData为首，重新排序
    /// </summary>
    /// <param name="complete"></param>
    public void SortMediaList(MediaData current, Action<List<MediaData>> complete = null)
    {
        List<MediaData> newList = new List<MediaData>();
        int index = currentMediaDataList.IndexOf(current);
        //获取currentMediaDataList以index为首的数据
        for (int i = index; i < currentMediaDataList.Count; i++)
        {
            newList.Add(currentMediaDataList[i]);
        }
        //获取currentMediaDataList以index为尾的数据
        for (int i = 0; i < index; i++)
        {
            newList.Add(currentMediaDataList[i]);
        }
        currentMediaDataList = newList;
        complete?.Invoke(currentMediaDataList);

    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => ResourceManager.Instance != null);
        ResourceManager.Instance.GetSystemInfo((data) =>
        {
            setupDataScriptableAsset.data = data;
        });
        ResourceManager.Instance.GetMediaDatas((data) =>
        {
            mediaDatasScriptableAsset.mediaDatas = data;
        });
    }

    public void UpdateSetupAsset()
    {
        ResourceManager.Instance.ChangeSystemInfo(setupDataScriptableAsset.data);
    }

    public void PlayMedia(MediaData data)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum SecurityType
{
    xiaofang,
    anfang,
    renfang
}
