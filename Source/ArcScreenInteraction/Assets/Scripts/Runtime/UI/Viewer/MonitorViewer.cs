using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProLiveCamera;
using System;
using System.IO;
using Cysharp.Threading.Tasks;
[Serializable]
public class MonitorData
{
    public string Name;
    public int VideoChannel;
}

public class MonitorViewer : BaseViewer
{
    [SerializeField] private AVProLiveCamera liveCamera;
    [SerializeField] AVProLiveCameraUGUIComponent liveCameraUGUIComponent;
    private MonitorData monitorData;
    public override void Show()
    {
        gameObject.SetActive(true);
        LoadMonitorData();
        SetUpLiveCamera();
    }
    public override void Hide()
    {
        if (liveCamera.Device != null)
        {
            liveCamera.Device.Stop();
        }
        gameObject.SetActive(false);
    }

    #region LiveCamera
    private async void SetUpLiveCamera()
    {
        liveCamera._deviceSelection = AVProLiveCamera.SelectDeviceBy.Index;
        liveCamera._desiredDeviceIndex = monitorData.VideoChannel - 1;
        liveCamera.Start();
        await UniTask.WaitUntil(() => liveCameraUGUIComponent.mainTexture != null);
        var fitter = liveCameraUGUIComponent.GetComponent<ImageFitter>();
        if (fitter != null)
        {
            liveCameraUGUIComponent.SetNativeSize();
            fitter.Fit();
        }
    }
    #endregion
    private void LoadMonitorData()
    {
        var json = File.ReadAllText(currentData.mediaPath);
        monitorData = JsonUtility.FromJson<MonitorData>(json);
    }
}
