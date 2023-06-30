using System.Collections.Generic;

namespace EntityProgram
{
    [System.Serializable]
    public class SystemData
    {
        public string appVersion;
        public SetupData setupData;
    }


    [System.Serializable]
    public class SetupData
    {
        //1-播放单次，2-顺序播放 3-单个循环 4-多个循环
        public int loopType;
        public float innerDelay;
        public float outerDelay;
    }

}