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

        player = new Player(new Catomic(), CardDBSchema.GetCatomicDefaultCardDeck());
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

        enemyTwoNPC = new Player(new Catomic(), CardDBSchema.GetCatomicDefaultCardDeck());
        enemyTwoHeroImage.sprite = enemyTwoNPC.animalHero.animalHeroImage;
        enemyTwoLifeHUB.SetMaxHealth(enemyTwoNPC.health);
        enemyTwoLifeHUB.SetShield(enemyTwoNPC.shield);

        enemyThreeNPC = new Player(new Catomic(), CardDBSchema.GetCatomicDefaultCardDeck());
        enemyThreeHeroImage.sprite = enemyThreeNPC.animalHero.animalHeroImage;
        enemyThreeLifeHUB.SetMaxHealth(enemyThreeNPC.health);
        enemyThreeLifeHUB.SetShield(enemyThreeNPC.shield);

        hornterrorNPC = new Player(new Hornterror(), CardDBSchema.GetHornterrorDefaultCardDeck());
        hornterrorHeroImage.sprite = hornterrorNPC.animalHero.animalHeroImage;
        hornterrorLifeHUB.SetMaxHealth(hornterrorNPC.health);
        hornterrorLifeHUB.SetShield(hornterrorNPC.shield);
        #endregion

        //testing TODO: Remove after testing
        enemyOneNPC.shield++;
        enemyOneLifeHUB.SetShield(enemyOneNPC.shield);
        // testing area

        // Function to call when game first started, before variable setting
        allCharacters = new List<Player>() { player, enemyOneNPC, enemyTwoNPC, enemyThreeNPC, hornterrorNPC };

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

        // TODO: Haven't implement AI endturn function to call UpdateGameTurn();
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

    private void NextPlayerTurn()
    {
        // Take the previous player index, iterate to the next one
        currentPlayerIndex = (currentPlayerIndex + 1) % allCharacters.Count;
        currentTurnPlayer = allCharacters[currentPlayerIndex]; // Already switch to next player

        UpdateGameTurn();

        // Check whether the next player has been knocked out
        if (currentTurnPlayer.isKnockedOut) 
        {
            currentTurnPlayer.isKnockedOut = false;
            // TODO: currentTurnPlayer.OnRevive();

            Debug.Log($"Player {currentPlayerIndex} is knocked out");
            NextPlayerTurn();
            return;
        }

        // Starting turn action for the next player
        currentTurnPlayer.actionPoint = currentTurnPlayer.animalHero.startingActionPoint;

        // Draw starting turn's card
        currentTurnPlayer.playerHandDeck.AddCards(currentTurnPlayer.cardDeck.DrawCards(1));

        // Update player card hand if current turn's player is player
        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
    }

    private void UpdateGameTurn() 
    {
        string[] currentPlayerString = { "You", "Enemy 1", "Enemy 2", "Enemy 3", "Hornterror"};
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
            if (drawnCards[0].CardCategory == CardCategory.Relic)
            {
                StartCoroutine(ShowChestDrawnCardPanel(drawnCards[0]));
                ActivateCard(drawnCards[0]);
            }
            // Draw Trap Card
            else if (drawnCards[0].CardCategory == CardCategory.Trap) 
            {
                StartCoroutine(ShowChestDrawnCardPanel(drawnCards[0]));
                StartCoroutine(ShowReinsertTrapCardPanel(drawnCards[0]));
                ActivateCard(drawnCards[0]);
            }
            // If Trap Done / Not trap, do the normal action here
            else
            {
                player.playerHandDeck.AddCards(drawnCards);
            } 
    
            NextPlayerTurn();
        }

        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
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

    public void CardOnDrop(int cardIndex)
    {
        Card playedCard = currentTurnPlayer.playerHandDeck.GetAllCards()[cardIndex];

        ActivateCard(playedCard);
        
        currentTurnPlayer.playerHandDeck.RemoveSingleCard(playedCard);
        currentTurnPlayer.discardDeck.AddSingleCard(playedCard);

        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());

        // Played card is the same category with the current turn's player her categgory.
        // Search every players and see whose player does this card falls into the same category with.
        // TODO: Wait until AI implemented first, then only add to discard pile based on the card category.
        //foreach (Player character in allCharacters)
        //{
        //    if (playedCard.CardCategory == character.heroCardCategory) 
        //    {
        //        Debug.Log($"used {playedCard.CardFrontSprite} and catergory is {character.heroCardCategory}");
        //        character.discardDeck.AddSingleCard(playedCard);
        //    }
        //}

        //if (playedCard.CardCategory == CardCategory.Artifact) 
        //{
        //    discardChestCardDeck.AddSingleCard(playedCard);
        //}
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

        if (playedCard.CardCategory != CardCategory.Relic || playedCard.CardCategory != CardCategory.Trap) 
        {
            currentTurnPlayer.actionPoint--;
        }

        switch (playedCard.CardAbility.abilityType)
        {
            case AbilityType.NonTargetable:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, null);
                break;

            case AbilityType.SingleTargetable:
                StartCoroutine(ShowSinglePlayerSelectionPanel(currentTurnPlayer, playedCard, 1));
                break;

            case AbilityType.TargetAllOpponents:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, targetAllOpponents);
                break;

            case AbilityType.TargetAllPlayers:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, targetAllPlayers);
                break; 

            case AbilityType.TargetAllCharacters:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, allCharacters);
                break;

            case AbilityType.CardViewable:
                ShowRevealCardsPanel(chestCardDeck, 5);
                break;

            default: break;
        }

        Debug.Log(currentTurnPlayer.health + " current turn");
        Debug.Log(player.health + " player health");
    }

    public IEnumerator ShowSinglePlayerSelectionPanel(Player caster, Card playedCard, int targetCount)
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

    private IEnumerator ShowChestDrawnCardPanel(Card card)
    {
        string[] currentPlayerString = { "You", "Enemy 1", "Enemy 2", "Enemy 3", "Hornterror" };
        int index = allCharacters.IndexOf(currentTurnPlayer);

        string relicMessage = $"{currentPlayerString[index]} collected a relic card!";
        string trapMessage = $"{currentPlayerString[index]} triggered a trap card!";

        showChestDrawnCardPanel.SetActive(true);

        if (card.CardCategory == CardCategory.Relic)
        {
            showChestDrawnCardText.text = relicMessage;
            showChestDrawnCardImage.sprite = card.CardFrontSprite;
        } 
        else if (card.CardCategory == CardCategory.Trap) 
        {
            showChestDrawnCardText.text = trapMessage;
            showChestDrawnCardImage.sprite = card.CardFrontSprite;
        }
       
        yield return new WaitForSeconds(3);

        if (showChestDrawnCardPanel.activeSelf) 
        { 
            CloseChestDrawnCardPanel();
        }
    }

    public void CloseChestDrawnCardPanel() 
    {
        showChestDrawnCardPanel.SetActive(false);
        showChestDrawnCardText.text = "";
        showChestDrawnCardImage.sprite = null;
    }

    private IEnumerator ShowReinsertTrapCardPanel(Card trapCard) 
    {
        yield return new WaitUntil(() => !showChestDrawnCardPanel.activeSelf);

        reinsertTrapCardPanel.SetActive(true);

        for (int i = 0; i < chestCardDeck.GetAllCards().Count; i++)
        {
            int buttonValue = i;
            GameObject prefab = Instantiate(reinsertItemPrefab, reinsertTrapScrollView.content);
            prefab.GetComponentInChildren<Button>().onClick.AddListener(() => ReinsertLocation(buttonValue));
        }

        // Internal function
        void ReinsertLocation(int index)
        {
            chestCardDeck.InsertCardAt(trapCard, index);
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
}
