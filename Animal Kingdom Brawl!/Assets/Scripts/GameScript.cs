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

    [Header("Variables")]
    private float buttonImageAlphaThreshold = 0.1f;

    [Header("Firebase")]
    public FirebaseAuth auth;
    public DatabaseReference firebaseDBReference;

    [Header("Player's Attribute")]
    private string userID;

    [Header("Game UI")]
    public TMP_Text remainingCardText;
    public GameObject remainingCardImage;
    public TMP_Text actionPointLeftText;
    public Button endTurnButton;

    [Header("Chracter Life Hub")]
    public LifeHUB playerLifeHUB;
    public LifeHUB hornterrorLifeHUB;
    public LifeHUB enemyOneLifeHUB;
    public LifeHUB enemyTwoLifeHUB;
    public LifeHUB enemyThreeLifeHUB;

    [Header("Player GameObjects")]
    public GameObject playerHand;
    //public GameObject cardPrefab;
    // TODO: Game Area

    void Awake()
    {
        InitialiseFirebase();

        userID = auth.CurrentUser.UserId;
    }

    void Start()
    {
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

        DrawCard(1);
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
        }
    }

    public void DrawCard(int numberOfDraw) 
    {
        // TODO: Optimize this
        for (int i = 0; i < numberOfDraw; i++)
        {
            //GameObject newCard = Instantiate(cardPrefab, playerHand.transform);
        }
    }
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
