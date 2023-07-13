using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class ResourceManager : Singleton<ResourceManager>
{
    public Action InitCompleteAction;
    public bool UseTestData;
    private SystemData systemData;
    private Texture2D defaultTexture;
    public SystemData SystemData
    {
        get { return systemData; }
    }
    /// <summary>
    /// 本地创意空间文件的鉴权
    /// </summary>
    /// <value></value>
    public string MapToken
    {
        get
        {
            return SystemData.appVersion;
        }
    }

    public override void Awake()
    {
        base.Awake();
        //StartCoroutine(WaitForNextAction(1f, CheckSystemInfo));
    }

    IEnumerator WaitForNextAction(float time, Action complete)
    {
        yield return new WaitForSeconds(time);
        complete?.Invoke();
    }

    /// <summary>
    ///检测版本配置文件是否存在
    //如果不存在 删除所有缓存 重新创建版本配置文件
    /// </summary>
    public void GetSystemInfo(Action<SystemData> complete = null)
    {
        if (!File.Exists(AssetUtility.GetSystemConfig()))
        {
            //  Debug.Log("无配置文件，开始下一步逻辑");
            systemData = new SystemData() { };
            string jsonStr = JsonUtility.ToJson(systemData);
            CreateFolder(AssetUtility.mapFolderName);
            CreateSystemConfig(jsonStr);
            complete?.Invoke(systemData);
        }
        else
        {
            // Debug.Log("存在配置文件，开始下一步逻辑");
            string jsonStr = GetSystemConfigInfo();
            systemData = JsonUtility.FromJson<SystemData>(jsonStr);
            complete?.Invoke(systemData);
        }
    }

    public void ChangeSystemInfo(SystemData data)
    {
        systemData = data;
        string jsonStr = JsonUtility.ToJson(systemData);
        CreateSystemConfig(jsonStr);
    }






    /// <summary>
    /// 清空缓存
    /// </summary>
    public void ClearCaches()
    {
        DeleteDir(AssetUtility.AssetResourcesDir);
        //初始化文件
        DelayToRuning(0.5f, () =>
        {
            GetSystemInfo();
        });
    }



    public T Load<T>(string path) where T : UnityEngine.Object
    {
        try
        {
            if (typeof(T).Equals(typeof(UnityEngine.Object)))
            {
                return Resources.Load<T>(path);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
        return null;
    }

    public void LoadAsync(string path, Action<object> complete)
    {
        StartCoroutine(AsyncLoadResources(path, complete));
    }


    IEnumerator AsyncLoadResources(string path, Action<UnityEngine.Object> complete)
    {
        ResourceRequest resourceRequest = Resources.LoadAsync(path);
        while (!resourceRequest.isDone)
        {
            yield return 0;
        }
        complete?.Invoke(resourceRequest.asset);
    }



    /// <summary>
    /// 创建本地配置表
    /// </summary>
    /// <param name="jsonStr"></param>
    private void CreateSystemConfig(string jsonStr)
    {
        FileStream fileStream = null;
        if (File.Exists(AssetUtility.GetSystemConfig()))
        {
            fileStream = new FileStream(AssetUtility.GetSystemConfig(), FileMode.Truncate, FileAccess.ReadWrite);
        }
        else
        {
            fileStream = new FileStream(AssetUtility.GetSystemConfig(), FileMode.OpenOrCreate);
        }
        StreamWriter sw = new StreamWriter(fileStream, Encoding.UTF8);
        sw.Write(jsonStr);
        sw.Close();
        fileStream.Close();

    }
    /// <summary>
    /// 获取配置表内容
    /// </summary>
    /// <returns></returns>
    private string GetSystemConfigInfo()
    {
        string info = "";
        using (StreamReader sr = new StreamReader(AssetUtility.GetSystemConfig()))
        {
            info = sr.ReadToEnd();
            sr.Close();
        }
        return info;
    }



    public bool ClearFolderAndUpdate()
    {
        //Directory.Delete(AssetUtility.AssetResourcesDir);

        DirectoryInfo di = new DirectoryInfo(AssetUtility.AssetResourcesDir);
        di.Delete(true);
        CreateFolder(AssetUtility.mapFolderName);
        DelayToRuning(0.5f, () =>
            {

            });

        return true;
    }

    public void GetMediaDatas(Action<AllData> complete)
    {
        string mediaConfigPath = AssetUtility.GetMediaDatasConfig();
        if (!File.Exists(mediaConfigPath))
        {
            Debug.LogError("没有此路径文件：" + mediaConfigPath);
            complete?.Invoke(null);
        }
        else
        {
            string info = "";
            using (StreamReader sr = new StreamReader(mediaConfigPath))
            {
                info = sr.ReadToEnd();
                sr.Close();
            }
            AllData allData = JsonUtility.FromJson<AllData>(info);
            complete?.Invoke(allData);
        }
    }






    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        //在这里检测camera有没有关闭
    }


    // public static void UnZipFile(string zipPath, string outPath, Action<bool> unzipComplete = null)
    // {
    //     if (!File.Exists(zipPath))
    //     {
    //         Debug.LogError("没有此路径文件：" + zipPath);
    //         unzipComplete?.Invoke(false);
    //         return;
    //     }

    //     using (ZipInputStream stream = new ZipInputStream(File.OpenRead(zipPath)))
    //     {
    //         try
    //         {
    //             ZipEntry theEntry;
    //             while ((theEntry = stream.GetNextEntry()) != null)
    //             {
    //                 string fileName = Path.GetFileName(theEntry.Name);
    //                 string filePath = Path.Combine(outPath, theEntry.Name);
    //                 string directoryName = Path.GetDirectoryName(filePath);

    //                 // 创建压缩文件中文件的位置
    //                 if (directoryName.Length > 0)
    //                 {
    //                     Directory.CreateDirectory(directoryName);
    //                 }

    //                 if (fileName != String.Empty)
    //                 {
    //                     using (FileStream streamWriter = File.Create(filePath))
    //                     {
    //                         int size = 2048;
    //                         byte[] data = new byte[2048];
    //                         while (true)
    //                         {
    //                             size = stream.Read(data, 0, data.Length);
    //                             if (size > 0)
    //                             {
    //                                 streamWriter.Write(data, 0, size);
    //                             }
    //                             else
    //                             {
    //                                 break;
    //                             }
    //                         }
    //                     }
    //                 }
    //             }

    //             unzipComplete?.Invoke(true);
    //         }
    //         catch (Exception ex)
    //         {
    //             Debug.Log(ex.Message);
    //             unzipComplete?.Invoke(false);
    //         }
    //     }
    // }

    /// <summary>
    /// 删除指定文件夹
    /// </summary>
    /// <param name="path"></param>
    public bool DeleteDir(string path)
    {

        try
        {
            //去除文件夹和子文件的只读属性
            System.IO.DirectoryInfo fileInfo = new DirectoryInfo(path);
            fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
            //去除文件的只读属性
            System.IO.File.SetAttributes(path, System.IO.FileAttributes.Normal);

            //判断文件夹是否还存在
            if (Directory.Exists(path))
            {

                foreach (string f in Directory.GetFileSystemEntries(path))
                {

                    if (File.Exists(f))
                    {
                        //如果有子文件删除文件
                        File.Delete(f);
                    }
                    else
                    {
                        //循环递归删除子文件夹
                        DeleteDir(f);
                    }

                }

                //删除空文件夹

                Directory.Delete(path);

            }
            return true;
        }
        catch (Exception ex) // 异常处理
        {
            Debug.Log(ex.Message);
            return false;
        }

    }
    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="folderName"></param>
    public void CreateFolder(string folderName)
    {
        string folderPath = AssetUtility.AssetResourcesDir + "/" + folderName;
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        else
        {
            Debug.Log("已存在文件夹");
        }
    }
    /// <summary>
    /// 等待指定时长执行某事件
    /// </summary>
    /// <param name="time"></param>
    /// <param name="delayAction"></param>
    public void DelayToRuning(float time, Action delayAction)
    {
        StartCoroutine(WaitDelayToRuning(time, delayAction));
    }

    IEnumerator WaitDelayToRuning(float time, Action delayAction)
    {
        yield return new WaitForSeconds(time);
        delayAction?.Invoke();
    }

    public void DownloadResource(string url, string fileName, Action<float, DownloadType> downloadCompleteAction = null)
    {

        DownloadCorotine = StartCoroutine(Download(url, fileName, downloadCompleteAction));
    }

    public void StopDownload()
    {
        if (DownloadCorotine != null)
        {
            StopCoroutine(DownloadCorotine);
        }
    }

    Coroutine DownloadCorotine;
    /// <summary>
    /// 协程网络下载文件
    /// </summary>
    /// <param name="url"></param>
    /// <param name="exportPath"></param>
    /// <returns></returns>
    IEnumerator Download(string url, string fileName, Action<float, DownloadType> downloadAction = null)
    {
        //string fileName = url.Substring(url.LastIndexOf('/') + 1, url.Length - url.LastIndexOf('/') - 1);
        string tempPath = AssetUtility.GetMapFolder() + fileName;
        Debug.Log("localName:" + fileName);
        Debug.Log("tempPath:" + tempPath);
        //如果有 先删除文件
        if (File.Exists(tempPath))
        {
            File.Delete(tempPath);
        }
        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {

            AsyncOperation request = uwr.SendWebRequest();
            while (!request.isDone)
            {
                downloadAction?.Invoke(request.progress, DownloadType.Downloading);
                //Debug.Log($"request.progress:{request.progress.ToString("f2")}");
                yield return 0;
            }


            if (uwr.isHttpError == true || uwr.isNetworkError == true)
            {
                downloadAction?.Invoke(request.progress, DownloadType.Fail);
                Debug.Log("www.error : " + uwr.error);
            }
            else
            {
                var data = uwr.downloadHandler.data;
                FileStream fs = new FileStream(tempPath, FileMode.OpenOrCreate);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(data, 0, data.Length);
                bw.Close();
                fs.Close();
                yield return new WaitForSeconds(0.1f);
                downloadAction?.Invoke(request.progress, DownloadType.Complete);

            }
        }

    }

    public void OpenApk(string path)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                var FileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");
                var Intent = new AndroidJavaClass("android.content.Intent");
                var ACTION_VIEW = Intent.GetStatic<string>("ACTION_VIEW");
                var FLAG_ACTIVITY_NEW_TASK = Intent.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
                var FLAG_GRANT_READ_URI_PERMISSION = Intent.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");
                var intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);
                var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                var file = new AndroidJavaObject("java.io.File", path);
                var uri = FileProvider.CallStatic<AndroidJavaObject>("getUriForFile", currentActivity, "com.sailfish.physicalgramming.fileprovider", file);
                intent.Call<AndroidJavaObject>("setFlags", FLAG_ACTIVITY_NEW_TASK);
                intent.Call<AndroidJavaObject>("addFlags", FLAG_GRANT_READ_URI_PERMISSION);
                intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");

                currentActivity.Call("startActivity", intent);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
#endif
    }
    /// <summary>
    /// 从外部路径获取字符串
    /// </summary>
    /// <param name="path"></param>
    /// <param name="completeAction"></param>
    public void GetTextStrFromFile(string path, Action<string> completeAction)
    {
        StartCoroutine(WaitForGetTextString(path, completeAction));
    }

    IEnumerator WaitForGetTextString(string path, Action<string> completeAction)
    {
#if UNITY_ANDROID
        path = "file://" + path;
#endif
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            completeAction?.Invoke(request.downloadHandler.text);
        }
        else
        {
            completeAction?.Invoke("");
        }
    }

    /// <summary>
    /// 加载场景的ab包
    /// </summary>
    /// <param name="path"></param>
    /// <param name="complete"></param>
    public void GetBundleFormFile(string path, Action<AssetBundle> complete)
    {
        StartCoroutine(WaitForGetBundle(path, complete));
    }
    private IEnumerator WaitForGetBundle(string path, Action<AssetBundle> complete)
    {
#if UNITY_ANDROID
        path = "file://" + path;
#endif
        UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(path);
        yield return webRequest.SendWebRequest();
        if (webRequest.isDone)
        {
            AssetBundle ab = (webRequest.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
            //Material[] m = ab.LoadAllAssets<Material>();
            //Debug.Log(m[0].name);
            complete?.Invoke(ab);
        }
        else
        {
            complete?.Invoke(null);
        }
    }


    #region 音频资源获取

    public void GetSound(string url, Action<AudioClip> callback)
    {
        if (!string.IsNullOrEmpty(url))
        {
            StartCoroutine(GetSoundCorountine(url, callback));
        }
        else
        {
            callback?.Invoke(null);
        }
    }

    private IEnumerator GetSoundCorountine(string url, Action<AudioClip> callback)
    {
#if UNITY_ANDROID
        url = "file://" + url;
#endif
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        //Debug.Log("url:" + url);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            AudioClip _audioClip = DownloadHandlerAudioClip.GetContent(request);
            if (_audioClip != null)
                callback?.Invoke(_audioClip);
        }
        else
        {
            callback?.Invoke(null);
        }
    }

    #endregion

    #region 图片资源获取

    public void GetTexture(string url, Action<Texture2D> callback, bool isLocal = true)
    {
        if (!string.IsNullOrEmpty(url))
        {
            StartCoroutine(GetTextureCorountine(url, callback, isLocal));
        }
        else
        {
            callback?.Invoke(null);
        }
    }

    private IEnumerator GetTextureCorountine(string url, Action<Texture2D> callback, bool isLocal = true)
    {
        yield return 0;
        if (isLocal)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
                url = "file://" + url;
#endif
        }
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        //Debug.Log("url:" + url);
        yield return request.SendWebRequest();
        try
        {
            if (request.isDone)
            {
                Texture2D texture = null;
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        texture = DownloadHandlerTexture.GetContent(request);
                        if (texture != null)
                        {
                            callback?.Invoke(texture);
                            break;
                        }
                    }
                    catch { Debug.Log("加载缩略图:" + i); }
                }
                if (texture == null)
                    callback?.Invoke(null);
            }
            else
            {
                callback?.Invoke(null);
            }
        }
        catch
        {
            callback?.Invoke(null);
        }
    }

    public void GetTextureList(List<string> urls, Action<List<Texture2D>> callback, bool isLocal = true)
    {
        if (urls.Count > 0)
        {
            StartCoroutine(GetTextureListCorountine(urls, callback, isLocal));
        }
        else
        {
            callback?.Invoke(null);
        }
    }

    private IEnumerator GetTextureListCorountine(List<string> urls, Action<List<Texture2D>> callback, bool isLocal = true)
    {
        yield return 0;
        if (isLocal)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
                url = "file://" + url;
#endif
        }

        List<Texture2D> textures = new List<Texture2D>();
        foreach (var url in urls)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            Debug.Log("url:" + url);
            yield return request.SendWebRequest();
            try
            {
                if (request.isDone)
                {
                    Texture2D texture = null;
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            texture = DownloadHandlerTexture.GetContent(request);
                            if (texture != null)
                            {
                                //callback?.Invoke(texture);
                                textures.Add(texture);
                                break;
                            }
                        }
                        catch { Debug.Log("加载缩略图:" + i); }
                    }
                    if (texture == null)
                        textures.Add(defaultTexture);
                }
                else
                {
                    textures.Add(defaultTexture);
                }
            }
            catch
            {
                textures.Add(defaultTexture);
            }
        }
        callback?.Invoke(textures);
    }

    #endregion

    #region 地图存档获取

    // /// <summary>
    // /// 获取自定义关卡信息
    // /// </summary>
    // public void GetCustomMapInfo(string path, Action<Map> completeAction)
    // {
    //     StartCoroutine(WaitGetCustomMapInfo(path, completeAction));
    // }

    // public IEnumerator WaitGetCustomMapInfo(string path, Action<Map> completeAction)
    // {
    //     yield return 0;
    //     BinaryFormatter bf = new BinaryFormatter();
    //     FileStream file = File.Open(path, FileMode.Open);
    //     Map m = (Map)bf.Deserialize(file);
    //     file.Close();
    //     completeAction?.Invoke(m);

    // }


    #endregion




    protected override void OnDestroy()
    {
        base.OnDestroy();

    }

    private void OnApplicationQuit()
    {
    }

}

public enum DownloadType
{
    Downloading,
    Complete,
    Fail
}
