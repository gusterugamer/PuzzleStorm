using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static event UnityAction<PointerEventData> onPointerDown;
    public static event UnityAction<PointerEventData> onDrag;
    public static event UnityAction<PointerEventData> onPointerUp;
    public static event UnityAction<PuzzlePiece> onPieceSelected;

    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown?.Invoke(eventData);
        ProcessTouch(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke(eventData);
        ProcessTouch(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(eventData);
        ProcessTouch(eventData);
    }

    private void ProcessTouch(PointerEventData eventData)
    {
        Vector3 rayPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero);

        if (hit.collider != null)
        {
            PuzzlePiece pp = hit.transform.GetComponent<PuzzlePiece>();
           // Assert.IsFalse(pp != null, "No puzzle piece selected!");
            onPieceSelected?.Invoke(pp);
        }
    }
}