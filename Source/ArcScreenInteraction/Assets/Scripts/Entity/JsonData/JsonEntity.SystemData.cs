using System.Collections.Generic;

[System.Serializable]
public class SystemData
{
    public string appVersion;
    public SetupData setupData;
}


[System.Serializable]
public class SetupData
{
    //1-单次播放，2-单次循环 3-顺序播放多个 4-全部循环
    public LoopType loopType;
    public List<MediaData> medias;
    public float innerDelay;
    public float outerDelay;
}

public enum LoopType
{
    SinglePlay = 0,
    SingleLoop = 1,
    OrderPlay = 2,
    AllLoop = 3
}
