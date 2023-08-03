using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.XR;
using static Unity.Burst.Intrinsics.X86.Avx;
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
    public GameObject popupTextParent;
    public TMP_Text gameAnnouncementText;
    public TMP_Text remainingChestCardText;

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

    // All players
    public Color aliveColor;
    public Color deadColor;
    public List<GameObject> characterGlowImages;
    public List<Button> enemySelectionButton;

    // Panels
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
    public GameObject helpPanel;
    public GameObject victoryPanel;
    public Image victoryPlayerImage;
    public TMP_Text victoryPlayerText;
    public TMP_Text victoryMethodText;
    public Image victoryByKnockoutImage, victoryByRelicImage;
    public Button victoryOkayButton;

    [Header("Prefabs")]
    public GameObject popupTextPrefab;
    public GameObject playerCardPrefab;
    public GameObject reinsertItemPrefab;
    public GameObject revealCardPrefab;

    [Header("Game Variables")]
    private bool gameWon = false;
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
    private bool deathCoroutineRunning = false;
    private bool reviveCouroutineRunning = false;

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
        AudioManagerScript.instance.Stop(SoundName.MainMenu);
        AudioManagerScript.instance.Play(SoundName.GameMatch);

        chestCardDeck = CardDBSchema.GetDefaultChestDeck();
        chestCardDeck.ShuffleCards();
        discardChestCardDeck = new CardDeck();
        UpdateChestCardRemainingCardText(chestCardDeck.GetAllCards().Count);
        #endregion

        #region Player
        string chosenHero = PlayerPrefs.GetString("ChosenHero");

        player = GetPlayerAnimalHero(chosenHero);
        playerLifeHUB.SetMaxHealth(player.health);
        playerLifeHUB.SetShield(player.shield);
        UpdateActionPointText(player.actionPoint);

        UpdateRemainingCardText(player.cardDeck.GetAllCards().Count);
        UpdateRemainingCardBackImage(player.cardDeck.GetAllCards()[0].CardBackSprite);
        #endregion

        #region Computer AI
        // Set all enemy NPC animal hero
        SetPlayerAIsAnimalHero(chosenHero);

        enemyOneHeroImage.sprite = enemyOneNPC.animalHero.animalHeroImage;
        enemyOneLifeHUB.SetMaxHealth(enemyOneNPC.health);
        enemyOneLifeHUB.SetShield(enemyOneNPC.shield);

        enemyTwoHeroImage.sprite = enemyTwoNPC.animalHero.animalHeroImage;
        enemyTwoLifeHUB.SetMaxHealth(enemyTwoNPC.health);
        enemyTwoLifeHUB.SetShield(enemyTwoNPC.shield);

        enemyThreeHeroImage.sprite = enemyThreeNPC.animalHero.animalHeroImage;
        enemyThreeLifeHUB.SetMaxHealth(enemyThreeNPC.health);
        enemyThreeLifeHUB.SetShield(enemyThreeNPC.shield);

        hornterrorNPC = new Player(new Hornterror(), CardDBSchema.GetHornterrorDefaultCardDeck());
        hornterrorHeroImage.sprite = hornterrorNPC.animalHero.animalHeroImage;
        hornterrorLifeHUB.SetMaxHealth(hornterrorNPC.health);
        hornterrorLifeHUB.SetShield(hornterrorNPC.shield);
        #endregion

        // Function to call when game first started, before variable setting
        allCharacters = new List<Player>() { player, enemyOneNPC, enemyTwoNPC, enemyThreeNPC, hornterrorNPC };

        // Starting draw cards
        foreach (Player character in allCharacters)
        {
            character.cardDeck.ShuffleCards();

            if (character.cardDeck.GetAllCards().Count <= 0)
            {
                character.cardDeck.AddCards(character.discardDeck.GetAllCards());
                character.cardDeck.ShuffleCards();

                character.discardDeck = new CardDeck();
            }

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

        StartCoroutine(UpdateGameTurn(() => { }));
        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (viewCardPanel.activeSelf)
            {
                CloseViewCardPanel();
            }
        }

        if (!gameWon) 
        { 
            CheckCurrentPlayerIsKnockedOut();
        }
        UpdateUI();
    }

    private void InitialiseFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        firebaseDBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void UpdateUI()
    {
        // Update UI that needs to be show prior to the start of a turn
        UpdateChestCardRemainingCardText(chestCardDeck.GetAllCards().Count);
        UpdateRemainingCardText(player.cardDeck.GetAllCards().Count);
        UpdateActionPointText(player.actionPoint);

        // Update Player related UI
        playerLifeHUB.SetHealthBar(player.health);
        playerLifeHUB.SetShield(player.shield);
        playerKnockoutCounterText.text = player.knockoutCounter.ToString();
        playerRelicCounterText.text = player.relicCounter.ToString();

        // Update Enemy 1 related UI
        enemyOneLifeHUB.SetHealthBar(enemyOneNPC.health);
        enemyOneLifeHUB.SetShield(enemyOneNPC.shield);
        enemyOneKnockoutCounterText.text = enemyOneNPC.knockoutCounter.ToString();
        enemyOneRelicCounterText.text = enemyOneNPC.relicCounter.ToString();

        // Update Enemy 2 related UI
        enemyTwoLifeHUB.SetHealthBar(enemyTwoNPC.health);
        enemyTwoLifeHUB.SetShield(enemyTwoNPC.shield);
        enemyTwoKnockoutCounterText.text = enemyTwoNPC.knockoutCounter.ToString();
        enemyTwoRelicCounterText.text = enemyTwoNPC.relicCounter.ToString();

        // Update Enemy 3 related UI
        enemyThreeLifeHUB.SetHealthBar(enemyThreeNPC.health);
        enemyThreeLifeHUB.SetShield(enemyThreeNPC.shield);
        enemyThreeKnockoutCounterText.text = enemyThreeNPC.knockoutCounter.ToString();
        enemyThreeRelicCounterText.text = enemyThreeNPC.relicCounter.ToString();

        // Update Hornterror related UI
        hornterrorLifeHUB.SetHealthBar(hornterrorNPC.health);
        hornterrorLifeHUB.SetShield(hornterrorNPC.shield);

        // Update the player's end turn button at all time
        UpdatePlayerEndTurnButton();
    }

    private Player GetPlayerAnimalHero(string chosenHero)
    {
        Player player;

        switch (chosenHero)
        {
            case "beedle":
                player = new Player(new Beedle(), CardDBSchema.GetBeedleDefaultCardDeck());
                break;

            case "catomic":
                player = new Player(new Catomic(), CardDBSchema.GetCatomicDefaultCardDeck());
                break;

            case "pandragon":
                player = new Player(new Pandragon(), CardDBSchema.GetPandragonDefaultCardDeck());
                break;

            case "piggion":
                player = new Player(new Piggion(), CardDBSchema.GetPiggionDefaultCardDeck());
                break;

            default:
                player = new Player(new Piggion(), CardDBSchema.GetPiggionDefaultCardDeck());
                break;
        }

        return player;
    }

    private void SetPlayerAIsAnimalHero(string playerChosenHero)
    {
        System.Random rand = new System.Random();

        List<string> heroPools = new List<string>() { Catomic.HERO_NAME, Piggion.HERO_NAME, Beedle.HERO_NAME, Pandragon.HERO_NAME };
        heroPools.Remove(playerChosenHero);

        int indexOne = rand.Next(0, heroPools.Count);
        enemyOneNPC = GetPlayerAnimalHero(heroPools[indexOne]);
        heroPools.RemoveAt(indexOne);

        int indexTwo = rand.Next(0, heroPools.Count);
        enemyTwoNPC = GetPlayerAnimalHero(heroPools[indexTwo]);
        heroPools.RemoveAt(indexTwo);

        int indexThree = rand.Next(0, heroPools.Count);
        enemyThreeNPC = GetPlayerAnimalHero(heroPools[indexThree]);
        heroPools.RemoveAt(indexThree);
    }

    private void CheckCurrentPlayerIsKnockedOut()
    {
        // Check if current player is knocked out
        if (currentTurnPlayer.isKnockedOut)
        {
            if (CheckCurrentPlayerVictory(currentTurnPlayer))
            {
                gameWon = true;
            }
            else
            {
                if (!deathCoroutineRunning && !reviveCouroutineRunning) 
                { 
                    StartCoroutine(ShowPlayerDeathText(() => 
                    { 
                        NextPlayerTurn();
                        deathCoroutineRunning = false;
                    }));
                }
            }
        }
    }

    private void CheckCurrentPlayerHandIsEmpty()
    {
        // Check if hands are empty
        if (currentTurnPlayer.playerHandDeck.GetAllCards().Count <= 0)
        {
            // Check if card deck are empty
            if (currentTurnPlayer.cardDeck.GetAllCards().Count <= 0)
            {
                currentTurnPlayer.cardDeck.AddCards(currentTurnPlayer.discardDeck.GetAllCards());
                currentTurnPlayer.cardDeck.ShuffleCards();

                currentTurnPlayer.discardDeck = new CardDeck();
            }

            currentTurnPlayer.playerHandDeck.AddCards(currentTurnPlayer.cardDeck.DrawCards(2));

            if (currentTurnPlayer == player)
            {
                UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
            }
        }
    }

    private bool CheckCurrentPlayerVictory(Player checkPlayer)
    {
        return (checkPlayer.knockoutCounter >= 5 || checkPlayer.relicCounter >= 3);
    }

    private void UpdateChestCardRemainingCardText(int remainingCards) 
    {
        remainingChestCardText.text = $"Remaining:\n{remainingCards}";
    }

    private void UpdateRemainingCardText(int remainingCards)
    {
        remainingCardText.text = $"Remaining:\n{remainingCards}";
    }

    private void UpdateRemainingCardBackImage(Sprite cardBackImage)
    {
        remainingCardImage.sprite = cardBackImage;
    }

    public void ShowPopupText(string message)
    {
        float duration = 1.5f;

        GameObject popupPrefab = Instantiate(popupTextPrefab, popupTextParent.transform);
        popupPrefab.GetComponent<TextMeshProUGUI>().text = message;

        Destroy(popupPrefab, duration);
    }

    private IEnumerator ShowPlayerDeathText(Action onComplete)
    {
        deathCoroutineRunning = true;

        // Delay 4 seconds if is AI, else player just show immediately
        if (currentTurnPlayer != player) 
        { 
            yield return new WaitForSeconds(4);    
        }

        // Show player is death text
        string[] currentPlayerString = { "You", "Enemy 1", "Enemy 2", "Enemy 3", "Hornterror" };
        int index = allCharacters.IndexOf(currentTurnPlayer);

        gameAnnouncementText.gameObject.SetActive(true);
        gameAnnouncementText.text = $"{currentPlayerString[index]} has knocked himself out!";

        // If player knock himself is different sentence
        if (index == 0)
        {
            gameAnnouncementText.text = $"{currentPlayerString[index]} have knocked yourself out!";
        }

        // Show for 4 second and close
        yield return new WaitForSeconds(4);
        gameAnnouncementText.gameObject.SetActive(false);
        gameAnnouncementText.text = "";

        onComplete?.Invoke();
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

    private void UpdateActionPointText(int actionPoint)
    {
        if (actionPoint >= 0)
        {
            actionPointLeftText.text = actionPoint.ToString();
        }
    }

    private void UpdatePlayerEndTurnButton()
    {
        endTurnButton.interactable = false;

        if (currentTurnPlayer == startingPlayer && !currentTurnPlayer.isKnockedOut)
        {
            endTurnButton.interactable = true;
        }
    }

    public void PlayerEndTurnButton()
    {
        if (ActionNotEndable(player.actionPoint))
        {
            Sound actionNotEndableSound = AudioManagerScript.instance.GetSound(SoundName.ActionNotEndable);

            if (!actionNotEndableSound.isPlaying)
            {
                AudioManagerScript.instance.Play(SoundName.ActionNotEndable);
            }

            ShowPopupText($"You still have {player.actionPoint} action points!");
        }
        else
        {
            AudioManagerScript.instance.Play(SoundName.EndTurnButton);
            
            if (chestCardDeck.GetAllCards().Count <= 0) 
            {
                chestCardDeck.AddCards(discardChestCardDeck.GetAllCards());
                chestCardDeck.ShuffleCards();

                discardChestCardDeck = new CardDeck();
            }
            List<Card> drawnCards = chestCardDeck.DrawCards(1);

            // Draw Relic Card
            if (drawnCards[0].CardType == CardType.Relic)
            {
                AudioManagerScript.instance.Play(SoundName.CollectRelicCard);

                StartCoroutine(ShowChestDrawnCardPanel(drawnCards[0], () =>
                {
                    // Stop relic sound
                    AudioManagerScript.instance.Stop(SoundName.CollectRelicCard);

                    // Resume action
                    ActivateCard(drawnCards[0]);
                    
                    // Check if player has won by collecting enough relic cards
                    if (CheckCurrentPlayerVictory(currentTurnPlayer))
                    {
                        ShowVictoryPanel(currentTurnPlayer);
                    }
                    else
                    {
                        NextPlayerTurn();
                    }
                }));
            }
            // Draw Trap Card
            else if (drawnCards[0].CardType == CardType.Trap)
            {
                AudioManagerScript.instance.Play(SoundName.ActivateTrap);

                StartCoroutine(ShowChestDrawnCardPanel(drawnCards[0], () => { }));
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

    private bool ActionNotEndable(int actionPoint)
    {
        return (actionPoint > 0);
    }
    
    private void NextPlayerTurn()
    {
        // Take the previous player index, iterate to the next one
        currentPlayerIndex = (currentPlayerIndex + 1) % allCharacters.Count;
        currentTurnPlayer = allCharacters[currentPlayerIndex]; // Already switch to next player
        
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

            // However, player is not available to move this turn, as reviving takes one turn
            StartCoroutine(UpdateGameTurn(() => {
                currentTurnPlayer.OnRevive();
                reviveCouroutineRunning = false;
                NextPlayerTurn();
            }));
            return;
        }

        StartCoroutine(UpdateGameTurn(() => { }));

        // Clear all active effects for the next player
        currentTurnPlayer.activeEffects.Clear();

        // Starting turn action for the next player
        currentTurnPlayer.actionPoint = currentTurnPlayer.animalHero.startingActionPoint;

        // Check if card decks are empty
        if (currentTurnPlayer.cardDeck.GetAllCards().Count <= 0)
        {
            currentTurnPlayer.cardDeck.AddCards(currentTurnPlayer.discardDeck.GetAllCards());
            currentTurnPlayer.cardDeck.ShuffleCards();

            currentTurnPlayer.discardDeck = new CardDeck();
        }
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

    private IEnumerator UpdateGameTurn(Action onComplete)
    {
        string[] currentPlayerString = { "You", "Enemy 1", "Enemy 2", "Enemy 3", "Hornterror" };
        int index = allCharacters.IndexOf(currentTurnPlayer);

        if (currentTurnPlayer == startingPlayer)
        {
            gameTurn++;

            if (!currentTurnPlayer.isKnockedOut) 
            { 
                AudioManagerScript.instance.Play(SoundName.EndTurnButton);
            }
        }

        currentTurnText.text = $"Turn: {gameTurn}";
        currentPlayerTurnText.text = currentPlayerString[index];

        foreach (GameObject glowImg in characterGlowImages)
        {
            glowImg.SetActive(false);
            glowImg.GetComponent<Image>().color = aliveColor;
        }
        characterGlowImages[index].SetActive(true);

        // If the turn's player were dead and is reviving
        if (currentTurnPlayer.isKnockedOut)
        {
            reviveCouroutineRunning = true;

            // Show player is reviving text and UI
            gameAnnouncementText.gameObject.SetActive(true);
            gameAnnouncementText.text = $"{currentPlayerString[index]} were knocked out and \nare reviving this turn!";
            characterGlowImages[index].GetComponent<Image>().color = deadColor;

            // Show for 4 second and proceed
            yield return new WaitForSeconds(4);
            gameAnnouncementText.gameObject.SetActive(false);
            gameAnnouncementText.text = "";
        }
        else 
        {
            yield return null;
        }

        onComplete?.Invoke();
    }

    public void CardOnDrop(int cardIndex)
    {
        Card playedCard = currentTurnPlayer.playerHandDeck.GetAllCards()[cardIndex];

        // Used and activate the card
        ActivateCard(playedCard);
        
        // Remove and discard the played card
        currentTurnPlayer.playerHandDeck.RemoveSingleCard(playedCard);
        HandleDiscardCard(playedCard);

        // Check if player's hand is empty or not
        CheckCurrentPlayerHandIsEmpty();
        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
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

                if (CheckCurrentPlayerVictory(player))
                {
                    ShowVictoryPanel(player);
                }
                break;

            case AbilityType.TargetAllCharacters:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, allCharacters);

                if (CheckCurrentPlayerVictory(player))
                {
                    ShowVictoryPanel(player);
                }
                break;

            case AbilityType.TargetAllPlayers:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, targetAllPlayers);

                if (CheckCurrentPlayerVictory(player))
                {
                    ShowVictoryPanel(player);
                }
                break;

            case AbilityType.TargetAllOpponents:
                playedCard.CardAbility.ActivateAbility(currentTurnPlayer, targetAllOpponents);

                if (CheckCurrentPlayerVictory(player))
                {
                    ShowVictoryPanel(player);
                }
                break;

            case AbilityType.AllCharacterSingleTargetable:
                StartCoroutine(ShowSinglePlayerSelectionPanel(currentTurnPlayer, false, playedCard, 1, () => 
                {
                    if (CheckCurrentPlayerVictory(player))
                    {
                        ShowVictoryPanel(player);
                    }
                }));
                break;

            case AbilityType.AllOpponentSingleTargetable:
                StartCoroutine(ShowSinglePlayerSelectionPanel(currentTurnPlayer, true, playedCard, 1, () => 
                { 
                    if (CheckCurrentPlayerVictory(player)) 
                    { 
                        ShowVictoryPanel(player); 
                    } 
                }));
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

    private IEnumerator PlayerAI(Player currentTurnPlayer)
    {
        System.Random rand = new System.Random();

        yield return new WaitForSeconds(1);

        // Use and activate card
        while (currentTurnPlayer.actionPoint > 0 && !currentTurnPlayer.isKnockedOut)
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
            AudioManagerScript.instance.Play(SoundName.PlayCard);

            // remove the card from hand and discard the card
            currentTurnPlayer.playerHandDeck.RemoveSingleCard(playedCard);
            HandleDiscardCard(playedCard);

            // check if current turn player's hand is empty
            CheckCurrentPlayerHandIsEmpty();

            // after show
            yield return StartCoroutine(ShowCardPlayedByAIPanel(playedCard));

            UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());

            yield return new WaitForSeconds(1);

            // check if current turn player won after playing the card
            if (CheckCurrentPlayerVictory(currentTurnPlayer))
            {
                ShowVictoryPanel(currentTurnPlayer);
                yield break;
            }
        }

        // After action point finish, draw a card from chest card deck
        // Hornterror is not required
        if (currentTurnPlayer == hornterrorNPC)
        {
            NextPlayerTurn();
            yield break;
        }
        
        // If player knock himself out, do not draw any more cards
        if (currentTurnPlayer.isKnockedOut) 
        {
            yield break;
        }

        if (chestCardDeck.GetAllCards().Count <= 0)
        {
            chestCardDeck.AddCards(discardChestCardDeck.GetAllCards());
            chestCardDeck.ShuffleCards();

            discardChestCardDeck = new CardDeck();
        }
        List<Card> drawnCards = chestCardDeck.DrawCards(1);

        // Drew Relic Card
        if (drawnCards[0].CardType == CardType.Relic)
        {
            AudioManagerScript.instance.Play(SoundName.CollectRelicCard);
            yield return StartCoroutine(ShowCardPlayedByAIPanel(drawnCards[0]));
            ActivateCardForAI(drawnCards[0]);

            if (CheckCurrentPlayerVictory(currentTurnPlayer))
            {
                ShowVictoryPanel(currentTurnPlayer);
            }
            else 
            { 
                NextPlayerTurn();
            }
        }
        // Drew Trap Card
        else if (drawnCards[0].CardType == CardType.Trap)
        {
            AudioManagerScript.instance.Play(SoundName.ActivateTrap);

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

        foreach (Player targetPlayer in targetAllEnemies)
        {
            if (targetPlayer.isKnockedOut || 
                targetPlayer.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption) ||
                (targetPlayer.shield >= 1 && targetPlayer.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin)))
            {
                continue;
            }

            if (targetPlayer.health <= 5)
            {
                prioritisedTarget.Add(targetPlayer);
                break;
            }
            else if (targetPlayer.relicCounter == 1)
            {
                prioritisedTarget.Add(targetPlayer);
                break;
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

    public void ShowViewCardPanel(int cardIndex)
    {
        AudioManagerScript.instance.Play(SoundName.ViewCard);

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

    public IEnumerator ShowSinglePlayerSelectionPanel(Player caster, bool allOpponent, Card playedCard, int targetCount, Action onComplete)
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
        
        onComplete?.Invoke();

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
        cardPlayedByAIPanel.SetActive(true);
        cardPlayedByAIImage.sprite = card.CardFrontSprite;

        SetCardPlayedByAIText(card);

        yield return new WaitForSeconds(4);
        CloseCardPlayedByAIPanel();
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

    public void CloseCardPlayedByAIPanel()
    {
        cardPlayedByAIPanel.SetActive(false);
        cardPlayedByAIImage.sprite = null;
    }

    public void ToggleHelpPanel()
    {
        if (helpPanel.activeSelf)
        {
            AudioManagerScript.instance.Play(SoundName.CloseHelpPanel);
        }
        else 
        {
            AudioManagerScript.instance.Play(SoundName.OpenHelpPanel);
        }

        helpPanel.SetActive(!helpPanel.activeSelf);
    }

    private void ShowVictoryPanel(Player victoryPlayer)
    {
        if (victoryPanel.activeSelf) 
        {
            return;
        }

        if (victoryPlayer == player)
        {
            StartCoroutine(AddToPlayerDatabase(victoryPlayer, 500, "win", () =>
            {
                victoryOkayButton.gameObject.SetActive(true);
            }));
        }
        else 
        {
            StartCoroutine(AddToPlayerDatabase(victoryPlayer, 250, "lost", () =>
            {
                victoryOkayButton.gameObject.SetActive(true);
            }));
        }

        AudioManagerScript.instance.Stop(SoundName.GameMatch);
        AudioManagerScript.instance.Play(SoundName.MatchVictory);

        string winMethod = "";

        // Check for the player's victory condition
        if (victoryPlayer.knockoutCounter >= 5)
        {
            winMethod = "knockout";
        }
        else if (victoryPlayer.relicCounter >= 3)
        {
            winMethod = "relics";
        }

        victoryPanel.SetActive(true);
        victoryPlayerImage.sprite = victoryPlayer.animalHero.animalHeroImage;

        string[] currentPlayerString = { "You", "Enemy 1 has", "Enemy 2 has", "Enemy 3 has" };
        int index = allCharacters.IndexOf(victoryPlayer);

        victoryPlayerText.text = $"{currentPlayerString[index]} won!";

        switch (winMethod)
        {
            case "knockout":
                victoryMethodText.text = $"Victory by {victoryPlayer.knockoutCounter} {winMethod}!";
                victoryByKnockoutImage.gameObject.SetActive(true);
                break;

            case "relics":
                victoryMethodText.text = $"Victory by {victoryPlayer.relicCounter} {winMethod}!";
                victoryByRelicImage.gameObject.SetActive(true);
                break;

            default: break;
        }
    }

    private IEnumerator AddToPlayerDatabase(Player victoryPlayer, int currencyReward, string winOrLost, Action onComplete) 
    {
        int playerCurrency = 0;
        List<string> matchHistories = new List<string>();
        int coinReward = currencyReward;

        // Get player current's currency
        var userTask = firebaseDBReference.Child("users").Child(userID).GetValueAsync();
        yield return new WaitUntil(() => userTask.IsCompleted);

        if (userTask.Exception != null)
        {
            Debug.LogWarning($"Failed to complete user task with error: {userTask.Exception} at {this.name}");
        }
        else
        {
            DataSnapshot userSnapshot = userTask.Result;

            // Update the coins
            playerCurrency = int.Parse(userSnapshot.Child("currency").Value.ToString());
            playerCurrency += coinReward;

            // Update the match history
            var playerMatchHistory = userSnapshot.Child("matchHistory");
            foreach (var matchHistory in playerMatchHistory.Children)
            {
                matchHistories.Add(matchHistory.Value.ToString());
            }

            matchHistories.Add(winOrLost);
        }

        // Update the database
        firebaseDBReference.Child("users").Child(userID).Child("currency").SetValueAsync(playerCurrency);
        firebaseDBReference.Child("users").Child(userID).Child("matchHistory").SetValueAsync(matchHistories);

        onComplete?.Invoke();
    }

    public void VictoryOkayButton()
    {
        AudioManagerScript.instance.Stop(SoundName.MatchVictory);
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);

        CloseVictoryPanel();

        SceneManager.LoadScene("MainMenuScene");
    }

    private void CloseVictoryPanel()
    {
        victoryPanel.SetActive(false);
        victoryByKnockoutImage.gameObject.SetActive(false);
        victoryByRelicImage.gameObject.SetActive(false);
        victoryOkayButton.gameObject.SetActive(false);
    }
}
