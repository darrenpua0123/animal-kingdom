using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovementHandlerScript : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public static bool CardIsDragging = false;
    [HideInInspector] public static int DraggedCardIndex;
    private GameScript gameScript;

    void Awake() 
    {
        gameScript = FindObjectOfType<GameScript>();

        if (gameScript == null) 
        {
            Debug.Log($"Error: GameScript is null in {this}");
            return;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !CardIsDragging)
        {
            int cardIndex = transform.GetSiblingIndex();
            gameScript.ShowViewCardPanel(cardIndex);   
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CardIsDragging = true;
        GetComponent<Image>().raycastTarget = false;

        DraggedCardIndex = transform.GetSiblingIndex();
        gameScript.CreateCardPlaceholder(DraggedCardIndex);

        this.transform.SetParent(gameScript.canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector3 mousePos = Input.mousePosition;
            float worldSpaceX = Camera.main.ScreenToWorldPoint(mousePos).x;
            float worldSpaceY = Camera.main.ScreenToWorldPoint(mousePos).y;
            Vector2 mousePosInWorldSpace = new Vector2(worldSpaceX, worldSpaceY);

            this.transform.position = mousePosInWorldSpace;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    { 
        CardIsDragging = false;
        GetComponent<Image>().raycastTarget = true;

        gameScript.DestroyCardPlacholder(DraggedCardIndex);

        this.transform.SetParent(gameScript.playerHandPanel.transform);
        this.transform.SetSiblingIndex(DraggedCardIndex);
    }
}
