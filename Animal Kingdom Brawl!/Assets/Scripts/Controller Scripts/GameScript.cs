using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    [Header("Setting Variables")]
    private float buttonImageAlphaThreshold = 0.1f;

    [Header("Firebase")]
    public FirebaseAuth auth;
    public DatabaseReference firebaseDBReference;

    [Header("Player's Attribute")]
    private string userID;

    [Header("UI")]
    public Canvas canvas;
    public TMP_Text remainingCardText;
    public Image remainingCardImage;
    public TMP_Text actionPointLeftText;
    public Button endTurnButton;

    public Image enemyOneHeroImage;
    public Button enemyOneSelectionButton;
    public Image enemyTwoHeroImage;
    public Button enemyTwoSelectionButton;
    public Image enemyThreeHeroImage;
    public Button enemyThreeSelectionButton;
    public Image hornterrorHeroImage;
    public Button hornterrorSelectionButton;

    public GameObject viewCardPanel;
    public Image viewCardImage;
    public GameObject viewCardAbilityPanel;
    public TMP_Text viewCardAbilityDescriptionText;
    public GameObject singlePlayerSelectionPanel;

    [Header("Player GameObjects")]
    public GameObject playerCardPrefab;
    public GameObject playerHandPanel;

    [Header("Character Life Hub")]
    public LifeHUB playerLifeHUB;
    public LifeHUB enemyOneLifeHUB;
    public LifeHUB enemyTwoLifeHUB;
    public LifeHUB enemyThreeLifeHUB;
    public LifeHUB hornterrorLifeHUB;

    [Header("Game Variables")]
    private int currentTurn = 0;
    private CardDeck chestCardDeck;
    private Player currentTurnPlayer;
    private Player player;
    private Player enemyOneNPC;
    private Player enemyTwoNPC;
    private Player enemyThreeNPC;
    private Player hornterrorNPC;
    private List<Player> allCharacters;


    void Awake()
    {
        InitialiseFirebase();

        userID = auth.CurrentUser.UserId;
    }

    void Start()
    {
        // Make button clickable on full precision of the button image
        endTurnButton.image.alphaHitTestMinimumThreshold = buttonImageAlphaThreshold;

        #region Match Setting
        chestCardDeck = CardDBSchema.defaultChestCardDeck;
        chestCardDeck.ShuffleCards();
        #endregion

        #region Player
        // TODO: Need to get AnimalHero from Choice in ChooseHeroScene
        player = new Player(new Catomic(), CardDBSchema.catomicDefaultCardDeck);
        player.cardDeck.ShuffleCards();
        playerLifeHUB.SetMaxHealth(player.health);
        playerLifeHUB.SetShield(player.shield);
        UpdateActionPointText(player.actionPoint);

        UpdateRemainingCardText(player.cardDeck.GetAllCards().Count);
        UpdateRemainingCardBackImage(player.cardDeck.GetAllCards()[0].CardBackSprite);
        #endregion

        #region Computer AI
        hornterrorNPC = new Player(new Hornterror(), CardDBSchema.hornterrorDefaultCardDeck);
        hornterrorNPC.cardDeck.ShuffleCards();
        hornterrorHeroImage.sprite = hornterrorNPC.animalHero.animalHeroImage;
        hornterrorLifeHUB.SetMaxHealth(hornterrorNPC.health);
        hornterrorLifeHUB.SetShield(hornterrorNPC.shield);
        hornterrorSelectionButton.image.sprite = hornterrorNPC.animalHero.animalHeroImage;

        // TODO: Make enemy 1 2 3 all random (make sure is unique and no repeat)
        enemyOneNPC = new Player(new Catomic(), CardDBSchema.catomicDefaultCardDeck);
        enemyOneNPC.cardDeck.ShuffleCards();
        enemyOneHeroImage.sprite = enemyOneNPC.animalHero.animalHeroImage;
        enemyOneLifeHUB.SetMaxHealth(enemyOneNPC.health);
        enemyOneLifeHUB.SetShield(enemyOneNPC.shield);
        enemyOneSelectionButton.image.sprite = enemyOneNPC.animalHero.animalHeroImage;

        //testing
        enemyOneNPC.activeEffects.Add(ActiveEffect.Piggion_ThickSkin);
        enemyOneNPC.shield++;
        enemyOneLifeHUB.SetShield(enemyOneNPC.shield);

        enemyTwoNPC = new Player(new Catomic(), CardDBSchema.catomicDefaultCardDeck);
        enemyTwoNPC.cardDeck.ShuffleCards();
        enemyTwoHeroImage.sprite = enemyTwoNPC.animalHero.animalHeroImage;
        enemyTwoLifeHUB.SetMaxHealth(enemyTwoNPC.health);
        enemyTwoLifeHUB.SetShield(enemyTwoNPC.shield);
        enemyTwoSelectionButton.image.sprite = enemyTwoNPC.animalHero.animalHeroImage;

        enemyThreeNPC = new Player(new Catomic(), CardDBSchema.catomicDefaultCardDeck);
        enemyThreeNPC.cardDeck.ShuffleCards();
        enemyThreeHeroImage.sprite = enemyThreeNPC.animalHero.animalHeroImage;
        enemyThreeLifeHUB.SetMaxHealth(enemyThreeNPC.health);
        enemyThreeLifeHUB.SetShield(enemyThreeNPC.shield);
        enemyThreeSelectionButton.image.sprite = enemyThreeNPC.animalHero.animalHeroImage;
        #endregion

        allCharacters = new List<Player>() { player, enemyOneNPC, enemyTwoNPC, enemyThreeNPC, hornterrorNPC }; 

        currentTurnPlayer = allCharacters[0];
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

            player.playerHandDeck.AddCards(player.cardDeck.DrawCards(2));
            UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
        }
        // REMOVE ^

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (viewCardPanel.activeSelf)
            {
                CloseViewCardPanel();
            }
        }

        UpdateUI();
    }

    private void InitialiseFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        firebaseDBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void UpdateUI() // TODO: Optimise this
    {
        UpdateRemainingCardText(player.cardDeck.GetAllCards().Count);
        UpdateActionPointText(player.actionPoint);
        playerLifeHUB.SetHealthBar(player.health);
        playerLifeHUB.SetShield(player.shield);

        enemyOneLifeHUB.SetHealthBar(enemyOneNPC.health);
        enemyOneLifeHUB.SetShield(enemyOneNPC.shield);

        enemyTwoLifeHUB.SetHealthBar(enemyTwoNPC.health);
        enemyTwoLifeHUB.SetShield(enemyTwoNPC.shield);

        enemyThreeLifeHUB.SetHealthBar(enemyThreeNPC.health);
        enemyThreeLifeHUB.SetShield(enemyThreeNPC.shield);

        hornterrorLifeHUB.SetHealthBar(hornterrorNPC.health);
        hornterrorLifeHUB.SetShield(hornterrorNPC.shield);
    }

    private void UpdateRemainingCardText(int remainingCards)
    {
        remainingCardText.text = $"Remaining:\n{remainingCards}";
    }

    private void UpdateRemainingCardBackImage(Sprite cardBackImage)
    {
        remainingCardImage.sprite = cardBackImage;
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
    // TODO: public IEnumerator DrawCardBySecond(int numberOfDraw, float seconds) 
    // yield new return new WaitForSeconds(second)

    public void CreateCardPlaceholder(int cardIndex)
    {
        GameObject cardPlaceholderPrefab = Instantiate(playerCardPrefab, playerHandPanel.transform);
        Sprite sprite = Resources.Load<Sprite>("Cards/Cardback/Empty Card Placeholder");
        cardPlaceholderPrefab.GetComponent<Image>().sprite = sprite;

        cardPlaceholderPrefab.transform.SetSiblingIndex(cardIndex);
    }

    public void DestroyCardPlacholder(int cardIndex)
    {
        GameObject cardPlaceholder = playerHandPanel.transform.GetChild(cardIndex).gameObject;
        Destroy(cardPlaceholder);
    }

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

            player.playerHandDeck.AddCards(drawnCards);
            UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
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

    public void ActivateCard(int cardIndex)
    {
        currentTurnPlayer.actionPoint--;

        Card playedCard = currentTurnPlayer.playerHandDeck.GetAllCards()[cardIndex];

        foreach (var abilityType in playedCard.CardAbility.abilityType)
        {
            switch (abilityType)
            {
                case AbilityType.NonTargetable:
                    playedCard.CardAbility.ActivateAbility(currentTurnPlayer, null);
                    break;

                case AbilityType.SingleTargetable:
                    ShowSinglePlayerSelectionPanel();
                    StartCoroutine(WaitForPlayerSelection(playedCard, currentTurnPlayer, 1));
                    break;

                case AbilityType.TargetAllCharacters:
                    playedCard.CardAbility.ActivateAbility(currentTurnPlayer, allCharacters);
                    break;

                case AbilityType.TargetAllPlayers:
                    List<Player> allPlayers = new List<Player>() { enemyOneNPC, enemyTwoNPC, enemyThreeNPC };
                    playedCard.CardAbility.ActivateAbility(currentTurnPlayer, allPlayers);
                    break;

                default: break;
            }
        }

        currentTurnPlayer.playerHandDeck.RemoveSingleCard(playedCard);
        currentTurnPlayer.discardDeck.AddSingleCard(playedCard);

        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
    }

    private IEnumerator WaitForPlayerSelection(Card playedCard, Player caster, int targetCount)
    {
        List<Player> selectedPlayers = new List<Player>();

        hornterrorSelectionButton.onClick.AddListener(() => OnPlayerSelectionButton(0));
        enemyOneSelectionButton.onClick.AddListener(() => OnPlayerSelectionButton(1));
        enemyTwoSelectionButton.onClick.AddListener(() => OnPlayerSelectionButton(2));
        enemyThreeSelectionButton.onClick.AddListener(() => OnPlayerSelectionButton(3));

        yield return new WaitUntil(() => selectedPlayers.Count == targetCount);
        
        playedCard.CardAbility.ActivateAbility(caster, selectedPlayers);
        CloseSinglePlayerSelectionPanel();
        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());

        // Internal function
        void OnPlayerSelectionButton(int index) 
        {
            switch (index)
            {
                case 0: selectedPlayers.Add(hornterrorNPC); break;
                case 1: selectedPlayers.Add(enemyOneNPC); break;
                case 2: selectedPlayers.Add(enemyTwoNPC); break;
                case 3: selectedPlayers.Add(enemyThreeNPC); break;
                default: break;
            }
        }
    }

    public void ShowSinglePlayerSelectionPanel()
    {
        singlePlayerSelectionPanel.SetActive(true);
    }

    public void CloseSinglePlayerSelectionPanel()
    {
        singlePlayerSelectionPanel.SetActive(false);
    }

    public void ShowViewCardPanel(int cardIndex) 
    {
        Card viewCard = player.playerHandDeck.GetAllCards()[cardIndex];
        
        viewCardImage.sprite = viewCard.CardFrontSprite;
        viewCardPanel.SetActive(true);

        if (viewCard.CardCategory == CardCategory.HeroAbility) 
        { 
            viewCardAbilityDescriptionText.text = viewCard.CardAbility.description;
            viewCardAbilityPanel.SetActive(true);
        }
    }

    public void CloseViewCardPanel() 
    {
        viewCardImage.sprite = null;
        viewCardPanel.SetActive(false);

        viewCardAbilityPanel.SetActive(false);
        viewCardAbilityDescriptionText.text = null;
    }
}
