using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovementHandlerScript : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameScript gameScript;
    private int draggedCardIndex;
    private bool isDragging = false;

    void Awake() 
    {
        gameScript = FindObjectOfType<GameScript>();

        if (gameScript == null) 
        {
            Debug.Log($"Error: GameScript is null in {this}");
            return;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !isDragging)
        {
            int cardIndex = transform.GetSiblingIndex();
            gameScript.ShowViewCardPanel(cardIndex);   
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        GetComponent<Image>().raycastTarget = false;

        draggedCardIndex = transform.GetSiblingIndex();
        gameScript.CreateCardPlaceholder(draggedCardIndex);

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
        isDragging = false;
        GetComponent<Image>().raycastTarget = true;

        gameScript.DestroyCardPlacholder(draggedCardIndex);

        this.transform.SetParent(gameScript.playerHandPanel.transform);
        this.transform.SetSiblingIndex(draggedCardIndex);

        draggedCardIndex = -1;
    }

    // TODO: When Drag to middle collision panel, then activate things.
}
