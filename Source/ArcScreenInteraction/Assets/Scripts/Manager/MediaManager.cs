using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaManager : Singleton<MediaManager>
{
    public MediaDatasScriptableAsset mediaDatasScriptableAsset;
    // Start is called before the first frame update

    private MediaData current;
    IEnumerator Start()
    {
        yield return new WaitUntil(() => ResourceManager.Instance != null);
        ResourceManager.Instance.GetMediaDatas((data) =>
        {
            mediaDatasScriptableAsset.data = data;
        });
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
