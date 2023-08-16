using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SelectHandleOverride : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public void OnDeselect(BaseEventData eventData)
    {
        transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
