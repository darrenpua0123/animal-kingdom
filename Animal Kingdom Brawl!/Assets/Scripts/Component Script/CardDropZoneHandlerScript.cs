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

        if (gameScript.currentTurnPlayer != gameScript.startingPlayer) 
        {
            Sound actionNotEndableSound = AudioManagerScript.instance.GetSound(SoundName.ActionNotEndable);

            if (!actionNotEndableSound.isPlaying)
            {
                AudioManagerScript.instance.Play(SoundName.ActionNotEndable);
            }

            gameScript.ShowPopupText("It is not your turn yet!");
            return; 
        }

        if (gameScript.currentTurnPlayer.actionPoint <= 0) 
        {
            Sound actionNotEndableSound = AudioManagerScript.instance.GetSound(SoundName.ActionNotEndable);

            if (!actionNotEndableSound.isPlaying)
            {
                AudioManagerScript.instance.Play(SoundName.ActionNotEndable);
            }

            gameScript.ShowPopupText("You have no action point left!");
            return;
        }

        AudioManagerScript.instance.Play(SoundName.PlayCard);

        gameScript.DestroyCardPlacholder(droppedCardIndex);
        Destroy(eventData.pointerDrag);

        gameScript.CardOnDrop(droppedCardIndex);
    }
}
