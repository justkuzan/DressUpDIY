using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HandController : MonoBehaviour
{
    public static HandController Instance;

    public enum HandState { Idle, MovingToTool, Dipping, Dragging, Applying, Returning }
    public HandState currentState = HandState.Idle;

    [Header("References")]
    public Image toolImage;
    public GameObject staticBookTool;
    public Transform chestPoint;

    [Header("Settings")]
    public float moveSpeed = 0.5f;

    private RectTransform rectTransform;
    private MakeupItemSO currentData;

    void Awake() => Instance = this;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        toolImage.enabled = false;
    }

    public void StartSequence(MakeupItemSO data, Vector3 colorPosition)
    {
        DOTween.KillAll();

        currentData = data;

        toolImage.sprite = data.cleanToolSprite;
        toolImage.SetNativeSize();
        toolImage.enabled = false;

        RunAnimationSequence(colorPosition);
    }

    public void RunAnimationSequence(Vector3 colorPosition)
    {
        currentState = HandState.MovingToTool;

        Sequence s = DOTween.Sequence();
        s.Append(transform.DOMove(staticBookTool.transform.position, 0.5f).SetEase(Ease.OutQuad));

        s.AppendCallback(() =>
        {
            staticBookTool.SetActive(false);
            toolImage.enabled = true;
            Debug.Log("Кисть захвачена!");
        });

        s.Append(transform.DOMove(colorPosition, 0.4f).SetEase(Ease.OutQuad));

        s.Append(transform.DOShakePosition(0.6f, strength: 15f, vibrato: 12));

        s.AppendCallback(() =>
        {
            toolImage.sprite = currentData.toolTipSprite;
            toolImage.SetNativeSize();
        });

        s.Append(transform.DOMove(chestPoint.position, 0.5f).SetEase(Ease.OutBack));

        s.OnComplete(() =>
        {
            currentState = HandState.Dragging;
            Debug.Log("Управление передано игроку!");
        });

    }
}
