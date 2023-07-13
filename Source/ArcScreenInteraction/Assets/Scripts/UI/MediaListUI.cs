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



    private void OnEnable()
    {


        HideBtn.onClick.AddListener(HideClick);
        prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(OnSelectionChanged);
        //playButton.onClick.AddListener(PlayClick);
    }


    private List<MediaData> currentMediaDatas;
    private MediaData currentMediaData;

    private int currentIndex = 0;

    private void ShowConfirmPanel()
    {


    }
    private void PlayClick()
    {
        MediaPlayUI ui = UIManager.Instance.GetUI(UIType.MediaPlayUI) as MediaPlayUI;
        ui.Init(currentMediaData, currentIndex);
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
                var items = MediaManager.Instance.mediaDatasScriptableAsset.data.xiaofangDatas;
                List<ItemData> itemDatas = new List<ItemData>();
                foreach (var i in items)
                {
                    itemDatas.Add(new ItemData(i, InitMediaDataList));
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
                    itemDatas2.Add(new ItemData(i, InitMediaDataList));
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
                    itemDatas3.Add(new ItemData(i, InitMediaDataList));
                }
                currentMediaDatas = items2;
                scrollView.UpdateData(itemDatas3);
                scrollView.SelectCell(0);
                break;
        }
        scrollView.SelectCell(0);
    }
    //显示子级界面
    public void InitMediaDataList(MediaData data)
    {
        List<ItemData> itemDatas = new List<ItemData>();
        foreach (var i in data.medias)
        {
            itemDatas.Add(new ItemData(data, i, ShowDetailMedia));
        }
        //currentMediaDatas = items;
        scrollView.UpdateData(itemDatas);
        scrollView.SelectCell(0);
    }

    public void ShowDetailMedia(MediaData data, Media media)
    {
        Debug.Log($"ShowDetailMedia {data} {media}");
        currentMediaData = data;
        PlayClick();
    }

    private void HideClick()
    {
        MainUI ui = UIManager.Instance.GetUI(UIType.Main) as MainUI;
        ui.OverviewBtn.isOn = true;
        HideUI();

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
