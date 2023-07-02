using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private UI[] uIs;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        foreach (var i in uIs)
        {
            if (i.CurrentState == UIState.Hide)
            {
                i.HideUI();
            }
        }
    }

    public void InitUIType()
    {
        uIs = FindObjectsOfType<UI>();
        foreach (var ui in uIs)
        {
            if (ui.CurrentState == UIState.Hide)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 获取指定UI
    /// </summary>
    /// <param name="uIType"></param>
    /// <returns></returns>
    public UI GetUI(UIType uIType)
    {
        for (int i = 0; i < uIs.Length; i++)
        {
            if (uIs[i].CurrentUIType == uIType)
                return uIs[i];
        }
        return null;
    }
    /// <summary>
    /// 隐藏所有UI
    /// </summary>
    public void HideAllUI()
    {
        for (int i = 0; i < uIs.Length; i++)
        {
            uIs[i].HideUI();
        }

    }

    public void HideAllUIExcept(UIType ui)
    {
        for (int i = 0; i < uIs.Length; i++)
        {
            if (uIs[i].CurrentUIType == ui)
            {
                uIs[i].ShowUI();
            }
            else
            {
                uIs[i].HideUI();
            }
        }

    }
    /// <summary>
    /// 批量改变UI状态
    /// </summary>
    /// <param name="selectUIs"></param>
    /// <param name="state"></param>
    public void SetUIsState(UI[] selectUIs, UIState state)
    {
        for (int i = 0; i < selectUIs.Length; i++)
        {
            selectUIs[i].SetUIState(state);
        }
    }

    public void SetUIState(UI ui, UIState state)
    {
        ui.SetUIState(state);
    }

    public void ToggleUIs(UI[] selectUIs)
    {
        for (int i = 0; i < selectUIs.Length; i++)
        {
            selectUIs[i].Toggle();
        }
    }

    public void ToggleUI(UI selectUI)
    {
        selectUI.Toggle();
    }
}
