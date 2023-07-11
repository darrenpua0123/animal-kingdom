using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RevealCardsPanelClickHandlerScript : MonoBehaviour, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
        {
            gameScript.CloseRevealCardsPanel();
        }
    }

}
