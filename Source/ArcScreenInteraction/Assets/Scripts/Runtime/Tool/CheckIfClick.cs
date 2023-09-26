using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;
public class CheckIfClick : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private float lastClickTime;
    public float hideTime = 10f;
    private bool isShow = false;
    [SerializeField] GameEvent submitEvent;
    void Start()
    {
        lastClickTime = Time.time;
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        submitEvent.AddListener(OnSubmit);
    }
    private void OnDisable()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        submitEvent.RemoveListener(OnSubmit);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isShow)
            {
                return;
            }
            isShow = true;
            canvasGroup.DOFade(1f, 0.2f).OnComplete(() =>
            {
                canvasGroup.interactable = true;
            });
            lastClickTime = Time.time;
        }

        if (isShow && (Time.time - lastClickTime > hideTime))
        {
            isShow = false;
            canvasGroup.DOFade(0f, 0.2f);
            canvasGroup.interactable = false;
        }
    }
    void OnSubmit()
    {
        if (isShow)
        {
            return;
        }
        isShow = true;
        canvasGroup.DOFade(1f, 0.2f).OnComplete(() =>
        {
            canvasGroup.interactable = true;
        });
        lastClickTime = Time.time;
    }
}
