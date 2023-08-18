using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class AutoTextLayout : MonoBehaviour
{
    public Text text;
    public Vector2 expandSize;
    private RectTransform _rectTransform;
    private RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
    private int textCount = 0;
    public void Fit()
    {
        if (text == null)
        {
            Debug.LogWarning("AutoTextLayout: No text set.");
            return;
        }
        Vector2 textPreferSize = new Vector2(text.preferredWidth, text.preferredHeight);
        //Debug.Log(textPreferSize);
        rectTransform.sizeDelta = textPreferSize + expandSize;
    }
}
