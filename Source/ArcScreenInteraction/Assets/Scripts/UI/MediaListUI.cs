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
    //[SerializeField] ScrollView scrollView = default;
    [SerializeField] Button prevCellButton = default;
    [SerializeField] Button nextCellButton = default;
    [SerializeField] Button playButton = default;
    [SerializeField] GameObject togglePrefab = default;

    [SerializeField] Transform classesToggleParent;
    private List<Toggle> classesToggle = new List<Toggle>();
    //小点
    [SerializeField] Transform toggleSelectParent;
    [SerializeField] GameObject toggleSelectPrefab;
    [SerializeField] Transform contentParent;
    List<Toggle> toggleSelectList = new List<Toggle>();
    //列表 
    private int totalPage = 0;
    private int currentPage = 0;
    private int pageSize = 4;
    [SerializeField] private GameObject cellPrefab;
    private List<GameObject> cells = new List<GameObject>();



    private void OnEnable()
    {
        HideBtn.onClick.AddListener(HideClick);
        prevCellButton.onClick.AddListener(LastPage);
        nextCellButton.onClick.AddListener(NextPage);

    }

    private void NextPage()
    {
        if (currentPage < totalPage - 1)
        {
            currentPage++;
            //ChangePage();
            toggleSelectList[currentPage].isOn = true;
        }

    }
    private void LastPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            toggleSelectList[currentPage].isOn = true;
        }
    }


    private MediaData currentMediaDatas;
    private List<Media> MediaList = new List<Media>();
    private Media currentMediaData;



    private void ShowConfirmPanel()
    {


    }
    private void PlayClick()
    {
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.Init(MediaList, currentMediaData);
        HideUI();
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

        foreach (var c in cells)
        {
            Destroy(c);
        }
        cells.Clear();
        int j = 0;
        foreach (var i in medias)
        {
            j++;
            GameObject cell = Instantiate(cellPrefab, contentParent);
            cell.name = "cell" + j;
            cell.GetComponent<CellView>().Init(i, ShowDetailMedia);
            cells.Add(cell);
        }
        totalPage = (int)Math.Ceiling((float)cells.Count / pageSize);
        currentPage = 0;
        Debug.Log("totalPage:" + totalPage);
        ChangePage();
        InitDots();

    }

    private void ChangePage()
    {
        int count = cells.Count;
        for (int i = 0; i < count; i++)
        {
            cells[i].SetActive(false);
        }
        int start = currentPage * pageSize;
        int end = (currentPage + 1) * pageSize;
        if (end > cells.Count)
        {
            end = cells.Count;
        }
        Debug.Log("start:" + start + "end:" + end + "cells.Count:" + cells.Count);

        for (int j = start; j < end; j++)
        {
            cells[j].SetActive(true);
        }

    }

    private void InitDots()
    {
        bool flag = false;
        foreach (var i in toggleSelectList)
        {
            Destroy(i.gameObject);
        }
        toggleSelectList.Clear();
        //下面的小圆点
        for (int i = 0; i < totalPage; i++)
        {
            GameObject ob = Instantiate(toggleSelectPrefab, toggleSelectParent);
            ob.SetActive(true);
            ToggleCheck tc = ob.GetComponent<ToggleCheck>();
            tc.index = i;
            tc.CheckAction += OnToggle;
            if (!flag)
            {
                flag = true;
                ob.GetComponent<Toggle>().isOn = true;
            }
            toggleSelectList.Add(ob.GetComponent<Toggle>());
            ob.GetComponent<Toggle>().group = toggleSelectParent.GetComponent<ToggleGroup>();
        }
    }

    private void OnToggle(int page)
    {
        currentPage = page;
        ChangePage();
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
            MediaList = currentMediaDatas.medias.FindAll(x => x.mediaClass == currentMediaDatas.classes[index]);
            currentPage = 0;
            InitMediaList(MediaList);
        }
        else
        {
            int i = -1;

            i = classesToggle.FindIndex(x => x.isOn);
            Debug.Log("是否为-1:" + i);
            if (i == -1)
            {
                MediaList.Clear();
                MediaList = CopyMedias(currentMediaDatas.medias);
                currentPage = 0;
                InitMediaList(MediaList);
            }
        }


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
        prevCellButton.onClick.RemoveAllListeners();
        nextCellButton.onClick.RemoveAllListeners();
        //playButton.onClick.RemoveListener(PlayClick);

    }

}
