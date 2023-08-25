using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class AssetUtility
{

    public static string GetCover(string cover)
    {
        return string.Format(AssetResourcesDir + "Medias/{0}", cover);
    }
    public static string GetMap(string folder, string mapName)
    {
        return string.Format(AssetResourcesDir + "/{0}/{1}", folder, mapName);
    }

    public static string GetMediaPath(string parentType, string title, string mediaName)
    {
        return string.Format(AssetResourcesDir + "Medias/{0}/{1}/{2}", parentType, title, mediaName);
    }
    public static string GetMediaFolderPath(string parentType, string title)
    {
        return string.Format(AssetResourcesDir + "Medias/{0}/{1}/", parentType, title);
    }
    // public static string GetAnfangFolder()
    // {
    //     return string.Format(AssetResourcesDir + "Medias/安防数据/");
    // }
    // public static string GetRenfangFolder()
    // {
    //     return string.Format(AssetResourcesDir + "Medias/人防数据/");
    // }
    // public static string GetXiaofangFolder()
    // {
    //     return string.Format(AssetResourcesDir + "Medias/消防数据/");
    // }
    /// <summary>
    /// 获取 详情数据文件夹
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public static string GetDetailDataFolder(string itemName)
    {
        return string.Format(AssetResourcesDir + $"Medias/列表/{itemName}/");
    }

    public static string GetMainBgFolder()
    {
        return string.Format(AssetResourcesDir + "Medias/主页数据/");
    }
    public static string GetMainListFolder()
    {
        return string.Format(AssetResourcesDir + "Medias/列表/");
    }
    public static string GetMainContentJson()
    {
        if (File.Exists(AssetResourcesDir + "Medias/主页数据/描述.json"))
        {
            // FileInfo file = new FileInfo(AssetResourcesDir + "Medias/主页数据/描述.json");
            // return string.Format(AssetResourcesDir + "Medias/主页数据/描述.json");
            //读取路径的json文件并返回字符串格式
            StreamReader sr = new StreamReader(AssetResourcesDir + "Medias/主页数据/描述.json");
            string str = sr.ReadToEnd();
            sr.Close();
            //Debug.Log(str);
            return str;
        }
        else
            return "";
    }
    public static string GetDetailDataContentJson(string itemName)
    {
        if (File.Exists(AssetResourcesDir + $"Medias/列表/{itemName}/描述.json"))
        {
            //读取路径的json文件并返回字符串格式
            StreamReader sr = new StreamReader(AssetResourcesDir + $"Medias/列表/{itemName}/描述.json");
            string str = sr.ReadToEnd();
            sr.Close();
            //Debug.Log(str);
            return str;
        }
        else
            return "";
    }

    //读取安防文件夹的json文件并返回字符串格式
    public static string GetAnfangContentJson()
    {
        if (File.Exists(AssetResourcesDir + "Medias/安防数据/描述.json"))
        {
            StreamReader sr = new StreamReader(AssetResourcesDir + "Medias/安防数据/描述.json");
            string str = sr.ReadToEnd();
            sr.Close();
            return str;
        }
        else
            return "";
    }



    //读取人防文件夹的json文件并返回字符串格式
    public static string GetRenfangContentJson()
    {
        if (File.Exists(AssetResourcesDir + "Medias/人防数据/描述.json"))
        {
            StreamReader sr = new StreamReader(AssetResourcesDir + "Medias/人防数据/描述.json");
            string str = sr.ReadToEnd();
            sr.Close();
            return str;
        }
        else
            return "";
    }
    //读取消防文件夹的json文件并返回字符串格式
    public static string GetXiaofangContentJson()
    {
        if (File.Exists(AssetResourcesDir + "Medias/消防数据/描述.json"))
        {
            StreamReader sr = new StreamReader(AssetResourcesDir + "Medias/消防数据/描述.json");
            string str = sr.ReadToEnd();
            sr.Close();
            return str;
        }
        else
            return "";
    }

    public static string GetSystemConfig()
    {
        return AssetResourcesDir + "SystemInfo.json";
    }
    public static string GetMediaDatasConfig()
    {
        return AssetResourcesDir + "MediaDatas.json";
    }
    public static string GetMediaClassesConfig()
    {
        return AssetResourcesDir + "MediaClass.json";
    }
    public static string GetTempFolder()
    {
        if (!Directory.Exists(AssetResourcesDir + TempFolderName + "/"))
            Directory.CreateDirectory(AssetResourcesDir + TempFolderName + "/");
        return AssetResourcesDir + TempFolderName + "/";
    }

    public static string GetMapFolder()
    {
        return AssetResourcesDir + mapFolderName + "/";
    }

    public static string TempFolderName = "Temp";
    public static string Medias = "Medias";
    public static string mapFolderName = "Maps";

    private static string modelFolderName = "Scene";
    private static string imageFolderName = "Texture";
    private static string soundFolderName = "Audio";
    /// <summary>
    /// 地图文件的前缀
    /// </summary>
    public static string MAPNAME = "map-";

    /// <summary>
    /// 应用的文件存储路径
    /// </summary>
    /// <value></value>
    public static string AssetResourcesDir
    {
        get
        {
            // #if UNITY_EDITOR
            //                 string dir = Application.streamingAssetsPath;
            // #else
            //                 string dir = Application.streamingAssetsPath;
            // #endif
            string dir = "";
#if UNITY_EDITOR
            dir = Application.dataPath.Replace("/Assets", "");

#elif UNITY_STANDALONE
                            dir = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')+1);
#elif UNITY_IOS
                            dir = Application.temporaryCachePath;//·Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Library/Caches/
#elif UNITY_ANDROID
                            dir = Application.persistentDataPath;//·data/data/xxx.xxx.xxx/files/
#else
                            dir = Application.streamingAssetsPath;//·/xxx_Data/StreamingAssets/
#endif
            if (!Directory.Exists(dir + "/Res/"))
            {
                Directory.CreateDirectory(dir + "/Res/");
            }
            return dir + "/Res/";
        }
    }
}