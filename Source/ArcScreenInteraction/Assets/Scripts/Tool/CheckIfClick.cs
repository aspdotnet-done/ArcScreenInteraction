using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheckIfClick : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private float lastClickTime;
    public float hideTime = 10f;

    void Start()
    {
        lastClickTime = Time.time;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            canvasGroup.DOFade(1f, 0.2f).OnComplete(() =>
            {
                canvasGroup.interactable = true;
            });
            lastClickTime = Time.time;
        }

        if (Time.time - lastClickTime > hideTime)
        {
            canvasGroup.DOFade(0f, 0.2f);
            canvasGroup.interactable = false;

        }
    }
}
