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
    public Catomic catomic = new Catomic();
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
    public GameObject playerCardPrefab;
    public GameObject playerHandPanel;

    [Header("Game Variables")]
    private CardDeck chestCardDeck;
    private CardDeck hornterrorCardDeck;
    private Player player;
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

        #region Game CardDeck
        chestCardDeck = CardDBSchema.defaultChestCardDeck;
        chestCardDeck.ShuffleCards();

        hornterrorCardDeck = CardDBSchema.hornterrorDefaultCardDeck;
        hornterrorCardDeck.ShuffleCards();
        #endregion

        #region Player
        // TODO: Need to get AnimalHero from Choice in ChooseHeroScene

        player = new Player(catomic, CardDBSchema.catomicDefaultCardDeck);
        player.cardDeck.ShuffleCards();
        playerLifeHUB.SetMaxHealth(player.health);
        playerLifeHUB.SetShield(player.shield);
        UpdateActionPointText(player.actionPoint);
        #endregion

        UpdateRemainingCardText(player.cardDeck.GetAllCards().Count);
    }

    void Update()
    {
        // Left click: Can drag card to MIDDLE to play it.
        // If card is CanTargetPlayers (mostly Attack/Mixed/Ability cards has)
        // Then prompt Target Selector
        // Else, target self

        // Right click: Stop all action, make that card in the middle to read the description
        // left click anywhere to closed it.

        // DEV: Remove;
        if (Input.GetKeyDown(KeyCode.Space)) // Action
        {
            player.health--;
            playerLifeHUB.SetHealthBar(player.health);

            player.shield--;
            playerLifeHUB.SetShield(player.shield);

            player.actionPoint--;
            UpdateActionPointText(player.actionPoint);
        }
        if (Input.GetKeyDown(KeyCode.R)) // Start turn
        {
            player.health++;
            playerLifeHUB.SetHealthBar(player.health);

            player.actionPoint = player.animalHero.startingActionPoint;
            UpdateActionPointText(player.actionPoint);

            player.AddCardsToHand(player.cardDeck.DrawCards(2));
            UpdateCardsInPlayerHandPanel(player.playerHand);
        }
        // REMOVE ^

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (viewCardPanel.activeSelf)
            {
                CloseViewCardPanel();
            }
        }

        UpdateRemainingCardText(player.cardDeck.GetAllCards().Count);
    }

    private void InitialiseFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        firebaseDBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void UpdateRemainingCardText(int remainingCards) 
    {
        remainingCardText.text = $"Remaining:\n{remainingCards}";
    }

    public void ClearCardsInPlayerHandPanel()
    {
        for (int i = playerHandPanel.transform.childCount - 1; i >= 0; i--)
        {
            GameObject childObject = playerHandPanel.transform.GetChild(i).gameObject;
            Destroy(childObject);
        }
    }

    public void UpdateCardsInPlayerHandPanel(List<Card> playerHandCards) 
    {
        ClearCardsInPlayerHandPanel();

        foreach (Card card in playerHandCards)
        {
            GameObject cardPrefab = Instantiate(playerCardPrefab, playerHandPanel.transform);
            cardPrefab.GetComponent<Image>().sprite = card.CardFrontSprite;
        }
    }
    // public IEnumerator DrawCardBySecond(int numberOfDraw, float seconds) 
    // yield new return new WaitForSeconds(second)

    public void PlayerEndTurnButton()
    {
        if (ActionNotEndable(player.actionPoint))
        {
            Debug.Log($"Cannot end turn as you have {player.actionPoint} turns left!");
            //return;
        }
        else
        {
            List<Card> drawnCards = chestCardDeck.DrawCards(1);

            player.AddCardsToHand(drawnCards);
            UpdateCardsInPlayerHandPanel(player.playerHand);
        }
    }

    private bool ActionNotEndable(int actionPoint)
    {
        return (actionPoint > 0);
    }

    private void UpdateActionPointText(int actionPoint) 
    {
        if (actionPoint >= 0) {
            actionPointLeftText.text = actionPoint.ToString();
        }
    }

    public void ShowViewCardPanel(int cardIndex) 
    {
        Card viewCard = player.playerHand[cardIndex];
        
        viewCardImage.sprite = viewCard.CardFrontSprite;
        viewCardPanel.SetActive(true);
    }

    public void CloseViewCardPanel() 
    {
        viewCardImage.sprite = null;
        viewCardPanel.SetActive(false);
    }
}
