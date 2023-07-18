using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaListUI : UI
{
    private Button hideBtn;
    public Button HideBtn
    {
        get
        {
            if (hideBtn == null) hideBtn = transform.Find("Group/Hide").GetComponent<Button>();
            return hideBtn;
        }
    }
    [SerializeField] ScrollView scrollView = default;
    [SerializeField] Button prevCellButton = default;
    [SerializeField] Button nextCellButton = default;
    [SerializeField] Button playButton = default;
    [SerializeField] GameObject togglePrefab = default;

    [SerializeField] Transform classesToggleParent;
    private List<Toggle> classesToggle = new List<Toggle>();



    private void OnEnable()
    {


        HideBtn.onClick.AddListener(HideClick);
        prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(OnSelectionChanged);

    }


    private MediaData currentMediaDatas;
    private List<Media> MediaList = new List<Media>();
    private Media currentMediaData;

    private int currentIndex = 0;

    private void ShowConfirmPanel()
    {


    }
    private void PlayClick()
    {
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.Init(MediaList, currentIndex);
        HideUI();
    }

    void OnSelectionChanged(int index)
    {
        currentIndex = index;
        Debug.Log($"Selected item info: index {index}");
        // selectedItemInfo.text = $"Selected item info: index {index}"
    }
    //显示父级界面
    public void InitMediaList(SecurityType securityType)
    {
        switch (securityType)
        {

            case SecurityType.xiaofang:
                var items = MediaManager.Instance.mediaDatasScriptableAsset.mediaDatas[1];
                currentMediaDatas = items;
                MediaList = CopyMedias(currentMediaDatas.medias);
                InitMediaList(MediaList);
                break;
            case SecurityType.anfang:
                var items1 = MediaManager.Instance.mediaDatasScriptableAsset.mediaDatas[0];
                currentMediaDatas = items1;
                MediaList = CopyMedias(currentMediaDatas.medias);
                InitMediaList(MediaList);
                break;
            case SecurityType.renfang:
                var items2 = MediaManager.Instance.mediaDatasScriptableAsset.mediaDatas[2];
                currentMediaDatas = items2;
                MediaList = CopyMedias(currentMediaDatas.medias);
                InitMediaList(MediaList);
                break;
        }
    }

    private void InitMediaList(List<Media> medias)
    {
        List<ItemData> itemDatas = new List<ItemData>();
        foreach (var i in medias)
        {
            itemDatas.Add(new ItemData(i, ShowDetailMedia));
        }

        scrollView.UpdateData(itemDatas);
        if (itemDatas.Count > 3)
        {
            scrollView.SelectCell(2);
        }
        else if (itemDatas.Count > 2)
        {
            scrollView.SelectCell(1);
        }
        else
        {
            scrollView.SelectCell(0);
        }
    }


    public void ShowDetailMedia(Media media)
    {
        currentMediaData = media;
        PlayClick();
    }

    private void HideClick()
    {
        MainUI ui = UIManager.Instance.GetUI(UIType.Main) as MainUI;
        ui.OverviewBtn.isOn = true;
        HideUI();

    }

    public void InitClasses()
    {
        foreach (var i in classesToggle)
        {
            Destroy(i.gameObject);
        }
        classesToggle.Clear();

        foreach (var t in currentMediaDatas.classes)
        {
            var toggle = Instantiate(togglePrefab, classesToggleParent).GetComponent<Toggle>();
            toggle.gameObject.SetActive(true);
            toggle.GetComponentInChildren<Text>().text = t;
            toggle.group = classesToggleParent.GetComponent<ToggleGroup>();
            classesToggle.Add(toggle);
            toggle.onValueChanged.AddListener(classChanged);
        }
    }

    private void classChanged(bool isOn)
    {
        if (isOn)
        {
            MediaList.Clear();
            var index = classesToggle.FindIndex(x => x.isOn);
            Debug.Log(index);
            Debug.Log(currentMediaDatas.classes[index]);
            MediaList = currentMediaDatas.medias.FindAll(x => x.mediaClass == currentMediaDatas.classes[index]);
            Debug.Log("MediaList.Count" + MediaList.Count);
        }
        else
        {
            int i = -1;
            MediaList.Clear();
            i = classesToggle.FindIndex(x => x.isOn);
            if (i == -1)
            {
                MediaList = CopyMedias(currentMediaDatas.medias);
            }
        }

        InitMediaList(MediaList);
    }
    /// <summary>
    /// 需要复制一份，不然会改变原来的数据
    /// </summary>
    /// <param name="medias"></param>
    /// <returns></returns>
    private List<Media> CopyMedias(List<Media> medias)
    {
        List<Media> list = new List<Media>();
        foreach (var i in medias)
        {
            Media m = new Media()
            {
                mediaClass = i.mediaClass,
                mediaName = i.mediaName,
                mediaPath = i.mediaPath,
                mediaType = i.mediaType,
                coverPath = i.coverPath
            };
            list.Add(m);
        }

        return list;
    }



    private void OnDisable()
    {
        HideBtn.onClick.RemoveListener(HideClick);
        prevCellButton.onClick.RemoveListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.RemoveListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(OnSelectionChanged);
        //playButton.onClick.RemoveListener(PlayClick);

    }

}
