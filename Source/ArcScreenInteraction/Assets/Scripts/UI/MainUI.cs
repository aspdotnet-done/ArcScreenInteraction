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

    private Toggle xiaofangBtn;
    public Toggle XiaofangBtn
    {
        get
        {
            if (xiaofangBtn == null) xiaofangBtn = transform.Find("Selection/xiaofang").GetComponent<Toggle>();
            return xiaofangBtn;
        }
    }

    private Toggle anfangBtn;
    public Toggle AnfangBtn
    {
        get
        {
            if (anfangBtn == null) anfangBtn = transform.Find("Selection/anfang").GetComponent<Toggle>();
            return anfangBtn;
        }
    }
    private Toggle renfangBtn;
    public Toggle RenfangBtn
    {
        get
        {
            if (renfangBtn == null) renfangBtn = transform.Find("Selection/renfang").GetComponent<Toggle>();
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
    [SerializeField] Slider mainDelaySlider = default;
    [SerializeField] Text mainDelayText = default;
    [SerializeField] Button confirmButton = default;
    [SerializeField] RawImage bgImage = default;
    [SerializeField] RawImage bg2Image = default;
    [SerializeField] Text topicTitle = default;
    [SerializeField] Text topicContent = default;
    [SerializeField] GameObject topicMoudle = default;
    [SerializeField] Button settingButton = default;


    private SystemData currentSystemData;
    List<Texture2D> bgs = new List<Texture2D>();
    public string backgroundFileName = "bg.jpg";
    IEnumerator Start()
    {
        yield return ResourceManager.Instance != null;
        //yield return new WaitUntil(() => MediaManager.Instance != null);
        GetMainBg();
    }


    private void GetMainBg()
    {
        bgs = null;
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
        ResourceManager.Instance.GetTexture(AssetUtility.GetAnfangFolder() + backgroundFileName, (t) =>
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
        ResourceManager.Instance.GetTexture(AssetUtility.GetXiaofangFolder() + backgroundFileName, (t) =>
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
        ResourceManager.Instance.GetTexture(AssetUtility.GetRenfangFolder() + backgroundFileName, (t) =>
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
                    bgImage.DOFade(1, 2.5f);
                    bg2Image.DOFade(0, 2.5f);
                }
            }
            yield return new WaitForSeconds(2.5f);
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
        mainDelaySlider.onValueChanged.AddListener(MainDelayChange);
        settingButton.onClick.AddListener(SettingShow);
        confirmButton.onClick.AddListener(ConfirmSettingClick);
        StartCoroutine(InitData());
    }

    public override void OnBack()
    {
        Debug.Log("main onback");
    }

    public void SettingShow()
    {
        settingPanel.SetActive(true);
        loopTypeDropdown.Select();
    }

    void ConfirmSettingClick()
    {
        settingPanel.SetActive(false);
        defaultSelectComponent.Select();
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
    private void MainDelayChange(float v)
    {
        MediaManager.Instance.setupDataScriptableAsset.data.setupData.mainDelay = v;
        MediaManager.Instance.UpdateSetupAsset();
        mainDelayText.text = v.ToString();
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
            HideUI();
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
            HideUI();
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
            HideUI();
        }
    }

    void OverviewClick(bool ison)
    {
        if (ison)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        title.text = "总览";
        MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
        ui.HideUI();
        GetMainBg();
    }



    private void OnDisable()
    {
        AnfangBtn.onValueChanged.RemoveListener(AnfangClick);
        XiaofangBtn.onValueChanged.RemoveListener(XiaofangClick);
        RenfangBtn.onValueChanged.RemoveListener(RenfangClick);
        OverviewBtn.onValueChanged.AddListener(OverviewClick);
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
        overviewBtn.Select();
        overviewBtn.isOn = true;
        Refresh();
    }

    public override void HideUI()
    {
        //base.HideUI();
        Selection.SetActive(false);
    }

    private float mainDelay
    {
        get
        {
            return MediaManager.Instance.setupDataScriptableAsset.data.setupData.mainDelay;
        }
    }
    public float maxWidth = 4576f;
    private void ResizeImage(RawImage image)
    {

        float aspectRatio = (float)image.texture.height / (float)image.texture.width;
        //float newHeight = Mathf.Min(maxHeight, image.texture.height);
        float newWidth = maxWidth;
        float newHeight = newWidth * aspectRatio;
        // if (newHeight < image.texture.height)
        // {
        image.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        // }
    }
}
