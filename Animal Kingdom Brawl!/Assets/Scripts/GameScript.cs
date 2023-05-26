using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject cardHolder;
    public GameObject cardPrefab;
    public float cardSpacing = 0.5f;
    // TODO: Game Area

    // Start is called before the first frame update
    void Start()
    {
        DrawCard(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerEndTurn() 
    {
        DrawCard(3);

        //CenterPlayableCard();
        CenterCards();
    }

    public void DrawCard(int numberOfDraw) 
    {
        for (int i = 0; i < numberOfDraw; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardHolder.transform);
            newCard.transform.localPosition = Vector3.zero;
            newCard.transform.localRotation = Quaternion.identity;
        }   
    }

    private void CenterCards()
    {
        int cardCount = cardHolder.transform.childCount;
        float totalWidth = (cardCount - 1) * cardSpacing;
        float offsetX = totalWidth / 2f;
        float offsetY = 0f;

        if (cardCount > 1)
        {
            offsetY = ((cardCount - 1) * cardSpacing) / 2f;
        }

        Vector3 startingPosition = new Vector3(-offsetX, offsetY, -1f);

        for (int i = 0; i < cardCount; i++)
        {
            Transform cardTransform = cardHolder.transform.GetChild(i);
            Vector3 newPosition = startingPosition + new Vector3(i * cardSpacing, -i * cardSpacing, 0f);
            cardTransform.localPosition = newPosition;
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

            Vector3 newPos = new Vector3(xPos, yPos, -1);
            child.position = newPos;

            startX += cardWidth;
        }
    }
}
