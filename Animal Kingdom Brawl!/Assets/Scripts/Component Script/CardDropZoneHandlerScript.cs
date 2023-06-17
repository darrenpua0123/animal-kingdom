using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropZoneHandlerScript : MonoBehaviour, IDropHandler
{   
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        CardMovementHandlerScript.CardIsDragging = false;

        int droppedCardIndex = CardMovementHandlerScript.DraggedCardIndex;

        gameScript.DestroyCardPlacholder(droppedCardIndex);
        Destroy(eventData.pointerDrag);

        gameScript.ActivateCard(droppedCardIndex);// TODO: Continue here
    }
}
