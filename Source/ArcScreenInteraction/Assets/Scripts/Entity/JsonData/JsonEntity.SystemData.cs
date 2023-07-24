﻿using System.Collections.Generic;

[System.Serializable]
public class SystemData
{
    public string appVersion;
    public SetupData setupData;
}


[System.Serializable]
public class SetupData
{
    //1-单次播放，2-单次循环  3-全部循环
    public LoopType loopType;
    //public List<MediaData> medias;
    public float innerDelay;
    public float outerDelay;
    public float mainDelay;
}

public enum LoopType
{
    Single = 0,
    LoopOnce = 1,
    Loop = 2
}
