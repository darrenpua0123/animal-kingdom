using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class GameScript : MonoBehaviour
{
    [Header("Configuration Variables")]
    private float buttonImageAlphaThreshold = 0.1f;

    [Header("Firebase")]
    public FirebaseAuth auth;
    public DatabaseReference firebaseDBReference;

    [Header("Player's Attribute")]
    private string userID;

    [Header("UI")]
    public Canvas canvas;
    public TMP_Text currentTurnText;
    public TMP_Text currentPlayerTurnText;

    // Player
    public TMP_Text remainingCardText;
    public Image remainingCardImage;
    public LifeHUB playerLifeHUB;
    public TMP_Text playerKnockoutCounterText;
    public TMP_Text playerRelicCounterText;
    public GameObject playerHandPanel;
    public TMP_Text actionPointLeftText;
    public Button endTurnButton;

    // Enemy 1
    public Image enemyOneHeroImage;
    public LifeHUB enemyOneLifeHUB; 
    public TMP_Text enemyOneKnockoutCounterText;
    public TMP_Text enemyOneRelicCounterText;

    // Enemy 2
    public Image enemyTwoHeroImage;
    public LifeHUB enemyTwoLifeHUB;
    public TMP_Text enemyTwoKnockoutCounterText;
    public TMP_Text enemyTwoRelicCounterText;

    // Enemy 3
    public Image enemyThreeHeroImage;
    public LifeHUB enemyThreeLifeHUB;
    public TMP_Text enemyThreeKnockoutCounterText;
    public TMP_Text enemyThreeRelicCounterText;

    // Hornterror
    public Image hornterrorHeroImage;
    public LifeHUB hornterrorLifeHUB;
    public List<GameObject> characterGlowImages;
    public List<Button> enemySelectionButton;

    public GameObject viewCardPanel;
    public Image viewCardImage;
    public GameObject viewCardAbilityPanel;
    public TMP_Text viewCardAbilityDescriptionText;
    public GameObject singlePlayerSelectionPanel;
    public GameObject showChestDrawnCardPanel;
    public TMP_Text showChestDrawnCardText;
    public Image showChestDrawnCardImage;
    public GameObject reinsertTrapCardPanel;
    public ScrollRect reinsertTrapScrollView;
    public GameObject revealCardsPanel;
    public GameObject revealCardsZone;
    public GameObject cardPlayedByAIPanel;
    public TMP_Text cardPlayedByAIText;
    public Image cardPlayedByAIImage;

    [Header("Prefabs")]
    public GameObject playerCardPrefab;
    public GameObject reinsertItemPrefab;
    public GameObject revealCardPrefab;

    [Header("Game Variables")]
    private int gameTurn = 0;
    public Player startingPlayer;
    private int currentPlayerIndex = 0;
    private CardDeck chestCardDeck;
    private CardDeck discardChestCardDeck;
    public Player currentTurnPlayer;
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
        chestCardDeck = CardDBSchema.GetDefaultChestDeck();
        chestCardDeck.ShuffleCards();
        discardChestCardDeck = new CardDeck();
        #endregion

        #region Player
        // TODO: Need to get AnimalHero from Choice in ChooseHeroScene

        player = new Player(new Beedle(), CardDBSchema.GetBeedleDefaultCardDeck());
        playerLifeHUB.SetMaxHealth(player.health);
        playerLifeHUB.SetShield(player.shield);
        UpdateActionPointText(player.actionPoint);

        UpdateRemainingCardText(player.cardDeck.GetAllCards().Count);
        UpdateRemainingCardBackImage(player.cardDeck.GetAllCards()[0].CardBackSprite);
        #endregion

        #region Computer AI
        // TODO: Make enemy 1 2 3 all random (make sure is unique and no repeat)
        enemyOneNPC = new Player(new Catomic(), CardDBSchema.GetCatomicDefaultCardDeck());
        enemyOneHeroImage.sprite = enemyOneNPC.animalHero.animalHeroImage;
        enemyOneLifeHUB.SetMaxHealth(enemyOneNPC.health);
        enemyOneLifeHUB.SetShield(enemyOneNPC.shield);

        enemyTwoNPC = new Player(new Piggion(), CardDBSchema.GetPiggionDefaultCardDeck());
        enemyTwoHeroImage.sprite = enemyTwoNPC.animalHero.animalHeroImage;
        enemyTwoLifeHUB.SetMaxHealth(enemyTwoNPC.health);
        enemyTwoLifeHUB.SetShield(enemyTwoNPC.shield);

        enemyThreeNPC = new Player(new Pandragon(), CardDBSchema.GetPandragonDefaultCardDeck());
        enemyThreeHeroImage.sprite = enemyThreeNPC.animalHero.animalHeroImage;
        enemyThreeLifeHUB.SetMaxHealth(enemyThreeNPC.health);
        enemyThreeLifeHUB.SetShield(enemyThreeNPC.shield);

        hornterrorNPC = new Player(new Hornterror(), CardDBSchema.GetHornterrorDefaultCardDeck());
        hornterrorHeroImage.sprite = hornterrorNPC.animalHero.animalHeroImage;
        hornterrorLifeHUB.SetMaxHealth(hornterrorNPC.health);
        hornterrorLifeHUB.SetShield(hornterrorNPC.shield);
        #endregion

        //testing TODO: Remove after testing
        enemyThreeNPC.health = 3;
        player.knockoutCounter = 2;

        enemyTwoNPC.isKnockedOut = true;
        enemyTwoNPC.health = 0;

        // testing area

        // Function to call when game first started, before variable setting
        allCharacters = new List<Player>() { player, enemyOneNPC, enemyTwoNPC, enemyThreeNPC, hornterrorNPC };

        // Starting draw cards
        foreach (Player character in allCharacters) 
        {
            character.cardDeck.ShuffleCards();

            if (character == hornterrorNPC)
            {
                character.playerHandDeck.AddCards(character.cardDeck.DrawCards(2));
            }
            else 
            { 
                character.playerHandDeck.AddCards(character.cardDeck.DrawCards(3));
            }
        }

        startingPlayer = allCharacters[0];
        currentTurnPlayer = allCharacters[currentPlayerIndex];

        UpdateGameTurn();
        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
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
        if (Input.GetKeyDown(KeyCode.T)) // Move to next turn
        {
            NextPlayerTurn();
        }
        if (Input.GetKeyDown(KeyCode.E)) // Kill player
        {
            player.health = 0;
            player.isKnockedOut = true;    
        }
        if (Input.GetKeyDown(KeyCode.Space)) // Reduce AP
        {
            player.actionPoint--;
            UpdateActionPointText(player.actionPoint);
        }
        if (Input.GetKeyDown(KeyCode.R)) // Draw animal hero card
        {
            player.actionPoint = player.animalHero.startingActionPoint;
            UpdateActionPointText(player.actionPoint);

            player.playerHandDeck.AddCards(player.cardDeck.DrawCards(2));
            UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
        }
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            player.playerHandDeck.GetAllCards().Clear();
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
        playerKnockoutCounterText.text = player.knockoutCounter.ToString();
        playerRelicCounterText.text = player.relicCounter.ToString();

        enemyOneLifeHUB.SetHealthBar(enemyOneNPC.health);
        enemyOneLifeHUB.SetShield(enemyOneNPC.shield);
        enemyOneKnockoutCounterText.text = enemyOneNPC.knockoutCounter.ToString();
        enemyOneRelicCounterText.text = enemyOneNPC.relicCounter.ToString();

        enemyTwoLifeHUB.SetHealthBar(enemyTwoNPC.health);
        enemyTwoLifeHUB.SetShield(enemyTwoNPC.shield);
        enemyTwoKnockoutCounterText.text = enemyTwoNPC.knockoutCounter.ToString();
        enemyTwoRelicCounterText.text = enemyTwoNPC.relicCounter.ToString();

        enemyThreeLifeHUB.SetHealthBar(enemyThreeNPC.health);
        enemyThreeLifeHUB.SetShield(enemyThreeNPC.shield);
        enemyThreeKnockoutCounterText.text = enemyThreeNPC.knockoutCounter.ToString();
        enemyThreeRelicCounterText.text = enemyThreeNPC.relicCounter.ToString();

        hornterrorLifeHUB.SetHealthBar(hornterrorNPC.health);
        hornterrorLifeHUB.SetShield(hornterrorNPC.shield);

        UpdatePlayerEndTurnButton();
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

    private void UpdatePlayerEndTurnButton() 
    { 
        endTurnButton.interactable = false;

        if (currentTurnPlayer == startingPlayer) 
        {
            endTurnButton.interactable = true;
        }
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

            // Draw Relic Card
            if (drawnCards[0].CardType == CardType.Relic)
            {
                StartCoroutine(ShowChestDrawnCardPanel(drawnCards[0], () => 
                { 
                    ActivateCard(drawnCards[0]);
                    NextPlayerTurn();
                }));
            }
            // Draw Trap Card
            else if (drawnCards[0].CardType == CardType.Trap) 
            {
                StartCoroutine(ShowChestDrawnCardPanel(drawnCards[0], () => {}));
                StartCoroutine(ShowReinsertTrapCardPanel(drawnCards[0], () => 
                { 
                    ActivateCard(drawnCards[0]);
                    NextPlayerTurn();
                }));
            }
            // Draw Artifact, do the normal action here
            else
            {
                player.playerHandDeck.AddCards(drawnCards);
                NextPlayerTurn();
            } 
        }

        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
    }

    private void UpdateActionPointText(int actionPoint)
    {
        if (actionPoint >= 0)
        {
            actionPointLeftText.text = actionPoint.ToString();
        }
    }

    private bool ActionNotEndable(int actionPoint)
    {
        return (actionPoint > 0);
    }

    private void CheckCurrentPlayerCondition() 
    {
        // Check if is knocked out
        if (currentTurnPlayer.isKnockedOut) 
        {
            NextPlayerTurn();
        }

        if (currentTurnPlayer.playerHandDeck.GetAllCards().Count <= 0) 
        {
            currentTurnPlayer.playerHandDeck.AddCards(currentTurnPlayer.cardDeck.DrawCards(2));

            if (currentTurnPlayer == player) 
            { 
                UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
            }
        }
    }

    private void NextPlayerTurn()
    {
        // Take the previous player index, iterate to the next one
        currentPlayerIndex = (currentPlayerIndex + 1) % allCharacters.Count;
        currentTurnPlayer = allCharacters[currentPlayerIndex]; // Already switch to next player

        UpdateGameTurn();

        // Check whether the next player has been knocked out
        if (currentTurnPlayer.isKnockedOut)
        {
            // Discard all cards in next player's hand, and send them to their respective card decks
            foreach (Card card in currentTurnPlayer.playerHandDeck.GetAllCards())
            {
                HandleDiscardCard(card);
            }

            // Clear next player's hand, add their discard cards to their card deck, then revive the player
            currentTurnPlayer.playerHandDeck.GetAllCards().Clear();
            currentTurnPlayer.cardDeck.AddCards(currentTurnPlayer.discardDeck.GetAllCards());
            currentTurnPlayer.OnRevive();

            Debug.Log($"Player {currentPlayerIndex} is knocked out");
            // However, player is not available to move this turn, as reviving takes one turn
            NextPlayerTurn();
            return;
        }

        // Clear all active effects for the next player
        currentTurnPlayer.activeEffects.Clear();

        // Starting turn action for the next player
        currentTurnPlayer.actionPoint = currentTurnPlayer.animalHero.startingActionPoint;

        // Draw starting turn's card
        currentTurnPlayer.playerHandDeck.AddCards(currentTurnPlayer.cardDeck.DrawCards(1));

        // Only engage AI if the current turn player isn't the player
        if (currentTurnPlayer != player)
        {
            StartCoroutine(PlayerAI(currentTurnPlayer));
        }

        // Update player card hand 
        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
    }

    private void UpdateGameTurn()
    {
        string[] currentPlayerString = { "You", "Enemy 1", "Enemy 2", "Enemy 3", "Hornterror" };
        int index = allCharacters.IndexOf(currentTurnPlayer);

        if (currentTurnPlayer == startingPlayer)
        {
            gameTurn++;
        }

        currentTurnText.text = $"Turn: {gameTurn}";
        currentPlayerTurnText.text = currentPlayerString[index];

        foreach (var glowImg in characterGlowImages)
        {
            glowImg.SetActive(false);
        }
        characterGlowImages[index].SetActive(true);
    }

    public void CardOnDrop(int cardIndex)
    {
        Card playedCard = currentTurnPlayer.playerHandDeck.GetAllCards()[cardIndex];

        // Used and activate the card
        ActivateCard(playedCard);
        
        // Remove the played card
        currentTurnPlayer.playerHandDeck.RemoveSingleCard(playedCard);

        // Discard the played card
        HandleDiscardCard(playedCard);
        
        // Check if player coondition
        CheckCurrentPlayerCondition();

        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
    }

    public void HandleDiscardCard(Card discardCard)
    {
        bool isChestCard = true;

        foreach (Player player in allCharacters)
        {
            if (discardCard.CardCategory == player.heroCardCategory) 
            {
                player.discardDeck.AddSingleCard(discardCard);
                isChestCard = false;
                break;
            }
        }

        if (isChestCard) 
        {
            discardChestCardDeck.AddSingleCard(discardCard);
        }
    }

    private void ActivateCard(Card playedCard) 
    { 
        // exclusive of Hornterror, but inclusive of Current Turn Player
        List<Player> targetAllPlayers = new List<Player>(allCharacters);
        targetAllPlayers.Remove(hornterrorNPC);
        
        // exclusive of Hornterror and Current Turn Player
        List<Player> targetAllOpponents = new List<Player>(allCharacters);
        targetAllOpponents.Remove(currentTurnPlayer);
        targetAllOpponents.Remove(hornterrorNPC);

        if (playedCard.CardType != CardType.Relic || playedCard.CardType != CardType.Trap) 
        {
            currentTurnPlayer.actionPoint--;
        }

        switch (playedCard.CardAbility.abilityType)
        {
            case AbilityType.NonTargetable:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, null);
                break;

            case AbilityType.TargetAllCharacters:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, allCharacters);
                break;

            case AbilityType.TargetAllPlayers:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, targetAllPlayers);
                break;

            case AbilityType.TargetAllOpponents:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, targetAllOpponents);
                break;

            case AbilityType.AllCharacterSingleTargetable:
                StartCoroutine(ShowSinglePlayerSelectionPanel(currentTurnPlayer, false, playedCard, 1));
                break;

            case AbilityType.AllOpponentSingleTargetable:
                StartCoroutine(ShowSinglePlayerSelectionPanel(currentTurnPlayer, true, playedCard, 1));
                break;

            case AbilityType.FiveCardViewable:
                ShowRevealCardsPanel(chestCardDeck, 5);
                break;

            case AbilityType.ThreeCardViewable:
                ShowRevealCardsPanel(chestCardDeck, 3);
                break;

            default: break;
        }
    }

    private void ActivateCardForAI(Card playedCard)
    {
        System.Random rand = new System.Random();

        // exclusive of Hornterror, but inclusive of Current Turn Player
        List<Player> targetAllPlayers = new List<Player>(allCharacters);
        targetAllPlayers.Remove(hornterrorNPC);

        // exclusive of Hornterror and Current Turn Player
        List<Player> targetAllOpponents = new List<Player>(allCharacters);
        targetAllOpponents.Remove(currentTurnPlayer);
        targetAllOpponents.Remove(hornterrorNPC);

        // exclusive of Current Turn Player
        List<Player> targetAllEnemies = new List<Player>(allCharacters);
        targetAllEnemies.Remove(currentTurnPlayer);

        // Target player who almost won
        List<Player> prioritisedTarget = new List<Player>();

        foreach (Player targetPlayer in targetAllOpponents)
        {
            if (targetPlayer.isKnockedOut) 
            {
                continue;            
            }

            if (targetPlayer.relicCounter == 1)
            {
                prioritisedTarget.Add(targetPlayer);
            }
            else if (targetPlayer.health <= 5)
            {
                prioritisedTarget.Add(targetPlayer);
            }
            else if (targetPlayer.relicCounter >= 2 || targetPlayer.knockoutCounter == 4)
            {
                prioritisedTarget.Clear();
                prioritisedTarget.Add(targetPlayer);
                break;
            }
        }

        // If no prioritse target, then random select
        while (prioritisedTarget.Count == 0)
        {
            int playerIndex = rand.Next(0, targetAllEnemies.Count);

            if (!targetAllEnemies[playerIndex].isKnockedOut) 
            { 
                prioritisedTarget.Add(targetAllEnemies[playerIndex]);
                break;
            }
        }

        // activate the card
        switch (playedCard.CardAbility.abilityType)
        {
            // TODO: Double check the target system
            case AbilityType.NonTargetable:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, null);
                break;

            case AbilityType.TargetAllCharacters:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, allCharacters);
                break;

            case AbilityType.TargetAllPlayers:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, targetAllPlayers);
                break;

            case AbilityType.TargetAllOpponents:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, targetAllOpponents);
                break;

            case AbilityType.AllCharacterSingleTargetable:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, prioritisedTarget);
                break;

            case AbilityType.AllOpponentSingleTargetable:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, prioritisedTarget);
                break;

            // unused
            case AbilityType.FiveCardViewable: break;
            case AbilityType.ThreeCardViewable: break;

            default: break;
        }
    }

    private IEnumerator PlayerAI(Player currentTurnPlayer)
    {
        System.Random rand = new System.Random();

        yield return new WaitForSeconds(1);

        // Use and activate card
        while (currentTurnPlayer.actionPoint > 0)
        {
            int cardIndex = rand.Next(0, currentTurnPlayer.playerHandDeck.GetAllCards().Count);
            Card playedCard = currentTurnPlayer.playerHandDeck.GetAllCards()[cardIndex];

            // reduce the action point
            if (playedCard.CardType != CardType.Relic || playedCard.CardType != CardType.Trap)
            {
                currentTurnPlayer.actionPoint--;
            }

            // play cards
            ActivateCardForAI(playedCard);

            // remove from hand
            currentTurnPlayer.playerHandDeck.RemoveSingleCard(playedCard);

            // discard the card
            HandleDiscardCard(playedCard);

            // check if current turn player's hand is empty
            CheckCurrentPlayerCondition();

            // after show
            yield return StartCoroutine(ShowCardPlayedByAIPanel(playedCard));

            UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());

            yield return new WaitForSeconds(1);
        }

        Debug.Log($"Enemy {allCharacters.IndexOf(currentTurnPlayer)} now have total of {currentTurnPlayer.playerHandDeck.GetAllCards().Count} cards");

        // After action point finish, draw a card from chest card deck
        // Horn terror is not required
        if (currentTurnPlayer == hornterrorNPC)
        {
            NextPlayerTurn();
            yield break;
        }

        List<Card> drawnCards = chestCardDeck.DrawCards(1);

        // Drew Relic Card
        if (drawnCards[0].CardType == CardType.Relic)
        {
            yield return StartCoroutine(ShowCardPlayedByAIPanel(drawnCards[0]));
            ActivateCardForAI(drawnCards[0]);
            NextPlayerTurn();
        }
        // Drew Trap Card
        else if (drawnCards[0].CardType == CardType.Trap)
        {
            yield return StartCoroutine(ShowCardPlayedByAIPanel(drawnCards[0]));
            ActivateCardForAI(drawnCards[0]);
            chestCardDeck.InsertCardAt(drawnCards[0], rand.Next(0, chestCardDeck.GetAllCards().Count));
            NextPlayerTurn();
        }
        // Drew Artifact, do the normal action here
        else
        {
            currentTurnPlayer.playerHandDeck.AddCards(drawnCards);
            NextPlayerTurn();
        }
    }

    public void ShowViewCardPanel(int cardIndex)
    {
        Card viewCard = player.playerHandDeck.GetAllCards()[cardIndex];

        viewCardImage.sprite = viewCard.CardFrontSprite;
        viewCardPanel.SetActive(true);

        if (viewCard.CardType == CardType.HeroAbility)
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

    public IEnumerator ShowSinglePlayerSelectionPanel(Player caster, bool allOpponent, Card playedCard, int targetCount)
    {
        singlePlayerSelectionPanel.SetActive(true);

        List<Player> selectedPlayers = new List<Player>();
        List<Player> allEnemies = new List<Player>(allCharacters);
        allEnemies.Remove(currentTurnPlayer);

        for (int i = 0; i < allEnemies.Count; i++)
        {
            int buttonValue = i;
            enemySelectionButton[i].image.sprite = allEnemies[i].animalHero.animalHeroImage;
            enemySelectionButton[i].onClick.AddListener(() => OnPlayerSelectionButton(buttonValue));

            if (allEnemies[i].isKnockedOut)
            {
                enemySelectionButton[i].interactable = false;
            }
        }

        if (allOpponent) 
        {
            // If target is all opponent, then disable hornterror's selection
            enemySelectionButton[allEnemies.Count - 1].interactable = false;
        }

        // Wait until the selected player count has met the expected selected amount
        yield return new WaitUntil(() => selectedPlayers.Count == targetCount);

        playedCard.CardAbility.ActivateAbility(caster, selectedPlayers);
        CloseSinglePlayerSelectionPanel();
        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());

        // internal function
        void OnPlayerSelectionButton(int index) 
        {
            switch (index)
            {
                case 0: selectedPlayers.Add(enemyOneNPC); break;
                case 1: selectedPlayers.Add(enemyTwoNPC); break;
                case 2: selectedPlayers.Add(enemyThreeNPC); break;
                case 3: selectedPlayers.Add(hornterrorNPC); break;
                default: break;
            }
        }
    }

    public void CloseSinglePlayerSelectionPanel()
    {
        singlePlayerSelectionPanel.SetActive(false);

        foreach (var enemyButton in enemySelectionButton)
        {
            enemyButton.interactable = true;
        }
    }

    private IEnumerator ShowChestDrawnCardPanel(Card card, Action onComplete)
    {
        string[] currentPlayerString = { "You", "Enemy 1", "Enemy 2", "Enemy 3", "Hornterror" };
        int index = allCharacters.IndexOf(currentTurnPlayer);

        string relicMessage = $"{currentPlayerString[index]} collected a relic card!";
        string trapMessage = $"{currentPlayerString[index]} triggered a trap card!";

        showChestDrawnCardPanel.SetActive(true);

        if (card.CardType == CardType.Relic)
        {
            showChestDrawnCardText.text = relicMessage;
            showChestDrawnCardImage.sprite = card.CardFrontSprite;
        } 
        else if (card.CardType == CardType.Trap) 
        {
            showChestDrawnCardText.text = trapMessage;
            showChestDrawnCardImage.sprite = card.CardFrontSprite;
        }

        yield return new WaitUntil(() => !showChestDrawnCardPanel.activeSelf);

        //CloseChestDrawnCardPanel();
        onComplete?.Invoke();
    }

    public void CloseChestDrawnCardPanel() 
    {
        showChestDrawnCardPanel.SetActive(false);
        showChestDrawnCardText.text = "";
        showChestDrawnCardImage.sprite = null;
    }

    private IEnumerator ShowReinsertTrapCardPanel(Card trapCard, Action onComplete) 
    {
        yield return new WaitUntil(() => !showChestDrawnCardPanel.activeSelf);

        reinsertTrapCardPanel.SetActive(true);
        bool trapCardInsert = false;

        for (int i = 0; i < chestCardDeck.GetAllCards().Count; i++)
        {
            int buttonValue = i;
            GameObject prefab = Instantiate(reinsertItemPrefab, reinsertTrapScrollView.content);
            prefab.GetComponentInChildren<Button>().onClick.AddListener(() => ReinsertLocation(buttonValue));
        }

        yield return new WaitUntil(() => trapCardInsert);
        onComplete?.Invoke();

        // Internal function
        void ReinsertLocation(int index)
        {
            chestCardDeck.InsertCardAt(trapCard, index);
            trapCardInsert = true;
            CloseReinsertTrapCardPanel();
        }
    }

    public void CloseReinsertTrapCardPanel() 
    {
        reinsertTrapCardPanel.SetActive(false);

        foreach (Transform child in reinsertTrapScrollView.content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowRevealCardsPanel(CardDeck cardDeck, int numberOfCards) 
    { 
        revealCardsPanel.SetActive(true);

        if (cardDeck.GetAllCards().Count < numberOfCards) 
        {
            numberOfCards = cardDeck.GetAllCards().Count;
        }

        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject cardPrefab = Instantiate(revealCardPrefab, revealCardsZone.transform);

            cardPrefab.GetComponent<Image>().sprite = cardDeck.GetAllCards()[i].CardFrontSprite;
        }
    }

    public void CloseRevealCardsPanel() 
    {
        revealCardsPanel.SetActive(false);

        foreach (Transform child in revealCardsZone.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator ShowCardPlayedByAIPanel(Card card)
    {
        // TODO: topright click to show tooltip (left click to dismiss, right click to reveal)
        // TODO: Add audio
        // TODO: After card ability, add random computer selection
        
        cardPlayedByAIPanel.SetActive(true);
        cardPlayedByAIImage.sprite = card.CardFrontSprite;

        SetCardPlayedByAIText(card);

        yield return new WaitForSeconds(4);
        CloseCardPlayedByAIPanel();
    }

    public void CloseCardPlayedByAIPanel()
    {
        cardPlayedByAIPanel.SetActive(false);
        cardPlayedByAIImage.sprite = null;
    }

    private void SetCardPlayedByAIText(Card card) 
    { 
        string[] currentPlayerString = { "You", "Enemy 1", "Enemy 2", "Enemy 3", "Hornterror" };
        int index = allCharacters.IndexOf(currentTurnPlayer);

        if (card.CardType == CardType.Relic)
        {
            cardPlayedByAIText.text = $"{currentPlayerString[index]} collected a relic card!";
        }
        else if (card.CardType == CardType.Trap)
        {
            cardPlayedByAIText.text = $"{currentPlayerString[index]} triggered this trap card!";
        }
        else 
        {
            cardPlayedByAIText.text = $"{currentPlayerString[index]} played this card!";
        }

    }
}
