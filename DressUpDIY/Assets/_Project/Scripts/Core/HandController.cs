using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HandController : MonoBehaviour
{
    public static HandController Instance;

    public enum HandState { Idle, MovingToTool, Dipping, Dragging, Applying, Returning }
    public HandState currentState = HandState.Idle;

    private Vector3 dragDelta;
    private bool isDragging = false;
    private bool isOverFace = false;
    private Vector3 initialPosition;

    private MakeupItemSO currentData;

    [Header("References")]
    public Image toolImage;
    public Image toolColorOverlay;
    public GameObject staticBookTool;
    public RectTransform tipAnchor;
    public RectTransform faceArea;
    public Transform chestPoint;
    public CharacterFace characterFace;

    [Header("Animation Settings")]
    public float flyToToolTime = 0.5f;    // Time to fly to the brush in the book
    public float flyToColorTime = 0.4f;   // Time to fly from the brush to the color palette
    public float shakeDuration = 0.6f;    // Duration of the "scrubbing" motion on the powder
    public float shakeStrength = 15f;     // Shake intensity (how far the hand moves)
    public int shakeVibrato = 12;         // Shake frequency (vibrations per second)
    public float flyToChestTime = 0.5f;   // Time to move the hand back to the "idle" chest position


    void Awake() => Instance = this;


    void Start()
    {
        initialPosition = transform.position;
        toolImage.enabled = false;
    }


    void Update()
    {
        if (currentState == HandState.Dragging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                dragDelta = transform.position - Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                if (isOverFace) StartApplying();
                else ReturnToChest();
            }

            if (isDragging)
            {
                transform.position = Input.mousePosition + dragDelta;
                isOverFace = RectTransformUtility.RectangleContainsScreenPoint(faceArea, tipAnchor.position);
            }
        }
    }


    public void StartSequence(MakeupItemSO data, GameObject clickedButton)
    {
        if (staticBookTool != null) staticBookTool.SetActive(true);

        DOTween.KillAll();
        currentData = data;

        if (staticBookTool == null) staticBookTool = clickedButton;

        toolImage.sprite = data.cleanToolSprite;
        toolImage.SetNativeSize();
        toolImage.enabled = false;
        toolColorOverlay.color = new Color(1, 1, 1, 0);

        RunAnimationSequence(clickedButton.transform.position);
    }


    public void RunAnimationSequence(Vector3 colorPosition)
    {
        Vector3 tipOffset = transform.position - tipAnchor.position;
        currentState = HandState.MovingToTool;

        Sequence s = DOTween.Sequence();

        AddPickupStep(s);

        if (!currentData.skipDipping)
        {
            AddDippingStep(s, colorPosition, tipOffset);
        }

        AddMoveToChestStep(s);
    }


    private void AddPickupStep(Sequence s)
    {
        s.Append(transform.DOMove(staticBookTool.transform.position, flyToToolTime).SetEase(Ease.OutQuad));
        s.AppendCallback(() =>
        {
            if (currentData.skipDipping)
            {
                RectTransform bookRect = staticBookTool.GetComponent<RectTransform>();
                if (bookRect != null)
                {
                    toolImage.rectTransform.sizeDelta = bookRect.sizeDelta;
                }

                toolImage.sprite = currentData.toolTipSprite;
            }
            else
            {
                toolImage.SetNativeSize();
            }

            staticBookTool.SetActive(false);
            toolImage.enabled = true;
        });
    }


    private void AddDippingStep(Sequence s, Vector3 colorPos, Vector3 offset)
    {
        s.Append(transform.DOMove(colorPos + offset, flyToColorTime).SetEase(Ease.OutQuad));
        s.AppendCallback(() =>
        {
            toolColorOverlay.sprite = currentData.toolTipSprite;
            toolColorOverlay.SetNativeSize();
            toolColorOverlay.color = new Color(1, 1, 1, 0);
        });
        s.Append(transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, randomness: 0, fadeOut: true));
        s.Join(toolColorOverlay.DOFade(1f, shakeDuration).SetEase(Ease.Linear));
        s.AppendCallback(() =>
        {
            toolImage.sprite = currentData.toolTipSprite;
            toolImage.SetNativeSize();
        });
    }


    private void AddMoveToChestStep(Sequence s)
    {
        s.Append(transform.DOMove(chestPoint.position, flyToChestTime).SetEase(Ease.OutCubic));
        s.OnComplete(() => currentState = HandState.Dragging);
    }


    private void StartApplying()
    {
        currentState = HandState.Applying;

        transform.DOShakePosition(1f, 20.0f, 8).OnComplete(() =>
        {
            characterFace.ApplyMakeup(currentData);
            FinishAndHide();
        });
    }


    private void ReturnToChest()
    {
        currentState = HandState.Returning;
        Vector3 tipOffset = transform.position - tipAnchor.position;

        transform.DOMove(chestPoint.position, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            currentState = HandState.Dragging;
        });
    }


    private void FinishAndHide()
    {
        currentState = HandState.Returning;

        Sequence s = DOTween.Sequence();

        s.Append(transform.DOMove(staticBookTool.transform.position, flyToToolTime).SetEase(Ease.OutQuad));

        s.AppendCallback(() =>
        {
            staticBookTool.SetActive(true);
            toolImage.enabled = false;
            toolColorOverlay.color = new Color(1, 1, 1, 0);
        });

        s.AppendInterval(0.15f);

        s.Append(transform.DOMove(initialPosition, flyToToolTime).SetEase(Ease.OutQuad));

        s.OnComplete(() =>
        {
            currentState = HandState.Idle;
        });
    }
}
