using UnityEngine;
using UnityEngine.UI;

public class MakeupButton : MonoBehaviour
{
    public MakeupItemSO itemData;
    private Image myImage;


    void Start()
    {
        myImage = GetComponent<Image>();

        if (itemData != null && itemData.spriteIcon != null)
        {
            myImage.sprite = itemData.spriteIcon;
        }
    }


    public void HandleClick()
    {
        if (itemData == null) return;
        HandController.Instance.StartSequence(itemData, transform.position);
    }
}
