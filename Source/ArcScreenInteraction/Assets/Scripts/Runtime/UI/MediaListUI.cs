using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] RectTransform contentParent;
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
            //toggleSelectList[currentPage].isOn = true;
            ChangePage();
        }

    }
    private void LastPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            //toggleSelectList[currentPage].isOn = true;
            ChangePage();
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
    // public void InitMediaList(SecurityType securityType)
    // {
    //     switch (securityType)
    //     {

    //         case SecurityType.xiaofang:
    //             var items = MediaManager.Instance.mediaDatasScriptableAsset.mediaDatas[1];
    //             currentMediaDatas = items;
    //             MediaList = CopyMedias(currentMediaDatas.medias);
    //             InitMediaList(MediaList);
    //             break;
    //         case SecurityType.anfang:
    //             var items1 = MediaManager.Instance.mediaDatasScriptableAsset.mediaDatas[0];
    //             currentMediaDatas = items1;
    //             MediaList = CopyMedias(currentMediaDatas.medias);
    //             InitMediaList(MediaList);
    //             break;
    //         case SecurityType.renfang:
    //             var items2 = MediaManager.Instance.mediaDatasScriptableAsset.mediaDatas[2];
    //             currentMediaDatas = items2;
    //             MediaList = CopyMedias(currentMediaDatas.medias);
    //             InitMediaList(MediaList);
    //             break;
    //     }
    // }

    public void InitMediaList(string itemName)
    {

        var items = MediaManager.Instance.GetMediaDataItem(itemName);
        currentMediaDatas = items;
        MediaList = CopyMedias(currentMediaDatas.medias);
        InitMediaList(MediaList);

    }

    private void InitMediaList(List<Media> medias)
    {
        for (int i = 0; i < contentParent.childCount; i++)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }
        cells.Clear();
        Debug.Log("cells:" + cells.Count);
        int j = 0;
        foreach (var i in medias)
        {
            j++;
            GameObject cell = Instantiate(cellPrefab, contentParent);
            cell.name = "cell" + j;
            cell.GetComponent<CellView>().Init(i, ShowDetailMedia);
            cell.GetComponent<CellView>().SelectAction += CellViewSelect;
            if (j == 1)
            {
                //第一个 默认选中
                cell.GetComponent<CellView>().Select();
            }
            cells.Add(cell);

        }
        totalPage = (int)Math.Ceiling((float)cells.Count / 4);
        currentPage = 0;
        //Debug.Log("totalPage:" + totalPage);
        prevCellButton.gameObject.SetActive(false);
        if (cells.Count <= 4)
        {
            nextCellButton.gameObject.SetActive(false);
            prevCellButton.gameObject.SetActive(false);
        }
        else
        {
            nextCellButton.gameObject.SetActive(true);
        }
        ChangePage();
        InitDots();
        //contentWidth = 380 * cells.Count + 50 + 72 * (cells.Count - 1);
        contentParent.anchoredPosition = new Vector2(0, contentParent.anchoredPosition.y);
        counter = 1;
        prePageCells.Clear();
        if (cells.Count > 0)
        {
            cells[0].GetComponent<CellView>().Select();
        }
    }
    [SerializeField] private float contentWidthOffset = 1808f;
    [SerializeField] private float lastEdgePosition = 1596f;
    [SerializeField] private float initScrollPosition = 240f;
    int counter = 1;
    public void CellViewSelect(CellView cell)
    {

        // Vector3 cellPosition = cell.GetComponent<RectTransform>().anchoredPosition;
        // Debug.Log("cellPosition:" + cellPosition);
        // //下一页
        // if (cellPosition.x == (initScrollPosition + (contentWidthOffset * counter)))
        // {
        //     Debug.Log("1 counter:" + counter + "| prePageCells" + prePageCells.Count);
        //     counter++;
        //     contentParent.anchoredPosition -= new Vector2(contentWidthOffset, 0);
        //     cell.Select();
        // }
        // //1596

        // if (cellPosition.x == (lastEdgePosition + (contentWidthOffset * (counter - 1))))
        // {
        //     Debug.Log("2 counter:" + counter + "| prePageCells" + prePageCells.Count);
        //     if (!prePageCells.Contains(cell))
        //         prePageCells.Add(cell);
        // }
        // if (cellPosition.x == (lastEdgePosition + (contentWidthOffset * (counter - 2))))
        // {
        //     Debug.Log("3 counter:" + counter + "| prePageCells" + prePageCells.Count);
        //     if (prePageCells.Contains(cell))
        //     {
        //         counter--;
        //         prePageCells.Remove(cell);
        //         contentParent.anchoredPosition += new Vector2(contentWidthOffset, 0);
        //         cell.Select();
        //     }
        // }
        // //上一页是否显示
        // if (counter > 0)
        // {
        //     prevCellButton.gameObject.SetActive(true);
        // }
        // else
        // {
        //     prevCellButton.gameObject.SetActive(false);
        // }
        // //下一页是否显示
        // if (counter < totalPage)
        // {
        //     nextCellButton.gameObject.SetActive(true);
        // }
        // else
        // {
        //     nextCellButton.gameObject.SetActive(false);
        // }

    }
    private List<CellView> prePageCells = new List<CellView>();

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
        bool isSelect = false;
        for (int j = start; j < end; j++)
        {
            cells[j].SetActive(true);
            if (!isSelect)
            {
                StartCoroutine(WaitForSelect(cells[j]));
                isSelect = true;
            }
        }
        //上一页是否显示
        if (currentPage > 0)
        {
            prevCellButton.gameObject.SetActive(true);
        }
        else
        {
            prevCellButton.gameObject.SetActive(false);
        }
        //下一页是否显示
        if (currentPage < totalPage - 1)
        {
            nextCellButton.gameObject.SetActive(true);
        }
        else
        {
            nextCellButton.gameObject.SetActive(false);
        }

    }

    IEnumerator WaitForSelect(GameObject obj)
    {
        yield return 0;
        EventSystem.current.SetSelectedGameObject(null);
        yield return 0;
        EventSystem.current.SetSelectedGameObject(obj);
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



    private GameObject lastSelectObject;
    public void ShowDetailMedia(Media media)
    {
        lastSelectObject = EventSystem.current.currentSelectedGameObject;
        Debug.Log("lastSelectObject:" + lastSelectObject.name);
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
            toggle.GetComponentInChildren<AutoFit>().SetContent(t);
            toggle.gameObject.SetActive(true);
            //toggle.GetComponentInChildren<Text>().text = t;
            var autoLayout = toggle.GetComponent<AutoTextLayout>();
            if (autoLayout)
            {
                autoLayout.Fit();
            }
            toggle.group = classesToggleParent.GetComponent<ToggleGroup>();
            classesToggle.Add(toggle);
            toggle.onValueChanged.AddListener(classChanged);
        }
    }

    public override void OnBack()
    {
        if (CurrentState == UIState.Show)
        {
            MainUI ui = UIManager.Instance.GetUI(UIType.Main) as MainUI;
            ui.ShowUI();
            cells.Clear();
            lastSelectObject = null;
        }
    }

    override public void ShowUI()
    {
        base.ShowUI();
        if (lastSelectObject)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectObject);
        }
        AppManager.Instance.BackAction += OnBack;
    }
    override public void HideUI()
    {
        base.HideUI();
        //AppManager.Instance.BackAction -= OnBack;
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
