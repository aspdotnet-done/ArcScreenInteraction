using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Cell : FancyCell<ItemData, Context>
{
    [SerializeField] Animator animator = default;
    [SerializeField] Text message = default;
    [SerializeField] RawImage image = default;
    [SerializeField] Button button = default;
    [SerializeField] Image pdfIcon = default;
    [SerializeField] Image videoIcon = default;
    [SerializeField] Image pictureIcon = default;

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
            currentItemData.ClickAction(currentItemData.Message);
            Debug.Log(1);
            return;
        }
    }

    ItemData currentItemData;
    public override void UpdateContent(ItemData itemData)
    {
        currentItemData = itemData;
        message.text = itemData.Message;
        //UpdateIcon(itemData.MediaData.mediaType);
        string logoPath = AssetUtility.GetDetailDataFolder(itemData.Message) + "icon.jpg";

        //判断文件路径是否存在
        if (File.Exists(logoPath))
        {
            ResourceManager.Instance.GetTexture(logoPath, (t) =>
            {
                image.texture = t;
            });
        }
        else
        {
            logoPath = AssetUtility.GetDetailDataFolder(itemData.Message) + "icon.png";
            if (File.Exists(logoPath))
            {
                ResourceManager.Instance.GetTexture(logoPath, (t) =>
                {
                    image.texture = t;
                });
            }
        }


        var selected = Context.SelectedIndex == Index;
        // image.color = selected
        //     ? new Color32(0, 255, 255, 100)
        //     : new Color32(255, 255, 255, 77);

    }

    // private void UpdateIcon(MediaType mediaType)
    // {
    //     pdfIcon.gameObject.SetActive(false);
    //     videoIcon.gameObject.SetActive(false);
    //     pictureIcon.gameObject.SetActive(false);
    //     switch (mediaType)
    //     {
    //         case MediaType.pdf:
    //             pdfIcon.gameObject.SetActive(true);
    //             break;
    //         case MediaType.video:
    //             videoIcon.gameObject.SetActive(true);
    //             break;
    //         case MediaType.picture:
    //             pictureIcon.gameObject.SetActive(true);
    //             break;
    //         default:
    //             break;
    //     }
    // }

    public override void UpdatePosition(float position)
    {
        currentPosition = position;

        if (animator.isActiveAndEnabled)
        {
            animator.Play(AnimatorHash.Scroll, -1, position);
        }

        animator.speed = 0;
    }


    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);
}