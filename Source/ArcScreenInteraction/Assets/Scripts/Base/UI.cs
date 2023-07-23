using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


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
        //Debug.Log("Show");
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
    /// <summary>
    /// 按钮果冻效果
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="size"></param>
    /// <param name="completeAction"></param>
    /// <param name="scaleEaseType"></param>
    /// <param name="isMinis"></param>
    public void ScaleTransform(Transform tran, float size = 1.3f, Action completeAction = null, Ease scaleEaseType = Ease.InQuad, bool isMinis = true)
    {
        if (isMinis)
        {
            tran.localScale = Vector3.zero;
        }
        tran.DOScale(Vector3.one * size, 0.3f).SetEase(scaleEaseType).OnComplete(() =>
        {
            tran.DOScale(Vector3.one, 0.2f).OnComplete(() =>
            {
                completeAction?.Invoke();
            });
        });
    }

    public void ScaleTransform(Transform tran, bool isMinis)
    {
        ScaleTransform(tran, 1.3f, null, Ease.InQuad, isMinis);
    }
    public void ScaleTransform(Transform tran, float size, bool isMinis)
    {
        ScaleTransform(tran, size, null, Ease.InQuad, isMinis);
    }
    public void ScaleTransform(Transform tran, Action completeAction, bool isMinis)
    {
        ScaleTransform(tran, 1.3f, completeAction, Ease.InQuad, isMinis);
    }
    public void ScaleTransform(Transform tran, Action completeAction, float size, bool isMinis)
    {
        ScaleTransform(tran, size, completeAction, Ease.InQuad, isMinis);
    }
    /// <summary>
    /// 左右侧面板平移方法
    /// </summary>
    public void SideMoveTransform(RectTransform tran, Side initSize)
    {
        float posX = tran.anchoredPosition.x;
        float posY = tran.anchoredPosition.y;
        switch (initSize)
        {
            case Side.Left:
                tran.anchoredPosition = new Vector3(-1000f, tran.anchoredPosition.y, 0);
                tran.DOAnchorPos3DX(posX, 0.2f).OnComplete(() =>
                {
                    ScaleTransform(tran, 1.1f, false);
                });
                break;
            case Side.Right:
                tran.anchoredPosition = new Vector3(1000f, tran.anchoredPosition.y, 0);
                tran.DOAnchorPos3DX(posX, 0.2f).OnComplete(() =>
                {
                    ScaleTransform(tran, 1.1f, false);
                });
                break;
            case Side.Up:
                tran.anchoredPosition = new Vector3(tran.anchoredPosition.x, 1000, 0);
                tran.DOAnchorPos3DY(posY, 0.2f).OnComplete(() =>
                {
                    ScaleTransform(tran, 1.1f, false);
                });
                break;
            case Side.Down:
                tran.anchoredPosition = new Vector3(tran.anchoredPosition.x, -1000, 0);
                tran.DOAnchorPos3DY(posY, 0.2f).OnComplete(() =>
                {
                    ScaleTransform(tran, 1.1f, false);
                });
                break;
        }
        tran.GetComponent<CanvasGroup>().alpha = 1;

    }

    public enum Side
    {
        Left,
        Right,
        Up,
        Down
    }
}

