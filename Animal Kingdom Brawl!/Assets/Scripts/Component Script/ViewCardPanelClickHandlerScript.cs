using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViewCardPanelClickHandlerScript : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameScript gameScript = FindObjectOfType<GameScript>();

            if (gameScript != null)
            {
                gameScript.CloseViewCardPanel();
            }
        }
    }
}
