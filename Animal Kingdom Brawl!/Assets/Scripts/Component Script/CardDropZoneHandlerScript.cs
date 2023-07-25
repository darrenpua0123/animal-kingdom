using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void OnDrop(PointerEventData eventData)
    {
        CardMovementHandlerScript.CardIsDragging = false;

        int droppedCardIndex = CardMovementHandlerScript.DraggedCardIndex;

        if (gameScript.currentTurnPlayer != gameScript.startingPlayer) { return; }

        if (gameScript.currentTurnPlayer.actionPoint <= 0) 
        {
            Debug.Log("You have no action point left!");
            // TODO: Prompt No Action Point left text in gameScript.ShowNoActionPointLeft with animation floating up
            return;
        }

        AudioManagerScript.instance.Play(SoundName.PlayCard);

        gameScript.DestroyCardPlacholder(droppedCardIndex);
        Destroy(eventData.pointerDrag);

        gameScript.CardOnDrop(droppedCardIndex);
    }
}
