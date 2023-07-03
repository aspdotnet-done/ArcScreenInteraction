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

    private void OnEnable()
    {
        HideBtn.onClick.AddListener(HideClick);
        prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
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
                scrollView.UpdateData(itemDatas3);
                scrollView.SelectCell(0);
                break;
        }
    }

    private void HideClick()
    {
        HideUI();
    }

    private void OnDisable()
    {
        HideBtn.onClick.RemoveListener(HideClick);
        prevCellButton.onClick.RemoveListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.RemoveListener(scrollView.SelectNextCell);
    }

}
