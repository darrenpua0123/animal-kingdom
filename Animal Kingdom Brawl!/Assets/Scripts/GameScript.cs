using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    // TODO: Remove
    public Beedle bee = new Beedle();
    int health;
    int shield;
    // REMOVE ^

    [Header("Firebase")]
    public FirebaseAuth auth;
    public DatabaseReference firebaseDBReference;

    [Header("Player's Attribute")]
    private string userID;

    [Header("Game UI")]
    public TMP_Text remainingCardText;
    public GameObject remainingCardImage; 

    [Header("Chracter Life Hub")]
    public LifeHUB playerLifeHUB;
    public LifeHUB hornterrorLifeHUB;
    public LifeHUB enemyOneLifeHUB;
    public LifeHUB enemyTwoLifeHUB;
    public LifeHUB enemyThreeLifeHUB;

    [Header("Player GameObjects")]
    public GameObject cardHolder;
    public GameObject cardPrefab;
    // TODO: Game Area

    void Awake()
    {
        InitialiseFirebase();

        userID = auth.CurrentUser.UserId;
    }

    void Start()
    {

        // TODO: Complete CardRemainingUI Update
        // As well as try to do CardHolde, refer to youtube is whether gameobject or canva.

        health = bee.defaultHealth;
        shield = 3;

        playerLifeHUB.SetMaxHealth(health);
        playerLifeHUB.SetShield(shield);

        DrawCard(1);
        //CenterPlayableCard(); 
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

        // DEV: Remove;
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            health -= 2;
            shield--;

            playerLifeHUB.SetHealthBar(health);
            playerLifeHUB.SetShield(shield);

        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            health++;
            shield++;

            playerLifeHUB.SetHealthBar(health);
            playerLifeHUB.SetShield(shield);
        }
        // REMOVE ^
    }

    private void InitialiseFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        firebaseDBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void PlayerEndTurn() 
    {
        DrawCard(3);
        //CenterPlayableCard();
    }

    public void DrawCard(int numberOfDraw) 
    {
        // TODO: Optimize this
        for (int i = 0; i < numberOfDraw; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardHolder.transform);
        }   
    }

    //private void CenterPlayableCard()
    //{
    //    // TODO: need check, maybe need to change
    //    int childCount = cardHolder.transform.childCount;

    //    if (childCount == 0)
    //        return;

    //    float totalWidth = 0f;
    //    float totalHeight = 0f;

    //    // Calculate the total width and height of all child cards
    //    for (int i = 0; i < childCount; i++)
    //    {
    //        Transform child = cardHolder.transform.GetChild(i);
    //        Renderer childRenderer = child.GetComponent<Renderer>();
    //        totalWidth += childRenderer.bounds.size.x;
    //        totalHeight = Mathf.Max(totalHeight, childRenderer.bounds.size.y);
    //    }

    //    // Calculate the center position of the cardHolder
    //    Vector3 centerPos = cardHolder.transform.position;

    //    // Calculate the starting position for the first card
    //    float startX = centerPos.x - totalWidth / 2f;
    //    float startY = centerPos.y + totalHeight / 2f;

    //    // Position each child card relative to the starting position
    //    for (int i = 0; i < childCount; i++)
    //    {
    //        Transform child = cardHolder.transform.GetChild(i);
    //        Renderer childRenderer = child.GetComponent<Renderer>();

    //        float cardWidth = childRenderer.bounds.size.x;
    //        float cardHeight = childRenderer.bounds.size.y;

    //        float xPos = startX + (cardWidth / 2f);
    //        float yPos = startY - (cardHeight / 2f);

    //        Vector3 newPos = new Vector3(xPos, yPos, 0);
    //        child.position = newPos;

    //        startX += cardWidth;
    //    }
    //}
}
