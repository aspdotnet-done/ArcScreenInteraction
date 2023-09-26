using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
public enum UIState
{
    Hide,
    Show
}

public enum UIType
{
    normal,
    Main,
    MediaListUI,
    MediaPlayUI

}

public enum HideMode
{
    Fade,
    UpDown

}

public class UI : MonoBehaviour
{
    public UIType CurrentUIType = UIType.normal;
    public UIState CurrentState = UIState.Show;
    public HideMode hideMode = HideMode.Fade;

    [Range(0, 2)] public float showDuration = 0.2f;
    [Range(0, 2)] public float hideDuration = 0.2f;

    public Selectable defaultSelectComponent;

    public float HideHeight = 1080;

    private RectTransform panel;

    public RectTransform Panel
    {
        get
        {
            if (panel == null)
            {
                panel = GetComponent<RectTransform>();
            }

            return panel;
        }
    }
    public bool useBackAction = true;

    [SerializeField] GameEvent backEvent;
    protected virtual void OnEnable()
    {
        backEvent.AddListener(OnBack);
    }
    protected virtual void OnDisable()
    {
        backEvent.RemoveListener(OnBack);
    }

    public virtual void OnBack()
    {
        if (!useBackAction) return;
        if (CurrentState == UIState.Show && this.enabled == true)
        {
            HideUI();
            Debug.Log("back");
        }
    }

    public virtual void ShowUI()
    {
        if (hideMode == HideMode.Fade)
        {
            if (CurrentState != UIState.Show)
            {
                Panel.GetComponent<CanvasGroup>().alpha = 0;
                Panel.GetComponent<CanvasGroup>().DOFade(1, showDuration);
            }
        }
        else
        {
            Panel.anchoredPosition = new Vector2(Panel.anchoredPosition.x, HideHeight);
            Panel.DOAnchorPosY(0, showDuration);
        }
        gameObject.SetActive(true);
        CurrentState = UIState.Show;
        if (defaultSelectComponent != null)
        {
            defaultSelectComponent.Select();
        }
        Debug.Log("Show");
    }

    public virtual void HideUI()
    {
        if (hideMode == HideMode.Fade)
        {
            Panel.GetComponent<CanvasGroup>().DOFade(0, hideDuration).OnComplete(() =>
            {
                Panel.gameObject.SetActive(false);
            });
        }
        else
        {
            Panel.DOAnchorPosY(HideHeight, hideDuration).OnComplete(() =>
            {
                Panel.gameObject.SetActive(false);
            });
        }
        CurrentState = UIState.Hide;
    }

    public virtual void Toggle()
    {
        if (CurrentState == UIState.Hide)
            ShowUI();
        else
            HideUI();
    }

    public void SetUIState(UIState state)
    {
        switch (state)
        {
            case UIState.Hide:
                HideUI();
                break;
            case UIState.Show:
                ShowUI();
                break;
            default:
                break;
        }
    }
}

