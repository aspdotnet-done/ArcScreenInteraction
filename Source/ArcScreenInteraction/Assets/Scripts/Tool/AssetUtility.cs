using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class AssetUtility
{

    public static string GetTexture(string textureName)
    {
        return string.Format(AssetResourcesDir + "maps/{0}", textureName);
    }
    public static string GetMap(string folder, string mapName)
    {
        return string.Format(AssetResourcesDir + "/{0}/{1}", folder, mapName);
    }

    public static string GetSerialMovie(string serialName)
    {
        return string.Format(AssetResourcesDir + "Serials/{0}.mp4", serialName);
    }

    public static string GetSystemConfig()
    {
        return AssetResourcesDir + "SystemInfo.config";
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
    public static string SerialsFolderName = "Serials";
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