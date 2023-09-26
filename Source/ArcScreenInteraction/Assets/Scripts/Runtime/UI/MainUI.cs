using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using ScriptableObjectArchitecture;

public class MainUI : UI
{
    private Toggle overviewBtn;
    public Toggle OverviewBtn
    {
        get
        {
            if (overviewBtn == null) overviewBtn = transform.Find("Selection/overview").GetComponent<Toggle>();
            return overviewBtn;
        }
    }

    private GameObject selection;
    public GameObject Selection
    {
        get
        {
            if (selection == null) selection = transform.Find("Selection").gameObject;
            return selection;
        }
    }
    [SerializeField] Text title = default;

    [Space(10)]
    [Header("设置面板")]
    [SerializeField] GameObject settingPanel = default;
    [SerializeField] Dropdown loopTypeDropdown = default;
    [SerializeField] Slider innerDelaySlider = default;
    [SerializeField] Text innerDelayText = default;
    [SerializeField] Slider outerDelaySlider = default;
    [SerializeField] Text outerDelayText = default;
    [SerializeField] Slider mainDelaySlider = default;
    [SerializeField] Text mainDelayText = default;
    [SerializeField] Button confirmButton = default;
    [SerializeField] RawImage bgImage = default;
    [SerializeField] RawImage bg2Image = default;

    [SerializeField] Text leftTopicTitle = default;
    [SerializeField] Text leftTopicContent = default;
    [SerializeField] GameObject leftTopicMoudle = default;
    [SerializeField] Text rightTopicTitle = default;
    [SerializeField] Text rightTopicContent = default;
    [SerializeField] GameObject rightTopicMoudle = default;

    [SerializeField] Button settingButton = default;
    //列表
    [SerializeField] RectTransform scrollView = default;
    [SerializeField] private GameObject cellPrefab;
    private SystemData currentSystemData;
    List<Texture2D> bgs = new List<Texture2D>();
    private List<string> itemNameList = new List<string>();
    private string backgroundFileName = "bg.jpg";
    private List<GameObject> cells = new List<GameObject>();

    IEnumerator Start()
    {
        yield return ResourceManager.Instance != null;
        GetMainBg();
        ResourceManager.Instance.GetMainItemList((s) =>
        {
            itemNameList = s;
            List<ItemData> itemDatas = new List<ItemData>();
            int j = 0;
            foreach (var i in itemNameList)
            {
                j++;
                GameObject cell = Instantiate(cellPrefab, scrollView);
                cell.name = i;
                cell.GetComponent<MainItem>().Init(i, ItemClick);
                cell.GetComponent<MainItem>().SelectAction = CellViewSelect;
                if (j == 1)
                {
                    //第一个 默认选中
                    cell.GetComponent<MainItem>().Select();
                }
                cells.Add(cell);
            }
            scrollView.anchoredPosition = new Vector2(0, scrollView.anchoredPosition.y);
            counter = 1;
            prePageCells.Clear();
            if (cells.Count > 0)
            {
                cells[0].GetComponent<MainItem>().Select();
            }

        });
    }

    [SerializeField] private float contentWidthOffset = 1808f;
    [SerializeField] private float lastEdgePosition = 1596f;
    [SerializeField] private float initScrollPosition = 240f;
    int counter = 1;
    public void CellViewSelect(MainItem cell)
    {
        Vector3 cellPosition = cell.GetComponent<RectTransform>().anchoredPosition;
        //下一页
        if (cellPosition.x == (initScrollPosition + (contentWidthOffset * counter)))
        {
            counter++;
            scrollView.anchoredPosition -= new Vector2(contentWidthOffset, 0);
            cell.Select();
        }

        if (cellPosition.x == (lastEdgePosition + (contentWidthOffset * (counter - 1))))
        {
            Debug.Log("counter:" + counter);
            if (!prePageCells.Contains(cell))
                prePageCells.Add(cell);
        }
        if (cellPosition.x == (lastEdgePosition + (contentWidthOffset * (counter - 2))))
        {
            if (prePageCells.Contains(cell))
            {
                counter--;
                prePageCells.Remove(cell);
                scrollView.anchoredPosition += new Vector2(contentWidthOffset, 0);
                cell.Select();
            }
        }

    }
    private List<MainItem> prePageCells = new List<MainItem>();

    void ItemClick(string itemName)
    {
        title.text = itemName;
        MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
        ui.gameObject.SetActive(true);
        ui.InitMediaList(itemName);
        ui.InitClasses();
        ui.ShowUI();
        GetCurrentItemBg(itemName);
        HideUI();
    }

    private void GetMainBg()
    {
        bgs = null;
        leftTopicMoudle.SetActive(false);
        rightTopicMoudle.SetActive(false);
        ResourceManager.Instance.GetBackgroundList((d) =>
        {
            ResourceManager.Instance.GetTextureList(d, (t) =>
            {
                bgs = t;
                if (t.Count > 0)
                {
                    bgImage.texture = bgs[0];
                    bgImage.color = new Color(1, 1, 1, 1);
                    bg2Image.color = new Color(1, 1, 1, 0);
                }
                if (t.Count > 1)
                {
                    if (loopMainBgCoroutine != null)
                        StopCoroutine(loopMainBgCoroutine);
                    loopMainBgCoroutine = StartCoroutine(LoopMainBg());
                }
            });
        });
        if (!string.IsNullOrEmpty(AssetUtility.GetMainContentJson()))
        {
            Topic t = JsonUtility.FromJson<Topic>(AssetUtility.GetMainContentJson());
            leftTopicContent.text = t.LeftDescription;
            leftTopicTitle.text = t.LeftTitle;
            if (leftTopicContent.text != "")
            {
                leftTopicMoudle.SetActive(true);
            }
            rightTopicContent.text = t.RightDescription;
            rightTopicTitle.text = t.RightTitle;
            if (rightTopicContent.text != "")
            {
                rightTopicMoudle.SetActive(true);
            }
        }
        else
        {
            leftTopicMoudle.SetActive(false);
            rightTopicMoudle.SetActive(false);
        }
    }
    public void GetCurrentItemBg(string currentItemName)
    {
        leftTopicMoudle.SetActive(false);
        rightTopicMoudle.SetActive(false);
        if (loopMainBgCoroutine != null)
            StopCoroutine(loopMainBgCoroutine);
        ResourceManager.Instance.GetTexture(AssetUtility.GetDetailDataFolder(currentItemName) + backgroundFileName, (t) =>
        {
            if (t != null)
            {
                bgImage.texture = t;
                bgImage.color = new Color(1, 1, 1, 1);
                bg2Image.color = new Color(1, 1, 1, 0);
                ResizeImage(bgImage);
                ResizeImage(bg2Image);
            }
        });
        if (!string.IsNullOrEmpty(AssetUtility.GetItemContentJson(currentItemName)))
        {
            Topic t = JsonUtility.FromJson<Topic>(AssetUtility.GetItemContentJson(currentItemName));
            leftTopicContent.text = t.LeftDescription;
            leftTopicTitle.text = t.LeftTitle;
            if (leftTopicContent.text != "")
            {
                leftTopicMoudle.SetActive(true);
            }
            rightTopicContent.text = t.RightDescription;
            rightTopicTitle.text = t.RightTitle;
            if (rightTopicContent.text != "")
            {
                rightTopicMoudle.SetActive(true);
            }
        }
        else
        {
            leftTopicMoudle.SetActive(false);
            rightTopicMoudle.SetActive(false);
        }
    }

    private int index = 0;
    Coroutine loopMainBgCoroutine;
    bool isBg = true;
    IEnumerator LoopMainBg()
    {
        isBg = true;
        bgImage.color = new Color(1, 1, 1, 1);
        bg2Image.color = new Color(1, 1, 1, 0);
        ResizeImage(bgImage);
        ResizeImage(bg2Image);

        while (true)
        {

            yield return new WaitForSeconds(mainDelay);
            yield return new WaitUntil(() => bgs != null);
            index++;
            if (index >= bgs.Count)
                index = 0;
            if (isBg)
            {
                isBg = false;
                if (bgs[index] != null)
                {
                    bg2Image.texture = bgs[index];
                    ResizeImage(bg2Image);
                    bg2Image.DOFade(1, 2.5f);
                    bgImage.DOFade(0, 2.5f);
                }
            }
            else
            {
                isBg = true;
                if (bgs[index] != null)
                {
                    bgImage.texture = bgs[index];
                    ResizeImage(bgImage);
                    bgImage.DOFade(1, 2.5f);
                    bg2Image.DOFade(0, 2.5f);
                }
            }
            yield return new WaitForSeconds(2.5f);
        }
    }

    private void OnEnable()
    {
        loopTypeDropdown.onValueChanged.AddListener(LoopTypeChange);
        innerDelaySlider.onValueChanged.AddListener(InnerDelayChange);
        outerDelaySlider.onValueChanged.AddListener(OuterDelayChange);
        mainDelaySlider.onValueChanged.AddListener(MainDelayChange);
        StartCoroutine(InitData());
    }
    IEnumerator InitData()
    {
        yield return new WaitForSeconds(0.5f);
        currentSystemData = MediaManager.Instance.setupDataScriptableAsset.data;
        loopTypeDropdown.value = (int)currentSystemData.setupData.loopType;
        innerDelaySlider.value = currentSystemData.setupData.innerDelay;
        outerDelaySlider.value = currentSystemData.setupData.outerDelay;
    }
    public List<string> mainList = new List<string>();

    private void LoopTypeChange(int index)
    {
        MediaManager.Instance.setupDataScriptableAsset.data.setupData.loopType = (LoopType)index;
        MediaManager.Instance.UpdateSetupAsset();
    }
    private void InnerDelayChange(float v)
    {
        innerDelayText.text = v.ToString();
        MediaManager.Instance.setupDataScriptableAsset.data.setupData.innerDelay = v;
        MediaManager.Instance.UpdateSetupAsset();
    }

    private void OuterDelayChange(float v)
    {
        MediaManager.Instance.setupDataScriptableAsset.data.setupData.outerDelay = v;
        MediaManager.Instance.UpdateSetupAsset();
        outerDelayText.text = v.ToString();
    }
    private void MainDelayChange(float v)
    {
        MediaManager.Instance.setupDataScriptableAsset.data.setupData.mainDelay = v;
        MediaManager.Instance.UpdateSetupAsset();
        mainDelayText.text = v.ToString();
    }

    // void AnfangClick(bool ison)
    // {
    //     if (ison)
    //     {
    //         title.text = "安防";
    //         MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
    //         ui.InitMediaList(SecurityType.anfang);
    //         ui.InitClasses();
    //         ui.ShowUI();
    //         GetAnfangBg();
    //         HideUI();
    //     }

    // }
    // void XiaofangClick(bool ison)
    // {
    //     if (ison)
    //     {
    //         title.text = "消防";
    //         MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
    //         ui.InitMediaList(SecurityType.xiaofang);
    //         ui.InitClasses();
    //         ui.ShowUI();
    //         GetXiaofangBg();
    //         HideUI();
    //     }
    // }
    // void RenfangClick(bool ison)
    // {
    //     if (ison)
    //     {
    //         title.text = "人防";
    //         MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
    //         ui.InitMediaList(SecurityType.renfang);
    //         ui.InitClasses();
    //         ui.ShowUI();
    //         GetRenfangBg();
    //         HideUI();
    //     }
    // }

    void OverviewClick(bool ison)
    {
        if (ison)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        title.text = "南海区安全服务运营中心";
        MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
        ui.HideUI();
        GetMainBg();
    }



    private void OnDisable()
    {
        // AnfangBtn.onValueChanged.RemoveListener(AnfangClick);
        // XiaofangBtn.onValueChanged.RemoveListener(XiaofangClick);
        // RenfangBtn.onValueChanged.RemoveListener(RenfangClick);
        //OverviewBtn.onValueChanged.AddListener(OverviewClick);
        innerDelaySlider.onValueChanged.RemoveAllListeners();
        loopTypeDropdown.onValueChanged.RemoveAllListeners();
        settingButton.onClick.RemoveAllListeners();
        confirmButton.onClick.RemoveAllListeners();
    }
    private void InitUI()
    {

    }
    public override void ShowUI()
    {
        //base.ShowUI();
        Selection.SetActive(true);
        scrollView.gameObject.SetActive(true);
        // overviewBtn.Select();
        // overviewBtn.isOn = true;
        Refresh();
        if (cells.Count > 0)
        {
            cells[0].GetComponent<MainItem>().Select();
        }
    }

    public override void HideUI()
    {
        //base.HideUI();
        Selection.SetActive(false);
        scrollView.gameObject.SetActive(false);
    }

    private float mainDelay
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.mainDelay;
        }
    }
    private void ResizeImage(RawImage image)
    {
        var imageFitter = image.GetComponent<ImageFitter>();
        if (imageFitter != null)
        {
            imageFitter.Fit();
        }
    }
}
