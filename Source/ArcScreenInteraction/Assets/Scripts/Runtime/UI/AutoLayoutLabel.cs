using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoLayoutLabel : MonoBehaviour
{
    [SerializeField]
    private Text titleText = null;
    public Vector2 expandSize = new Vector2(0, 0);
    public void SetText(string text)
    {
        titleText.text = text;
        StartCoroutine(SetLayoutRoutine());
    }
    [ContextMenu("ResetLayout")]
    public void ResetLayout()
    {
        StartCoroutine(SetLayoutRoutine());
    }
    private IEnumerator SetLayoutRoutine()
    {
        yield return new WaitForEndOfFrame();
        var rectTransform = GetComponent<RectTransform>();
        var sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = titleText.preferredHeight;
        sizeDelta.x = titleText.preferredWidth;
        rectTransform.sizeDelta = sizeDelta + expandSize;
    }

}
