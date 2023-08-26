using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoFit : MonoBehaviour
{
    private RectTransform _currentRectTransform = null;
    private RectTransform currentRectTransform
    {
        get
        {
            if (_currentRectTransform == null) _currentRectTransform = GetComponent<RectTransform>();
            return _currentRectTransform;
        }
    }
    [SerializeField] private Text contentText;
    private RectTransform _textRectTransform = null;
    private RectTransform textRectTransform
    {
        get
        {
            if (_textRectTransform == null) _textRectTransform = contentText.GetComponent<RectTransform>();
            return _textRectTransform;
        }
    }
    [Range(0, 512)]
    public float expandLeft;
    [Range(0, 512)]
    public float expandRight;
    private int lastTextCount;
    private bool isTextCountChanged => contentText.text.Length != lastTextCount;
    public float contentSize;
    public RawImage rawImage;

    public void SetContent(string content)
    {
        contentText.text = content;
        FitSize();
    }
    private void SetTextCount() => lastTextCount = contentText.text.Length;
    private void ClearContent()
    {
        contentText.text = "";
        SetTextCount();
        FitSize();
    }

    [ContextMenu("SetText")]
    public void TestSet()
    {
        SetContent("1242342423432432");
    }
    private void FitSize()
    {
        float textPreferredWidth = contentText.preferredWidth;
        var rectSize = currentRectTransform.sizeDelta;
        contentSize = expandLeft + textPreferredWidth + expandRight;
        currentRectTransform.sizeDelta = new Vector2(contentSize, rectSize.y);
        textRectTransform.anchoredPosition = new Vector2(expandLeft * 0.5f - expandRight * 0.5f, 0);
    }
}
