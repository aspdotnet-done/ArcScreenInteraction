using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class SelectHandleOverride : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private bool autoSelect = false;
    [SerializeField] private Button button;
    [SerializeField] private bool isScroll = false;
    public void OnDeselect(BaseEventData eventData)
    {

        transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (autoSelect && button != null)
        {
            Debug.Log("OnSelect");
            ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
        if (isScroll)
        {


            transform.parent.parent.parent.GetComponent<ScrollView>().SelectCell(transform.parent.GetSiblingIndex());
        }
        transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
    }

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
