using UnityEngine;

public class CharacterFace : MonoBehaviour
{
    [Header("Makeup Slots")]
    public SpriteRenderer lipstick;
    public SpriteRenderer blush;
    public SpriteRenderer eyeshadow;

    [Header("Special Layers")]
    public GameObject acneLayer;

    public void ApplyMakeup(MakeupItemSO data)
    {
        switch (data.type)
        {
            case MakeupItemSO.MakeupType.Lipstick:
                lipstick.sprite = data.faceResultSprite;
                break;
            case MakeupItemSO.MakeupType.Blush:
                blush.sprite = data.faceResultSprite;
                break;
            case MakeupItemSO.MakeupType.Eyeshadow:
                eyeshadow.sprite = data.faceResultSprite;
                break;
            case MakeupItemSO.MakeupType.Cream:
                if (acneLayer != null)
                    acneLayer.SetActive(false);
                break;
        }
    }
}
