using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    // TODO: Remove
    public Beedle bee = new Beedle();
    int health;
    int shield;
    int cardRemain;
    // REMOVE ^

    [Header("Setting Variables")]
    private float buttonImageAlphaThreshold = 0.1f;

    [Header("Firebase")]
    public FirebaseAuth auth;
    public DatabaseReference firebaseDBReference;

    [Header("Player's Attribute")]
    private string userID;

    [Header("Game UI")]
    public TMP_Text remainingCardText;
    public Image remainingCardImage;
    public TMP_Text actionPointLeftText;
    public Button endTurnButton;

    [Header("Chracter Life Hub")]
    public LifeHUB playerLifeHUB;
    public LifeHUB hornterrorLifeHUB;
    public LifeHUB enemyOneLifeHUB;
    public LifeHUB enemyTwoLifeHUB;
    public LifeHUB enemyThreeLifeHUB;

    [Header("Player GameObjects")]
    public GameObject playerCardPrefab;//test
    public GameObject playerHand;

    [Header("Game Variables")]
    private CardDeck chestCardDeck;//test


    void Awake()
    {
        InitialiseFirebase();

        userID = auth.CurrentUser.UserId;

    }

    void Start()
    {
        chestCardDeck = CardDBSchema.defaultChestCardDeck;//test
        // Make button clickable on full precision of the button image
        endTurnButton.image.alphaHitTestMinimumThreshold = buttonImageAlphaThreshold;

        // Instantiate Player.cs Class, for the Player based on choice, and other based on random
        // Player player = new Player()...

        health = bee.defaultHealth;
        shield = 3;
        cardRemain = 5;

        playerLifeHUB.SetMaxHealth(health);
        playerLifeHUB.SetShield(shield);
        UpdateActionPointText(cardRemain);
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

            cardRemain--;
            UpdateActionPointText(cardRemain);
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

    public void PlayerEndTurnButton() 
    {
        // Check for AP
        // Draw 2 cards from chest, put to player's hand
        // Update 
        //DrawCard(3);

        if (ActionNotEndable())
        {
            Debug.Log($"Cannot end turn as you have {cardRemain} turns left!");
            //return;
        }
        else
        {
            cardRemain--;
            UpdateActionPointText(cardRemain);
            DrawCard(2);
        }
    }

    public void DrawCard(int numberOfDraw) 
    {
        // TODO: Optimize this
        for (int i = 0; i < numberOfDraw; i++)
        {
            //yield return new WaitForSeconds(1); // Draw card second by second
            GameObject newCard = Instantiate(playerCardPrefab, playerHand.transform);
            newCard.GetComponent<Image>().sprite = chestCardDeck.GetDrawableCards()[i].CardFrontSprite;
        }
    }

    // public IEnumerator DrawCardBySecond(int numberOfDraw, float seconds) 
    // yield new return new WaitForSeconds(second)

    private bool ActionNotEndable()
    {
        return (cardRemain > 0);
    }

    private void UpdateActionPointText(int actionPoint) 
    {
        if (actionPoint >= 0) {
            actionPointLeftText.text = actionPoint.ToString();
        }
    }
}
