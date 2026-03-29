using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HandController : MonoBehaviour
{
    public static HandController Instance;

    public enum HandState { Idle, MovingToTool, Dipping, Dragging, Applying, Returning }
    public HandState currentState = HandState.Idle;

    public Vector3 dragOffset = new Vector3(0, 150, 0);
    private bool isDragging = false;

    [Header("References")]
    public Image toolImage;
    public Image toolColorOverlay;
    public GameObject staticBookTool;
    public Transform tipAnchor;
    public Transform chestPoint;

    [Header("Animation Settings")]
    public float flyToToolTime = 0.5f;    // Time to fly to the brush in the book
    public float flyToColorTime = 0.4f;   // Time to fly from the brush to the color palette
    public float shakeDuration = 0.6f;    // Duration of the "scrubbing" motion on the powder
    public float shakeStrength = 15f;     // Shake intensity (how far the hand moves)
    public int shakeVibrato = 12;         // Shake frequency (vibrations per second)
    public float flyToChestTime = 0.5f;   // Time to move the hand back to the "idle" chest position

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
        Vector3 tipOffset = transform.position - tipAnchor.position;

        currentState = HandState.MovingToTool;

        Sequence s = DOTween.Sequence();

        s.Append(transform.DOMove(staticBookTool.transform.position, flyToToolTime).SetEase(Ease.OutQuad));

        s.AppendCallback(() =>
        {
            staticBookTool.SetActive(false);
            toolImage.enabled = true;
        });

        s.Append(transform.DOMove(colorPosition + tipOffset, flyToColorTime).SetEase(Ease.OutQuad));

        s.AppendCallback(() =>
        {
            toolColorOverlay.sprite = currentData.toolTipSprite;
            toolColorOverlay.SetNativeSize();
            toolColorOverlay.color = new Color(1, 1, 1, 0);
        });

        s.Append(transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, randomness: 0, snapping: false, fadeOut: true));
        s.Join(toolColorOverlay.DOFade(1f, shakeDuration).SetEase(Ease.Linear));

        s.AppendCallback(() =>
        {
            toolImage.sprite = currentData.toolTipSprite;
            toolImage.SetNativeSize();
            currentState = HandState.Dipping;
        });

        s.Append(transform.DOMove(chestPoint.position, flyToChestTime).SetEase(Ease.OutCubic));

        s.OnComplete(() =>
        {
            currentState = HandState.Dragging;
        });
    }

    void Update()
    {
        Debug.Log(currentState);
        if (currentState == HandState.Dragging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                // Тут мы позже проверим: над лицом мы или нет
            }

            if (isDragging)
            {
                transform.position = Input.mousePosition + dragOffset;
            }
        }

    }
}
