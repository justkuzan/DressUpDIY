using UnityEngine;
using DG.Tweening;

public class CharacterFace : MonoBehaviour
{
    [Header("Makeup Slots")]
    public SpriteRenderer lipstick;
    public SpriteRenderer blush;
    public SpriteRenderer eyeshadow;

    [Header("Special Layers")]
    public GameObject acneLayer;


    void Start()
    {
        ResetMakeup();
        if (acneLayer != null) acneLayer.SetActive(true);
    }

    public void ApplyMakeup(MakeupItemSO data)
    {
        SpriteRenderer currentRenderer = null;

        switch (data.type)
        {
            case MakeupItemSO.MakeupType.Lipstick:
                currentRenderer = lipstick;
                break;
            case MakeupItemSO.MakeupType.Blush:
                currentRenderer = blush;
                break;
            case MakeupItemSO.MakeupType.Eyeshadow:
                currentRenderer = eyeshadow;
                break;
            case MakeupItemSO.MakeupType.Cream:
                if (acneLayer != null) acneLayer.SetActive(false);
                break;
        }

        if (currentRenderer != null)
        {
            currentRenderer.sprite = data.faceResultSprite;
            currentRenderer.color = new Color(1, 1, 1, 0);
            currentRenderer.DOFade(1f, 0.5f);
        }
    }


    public void CleanAll()
    {
        if (acneLayer != null) acneLayer.SetActive(true);

        lipstick.sprite = null;
        blush.sprite = null;
        eyeshadow.sprite = null;
    }

    public void ResetMakeup()
    {
        if (lipstick != null) { lipstick.sprite = null; lipstick.color = new Color(1, 1, 1, 0); }
        if (blush != null) { blush.sprite = null; blush.color = new Color(1, 1, 1, 0); }
        if (eyeshadow != null) { eyeshadow.sprite = null; eyeshadow.color = new Color(1, 1, 1, 0); }
    }

}
