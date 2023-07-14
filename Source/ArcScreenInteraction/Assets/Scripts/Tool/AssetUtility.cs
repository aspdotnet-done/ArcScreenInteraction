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
    public static string GetAnfangFolder()
    {
        return string.Format(AssetResourcesDir + "Medias/anfang/");
    }
    public static string GetRenfangFolder()
    {
        return string.Format(AssetResourcesDir + "Medias/renfang/");
    }
    public static string GetXiaofangFolder()
    {
        return string.Format(AssetResourcesDir + "Medias/xiaofang/");
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