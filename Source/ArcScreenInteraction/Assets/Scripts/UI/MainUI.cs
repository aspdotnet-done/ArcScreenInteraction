using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;


public class MainUI : UI
{


    private Transform bg;
    public Transform Bg
    {
        get
        {
            if (bg == null) bg = transform.parent.Find("BG");
            return bg;
        }
    }
    private Toggle overviewBtn;
    public Toggle OverviewBtn
    {
        get
        {
            if (overviewBtn == null) overviewBtn = transform.Find("Selection/overview/Toggle").GetComponent<Toggle>();
            return overviewBtn;
        }
    }

    private Toggle xiaofangBtn;
    public Toggle XiaofangBtn
    {
        get
        {
            if (xiaofangBtn == null) xiaofangBtn = transform.Find("Selection/xiaofang/Toggle").GetComponent<Toggle>();
            return xiaofangBtn;
        }
    }

    private Toggle anfangBtn;
    public Toggle AnfangBtn
    {
        get
        {
            if (anfangBtn == null) anfangBtn = transform.Find("Selection/anfang/Toggle").GetComponent<Toggle>();
            return anfangBtn;
        }
    }
    private Toggle renfangBtn;
    public Toggle RenfangBtn
    {
        get
        {
            if (renfangBtn == null) renfangBtn = transform.Find("Selection/renfang/Toggle").GetComponent<Toggle>();
            return renfangBtn;
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
    [SerializeField] Button confirmButton = default;
    [SerializeField] RawImage bgImage = default;
    [SerializeField] Text topicTitle = default;
    [SerializeField] Text topicContent = default;
    [SerializeField] GameObject topicMoudle = default;


    private SystemData currentSystemData;
    List<Texture2D> bgs = new List<Texture2D>();

    IEnumerator Start()
    {
        yield return ResourceManager.Instance != null;
        //yield return new WaitUntil(() => MediaManager.Instance != null);
        GetMainBg();
    }


    private void GetMainBg()
    {
        bgs = new List<Texture2D>();
        ResourceManager.Instance.GetBackgroundList((d) =>
        {
            ResourceManager.Instance.GetTextureList(d, (t) =>
            {
                bgs = t;
                if (t.Count > 0)
                    bgImage.texture = bgs[0];
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
            topicContent.text = t.Description;
            topicTitle.text = t.Title;
            topicMoudle.SetActive(true);
        }
        else
        {
            topicMoudle.SetActive(false);
        }
    }

    private void GetAnfangBg()
    {
        if (loopMainBgCoroutine != null)
            StopCoroutine(loopMainBgCoroutine);
        ResourceManager.Instance.GetTexture(AssetUtility.GetAnfangFolder() + "bg.png", (t) =>
        {
            if (t != null)
                bgImage.texture = t;
        });
        if (!string.IsNullOrEmpty(AssetUtility.GetAnfangContentJson()))
        {
            Topic t = JsonUtility.FromJson<Topic>(AssetUtility.GetAnfangContentJson());
            topicContent.text = t.Description;
            topicTitle.text = t.Title;
            topicMoudle.SetActive(true);
        }
        else
        {
            topicMoudle.SetActive(false);
        }
    }
    private void GetXiaofangBg()
    {
        if (loopMainBgCoroutine != null)
            StopCoroutine(loopMainBgCoroutine);
        ResourceManager.Instance.GetTexture(AssetUtility.GetXiaofangFolder() + "bg.png", (t) =>
        {
            if (t != null)
                bgImage.texture = t;
        });
        if (!string.IsNullOrEmpty(AssetUtility.GetXiaofangContentJson()))
        {
            Topic t = JsonUtility.FromJson<Topic>(AssetUtility.GetXiaofangContentJson());
            topicContent.text = t.Description;
            topicTitle.text = t.Title;
            topicMoudle.SetActive(true);
        }
        else
        {
            topicMoudle.SetActive(false);
        }
    }
    private void GetRenfangBg()
    {
        if (loopMainBgCoroutine != null)
            StopCoroutine(loopMainBgCoroutine);
        ResourceManager.Instance.GetTexture(AssetUtility.GetRenfangFolder() + "bg.png", (t) =>
        {
            if (t != null)
                bgImage.texture = t;
        });
        if (!string.IsNullOrEmpty(AssetUtility.GetRenfangContentJson()))
        {
            Topic t = JsonUtility.FromJson<Topic>(AssetUtility.GetRenfangContentJson());
            topicContent.text = t.Description;
            topicTitle.text = t.Title;
            topicMoudle.SetActive(true);
        }
        else
        {
            topicMoudle.SetActive(false);
        }
    }



    [SerializeField] public float delayTime = 5f;
    private int index = 0;
    Coroutine loopMainBgCoroutine;
    IEnumerator LoopMainBg()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayTime);
            index++;
            if (index >= bgs.Count)
                index = 0;
            bgImage.texture = bgs[index];
        }
    }

    private void OnEnable()
    {
        AnfangBtn.onValueChanged.AddListener(AnfangClick);
        XiaofangBtn.onValueChanged.AddListener(XiaofangClick);
        RenfangBtn.onValueChanged.AddListener(RenfangClick);
        OverviewBtn.onValueChanged.AddListener(OverviewClick);
        loopTypeDropdown.onValueChanged.AddListener(LoopTypeChange);
        innerDelaySlider.onValueChanged.AddListener(InnerDelayChange);
        outerDelaySlider.onValueChanged.AddListener(OuterDelayChange);
        StartCoroutine(InitData());
    }



    IEnumerator InitData()
    {
        yield return new WaitForSeconds(0.5f);
        //yield return new WaitUntil(() => MediaManager.Instance != null);
        //yield return new WaitUntil(() => MediaManager.Instance.setupDataScriptableAsset != null);
        currentSystemData = MediaManager.Instance.setupDataScriptableAsset.data;
        loopTypeDropdown.value = (int)currentSystemData.setupData.loopType;
        innerDelaySlider.value = currentSystemData.setupData.innerDelay;
        outerDelaySlider.value = currentSystemData.setupData.outerDelay;
    }

    private void LoopTypeChange(int index)
    {
        MediaManager.Instance.setupDataScriptableAsset.data.setupData.loopType = (LoopType)index;
        MediaManager.Instance.UpdateSetupAsset();
        //Debug.Log($"LoopTypeChange {index}");
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

    void AnfangClick(bool ison)
    {
        if (ison)
        {
            title.text = "安防";
            MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
            ui.InitMediaList(SecurityType.anfang);
            ui.InitClasses();
            ui.ShowUI();
            GetAnfangBg();
        }

    }
    void XiaofangClick(bool ison)
    {
        if (ison)
        {
            title.text = "消防";
            MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
            ui.InitMediaList(SecurityType.xiaofang);
            ui.InitClasses();
            ui.ShowUI();
            GetXiaofangBg();
        }
    }
    void RenfangClick(bool ison)
    {
        if (ison)
        {
            title.text = "人防";
            MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
            ui.InitMediaList(SecurityType.renfang);
            ui.InitClasses();
            ui.ShowUI();
            GetRenfangBg();
        }
    }

    void OverviewClick(bool ison)
    {
        if (ison)
        {
            title.text = "总览";
            MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
            ui.HideUI();

            GetMainBg();
        }
    }



    private void OnDisable()
    {
        AnfangBtn.onValueChanged.RemoveListener(AnfangClick);
        XiaofangBtn.onValueChanged.RemoveListener(XiaofangClick);
        RenfangBtn.onValueChanged.RemoveListener(RenfangClick);
        OverviewBtn.onValueChanged.AddListener(OverviewClick);
        innerDelaySlider.onValueChanged.RemoveAllListeners();
        loopTypeDropdown.onValueChanged.RemoveAllListeners();
    }
    private void InitUI()
    {

    }
    public override void ShowUI()
    {


        base.ShowUI();


    }

    public override void HideUI()
    {
        base.HideUI();
    }
}
