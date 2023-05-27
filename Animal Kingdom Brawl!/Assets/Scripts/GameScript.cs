using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject cardHolder;
    public GameObject cardPrefab;
    // TODO: Game Area

    // Start is called before the first frame update
    void Start()
    {
        DrawCard(1);
        CenterPlayableCard();
    }

    // Update is called once per frame
    void Update()
    {
        // Left click: Can drag card to MIDDLE to play it.
        // If card is CanTargetPlayers (mostly Attack/Mixed/Ability cards has)
        // Then prompt Target Selector
        // Else, target self
        
        // Right click: Stop all action, make that card in the middle to read the description
        // left click anywhere to closed it.
    }

    public void PlayerEndTurn() 
    {
        DrawCard(3);
        CenterPlayableCard();
    }

    public void DrawCard(int numberOfDraw) 
    {
        // TODO: Optimize this
        for (int i = 0; i < numberOfDraw; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardHolder.transform);
        }   
    }

    private void CenterPlayableCard()
    {
        int childCount = cardHolder.transform.childCount;

        if (childCount == 0)
            return;

        float totalWidth = 0f;
        float totalHeight = 0f;

        // Calculate the total width and height of all child cards
        for (int i = 0; i < childCount; i++)
        {
            Transform child = cardHolder.transform.GetChild(i);
            Renderer childRenderer = child.GetComponent<Renderer>();
            totalWidth += childRenderer.bounds.size.x;
            totalHeight = Mathf.Max(totalHeight, childRenderer.bounds.size.y);
        }

        // Calculate the center position of the cardHolder
        Vector3 centerPos = cardHolder.transform.position;

        // Calculate the starting position for the first card
        float startX = centerPos.x - totalWidth / 2f;
        float startY = centerPos.y + totalHeight / 2f;

        // Position each child card relative to the starting position
        for (int i = 0; i < childCount; i++)
        {
            Transform child = cardHolder.transform.GetChild(i);
            Renderer childRenderer = child.GetComponent<Renderer>();

            float cardWidth = childRenderer.bounds.size.x;
            float cardHeight = childRenderer.bounds.size.y;

            float xPos = startX + (cardWidth / 2f);
            float yPos = startY - (cardHeight / 2f);

            Vector3 newPos = new Vector3(xPos, yPos, 0);
            child.position = newPos;

            startX += cardWidth;
        }
    }
}
