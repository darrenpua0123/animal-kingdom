using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViewCardPanelClickHandlerScript : MonoBehaviour, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            gameScript.CloseViewCardPanel();
        }
    }
}
