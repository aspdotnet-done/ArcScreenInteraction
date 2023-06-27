using System.Collections.Generic;

namespace EntityProgram
{
    [System.Serializable]
    public class SystemData
    {
        public string appVersion;//地图所属人的判断 如果为空 设置成当前用户，如果不为空 当前token与用户不匹配时则清空创意空间数据
        public List<JMapInfo> maps = new List<JMapInfo>();
    }

    [System.Serializable]
    public class JMapInfo
    {
        public MapState mapState;
        public string mapName;
        public string createDatatime;
        public string updateTime;
        public string description;
        public string pic;
        public string privewUrl;//在线的缩略图
        public string picUrl;
        public string mapUrl;
        public string uuid;//资源对应的索引

        public int id;

        public int planeSize = 5;

        public int version = 0;
    }
    /// <summary>
    /// 地图的同步状态
    /// </summary>
    public enum MapState
    {
        none,//因为旧版没有同步功能，当前状态为none 如果比对后台不存在则默认为add
        delete,
        add,
        modify,
        normal,//同步完成的状态
        discard//废弃状态
    }

}