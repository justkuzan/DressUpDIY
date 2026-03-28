using UnityEngine;

[CreateAssetMenu(fileName = "NewMakeupItem", menuName = "Makeup/Create Item")]
public class MakeupItemSO : ScriptableObject
{
    public enum MakeupType { Cream, Blush, Eyeshadow, Lipstick }
    public MakeupType type;
    public Sprite toolTipSprite;
    public Sprite faceResultSprite;
}
