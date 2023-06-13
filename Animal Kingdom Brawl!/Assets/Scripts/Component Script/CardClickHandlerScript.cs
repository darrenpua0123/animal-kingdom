using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickHandlerScript : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameScript gameScript = FindObjectOfType<GameScript>();

            if (gameScript != null)
            {
                int cardIndex = transform.GetSiblingIndex();
                gameScript.ShowViewCardPanel(cardIndex);
            }
        }
    }

    // TODO: Add on Drag, when Drag to middle collision panel, then activate things
}
