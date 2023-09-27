using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

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
    [SerializeField] GameObject classTogglePrefab = default;
    [SerializeField] Transform classesToggleParent;
    private List<Toggle> classesToggle = new List<Toggle>();
    [SerializeField] RectTransform contentParent;
    [SerializeField] private GameObject cellPrefab;
    private List<GameObject> cells = new List<GameObject>();
    protected override void OnEnable()
    {
        base.OnEnable();
        HideBtn.onClick.AddListener(HideClick);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        HideBtn.onClick.RemoveListener(HideClick);
    }
    private MediaData currentMediaDatas;
    private List<Media> MediaList = new List<Media>();
    private Media currentMediaData;

    private void PlayClick()
    {
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.Init(MediaList, currentMediaData);
        HideUI();
    }

    public void InitMediaList(string itemName)
    {
        var items = MediaManager.Instance.GetMediaDataItem(itemName);
        currentMediaDatas = items;
        MediaList = CopyMedias(currentMediaDatas.Medias);
        InitClasses();
    }
    IEnumerator WaitForSelect(GameObject obj)
    {
        yield return 0;
        EventSystem.current.SetSelectedGameObject(null);
        yield return 0;
        EventSystem.current.SetSelectedGameObject(obj);
    }
    private void ClearChildrenSafe(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            children.Add(parent.GetChild(i));
        }
        foreach (var i in children)
        {
            Destroy(i.gameObject);
        }
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


    /// <summary>
    /// 初始化分类
    /// </summary>
    public async void InitClasses()
    {
        foreach (var i in classesToggle)
        {
            Destroy(i.gameObject);
        }
        classesToggle.Clear();
        for (int i = 0; i < currentMediaDatas.ClassesData.Count; i++)
        {
            var data = currentMediaDatas.ClassesData[i];
            var toggle = Instantiate(classTogglePrefab, classesToggleParent).GetComponent<Toggle>();
            toggle.gameObject.SetActive(true);
            var toggleClass = toggle.GetComponent<ToggleClasses>();
            toggleClass.SetData(data);
            toggleClass.SelectAction += OnSelectClassChanged;
            toggle.group = classesToggleParent.GetComponent<ToggleGroup>();
            classesToggle.Add(toggle);
        }
        if (classesToggle.Count > 0)
        {
            var toggle = classesToggle[0];
            await UniTask.DelayFrame(1);
            toggle.isOn = true;
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
    }
    override public void HideUI()
    {
        base.HideUI();
    }

    private void OnSelectClassChanged(string className)
    {
        MediaList.Clear();
        MediaList = currentMediaDatas.Medias.FindAll(x => x.mediaClass == className);
        DisplayMediaList(MediaList);
    }
    private void DisplayMediaList(List<Media> mediaList)
    {
        ClearChildrenSafe(contentParent);
        cells.Clear();
        for (int i = 0; i < mediaList.Count; i++)
        {
            var media = mediaList[i];
            var cell = Instantiate(cellPrefab, contentParent);
            cell.SetActive(true);
            var cellView = cell.GetComponent<CellView>();
            cellView.Init(media, ShowDetailMedia);
            cells.Add(cell);
            if (i == 0)
            {
                StartCoroutine(WaitForSelect(cell));
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
}
