using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Cell : FancyCell<ItemData, Context>
{
    [SerializeField] Animator animator = default;
    [SerializeField] Text message = default;
    [SerializeField] RawImage image = default;
    [SerializeField] Button button = default;

    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }

    public override void Initialize()
    {
        button.onClick.AddListener(Click);
    }

    void Click()
    {
        Context.OnCellClicked?.Invoke(Index);
        if (currentItemData != null && currentItemData.ClickAction != null)
        {
            currentItemData.ClickAction(currentItemData.MediaData);
            Debug.Log(1);
            return;
        }
    }

    ItemData currentItemData;
    public override void UpdateContent(ItemData itemData)
    {
        currentItemData = itemData;
        message.text = itemData.MediaData.mediaName;
        ResourceManager.Instance.GetTexture(itemData.MediaData.coverPath, (t) =>
        {
            image.texture = t;
        });

        var selected = Context.SelectedIndex == Index;
        image.color = selected
            ? new Color32(0, 255, 255, 100)
            : new Color32(255, 255, 255, 77);

    }

    public override void UpdatePosition(float position)
    {
        currentPosition = position;

        if (animator.isActiveAndEnabled)
        {
            animator.Play(AnimatorHash.Scroll, -1, position);
        }

        animator.speed = 0;
    }

    // GameObject が非アクティブになると Animator がリセットされてしまうため
    // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);
}