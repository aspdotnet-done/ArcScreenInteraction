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

    [Space(10)]
    [Header("设置面板")]

    [SerializeField] GameObject confirmPanel = default;
    [SerializeField] Dropdown loopTypeDropdown = default;
    [SerializeField] Slider innerDelaySlider = default;
    [SerializeField] Text innerDelayText = default;
    [SerializeField] Slider outerDelaySlider = default;
    [SerializeField] Text outerDelayText = default;
    [SerializeField] Button confirmButton = default;

    private void OnEnable()
    {


        HideBtn.onClick.AddListener(HideClick);
        prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(OnSelectionChanged);
        playButton.onClick.AddListener(PlayClick);
        loopTypeDropdown.onValueChanged.AddListener(LoopTypeChange);
        innerDelaySlider.onValueChanged.AddListener(InnerDelayChange);
        outerDelaySlider.onValueChanged.AddListener(OuterDelayChange);
        InitData();


    }

    private void InitData()
    {
        currentSystemData = MediaManager.Instance.setupDataScriptableAsset.data;
        loopTypeDropdown.value = (int)currentSystemData.setupData.loopType;
        innerDelaySlider.value = currentSystemData.setupData.innerDelay;
        outerDelaySlider.value = currentSystemData.setupData.outerDelay;

    }
    private List<MediaData> currentMediaDatas;
    private MediaData currentMediaData;
    private SystemData currentSystemData;
    private int currentIndex = 0;

    private void ShowConfirmPanel()
    {
        //todo 加载配置信息
        currentMediaData = currentMediaDatas[currentIndex];
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.Init(currentMediaData);
        HideUI();

    }
    private void PlayClick()
    {
        confirmPanel.SetActive(true);

    }

    void OnSelectionChanged(int index)
    {
        currentIndex = index;
        Debug.Log($"Selected item info: index {index}");
        // selectedItemInfo.text = $"Selected item info: index {index}"
    }

    public void InitMediaList(SecurityType securityType)
    {
        switch (securityType)
        {

            case SecurityType.xiaofang:
                var items = MediaManager.Instance.mediaDatasScriptableAsset.data.xiaofangDatas;
                List<ItemData> itemDatas = new List<ItemData>();
                foreach (var i in items)
                {
                    itemDatas.Add(new ItemData(i));
                }
                currentMediaDatas = items;
                scrollView.UpdateData(itemDatas);
                scrollView.SelectCell(0);
                break;
            case SecurityType.anfang:
                var items1 = MediaManager.Instance.mediaDatasScriptableAsset.data.anfangDatas;
                List<ItemData> itemDatas2 = new List<ItemData>();
                foreach (var i in items1)
                {
                    itemDatas2.Add(new ItemData(i));
                }
                currentMediaDatas = items1;
                scrollView.UpdateData(itemDatas2);
                scrollView.SelectCell(0);
                break;
            case SecurityType.renfang:
                var items2 = MediaManager.Instance.mediaDatasScriptableAsset.data.renfangDatas;
                List<ItemData> itemDatas3 = new List<ItemData>();
                foreach (var i in items2)
                {
                    itemDatas3.Add(new ItemData(i));
                }
                currentMediaDatas = items2;
                scrollView.UpdateData(itemDatas3);
                scrollView.SelectCell(0);
                break;
        }
        scrollView.SelectCell(0);
    }

    private void HideClick()
    {
        HideUI();
    }

    private void LoopTypeChange(int index)
    {
        MediaManager.Instance.setupDataScriptableAsset.data.setupData.loopType = (LoopType)index;
        MediaManager.Instance.UpdateSetupAsset();
        Debug.Log($"LoopTypeChange {index}");
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

    private void OnDisable()
    {
        HideBtn.onClick.RemoveListener(HideClick);
        prevCellButton.onClick.RemoveListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.RemoveListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(OnSelectionChanged);
        playButton.onClick.RemoveListener(PlayClick);
        innerDelaySlider.onValueChanged.RemoveAllListeners();
        loopTypeDropdown.onValueChanged.RemoveAllListeners();
    }

}
