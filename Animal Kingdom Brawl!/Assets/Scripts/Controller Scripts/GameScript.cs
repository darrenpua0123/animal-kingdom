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

    public GameObject viewCardPanel;
    public Image viewCardImage;

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
    private CardDeck chestCardDeck;
    private CardDeck hornterrorCardDeck;
    // private Player player = new Player...
    // private Player hornterrorPlayer = new Player...
    // private Player player; (at Start, only declare new Player randomly for computers)


    void Awake()
    {
        InitialiseFirebase();

        userID = auth.CurrentUser.UserId;

    }

    void Start()
    {
        // Make button clickable on full precision of the button image
        endTurnButton.image.alphaHitTestMinimumThreshold = buttonImageAlphaThreshold;
        
        chestCardDeck = CardDBSchema.defaultChestCardDeck;
        hornterrorCardDeck = CardDBSchema.hornterrorDefaultCardDeck;

        // Instantiate Player.cs Class, for the Player based on choice, and other based on random
        // Player player = new Player()...

        // REMOVE
        health = bee.defaultHealth;
        shield = 3;
        cardRemain = 5;

        playerLifeHUB.SetMaxHealth(health);
        playerLifeHUB.SetShield(shield);
        UpdateActionPointText(cardRemain);
        // REMOVE
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (viewCardPanel.activeSelf)
            {
                CloseViewCardPanel();
            }
        }
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
            // TODO: playerHand
            // playerHand.Add(cardDeck.DrawCard(2) <- List)
        }
    }

    public void DrawCard(int numberOfDraw) 
    {
        // TODO: Need to change this playerHand.Add(cardDeck.DrawCard) instead
        // IF YOU SEE THIS, IT MEANS DO THIS FIRTST IMPORTANT!!!
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

    public void ShowViewCardPanel(int cardIndex) 
    {
        // TODO: Change to get from PlayerHand's List instead.
        // Card card = playerHand[cardIndex];
        Card card = chestCardDeck.GetDrawableCards()[cardIndex];
        
        viewCardImage.sprite = card.CardFrontSprite;
        viewCardPanel.SetActive(true);
    }

    public void CloseViewCardPanel() 
    {
        viewCardImage.sprite = null;
        viewCardPanel.SetActive(false);
    }
}
