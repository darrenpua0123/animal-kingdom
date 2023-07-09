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
    public GameObject playerHandPanel;
    public TMP_Text actionPointLeftText;
    public Button endTurnButton;

    // Enemy 1
    public Image enemyOneHeroImage;
    public LifeHUB enemyOneLifeHUB;

    // Enemy 2
    public Image enemyTwoHeroImage;
    public LifeHUB enemyTwoLifeHUB;

    // Enemy 3
    public Image enemyThreeHeroImage;
    public LifeHUB enemyThreeLifeHUB;

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

    [Header("Prefabs")]
    public GameObject playerCardPrefab;

    [Header("Game Variables")]
    private int gameTurn = 0;
    public Player startingPlayer;
    private int currentPlayerIndex = 0;
    private CardDeck chestCardDeck;
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

        enemyTwoNPC.isKnockedOut = true;
        // TODO: testing area

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
        // TODO: Update turn should only happen when EndButton is clickable and turn endable
        // Haven't implement AI endturn function to call UpdateGameTurn();
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
        if (Input.GetKeyDown(KeyCode.G)) 
        {
            NextPlayerTurn();
            UpdateGameTurn();
        }

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

        UpdatePlayerEndTurnButton();
    }

    private void NextPlayerTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % allCharacters.Count;
        currentTurnPlayer = allCharacters[currentPlayerIndex];

        currentTurnPlayer.actionPoint = currentTurnPlayer.animalHero.startingActionPoint;
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
            // TODO: Update logic that happened after EndTurn, i.e. change currentTurnPlayer, increase turn etc
            List<Card> drawnCards = chestCardDeck.DrawCards(1);

            // TODO: Continue here: Implement if draw trap/relic card should StartCoroutine and WaitUntil card reinserted

            // If Trap Done / Not trap, do the normal action here
            player.playerHandDeck.AddCards(drawnCards);
            UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());
            
            NextPlayerTurn();
            UpdateGameTurn();
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

    public void ActivateCard(int cardIndex)
    {
        currentTurnPlayer.actionPoint--;

        Card playedCard = currentTurnPlayer.playerHandDeck.GetAllCards()[cardIndex];

        foreach (var abilityType in playedCard.CardAbility.abilityType)
        {
            List<Player> targetAllPlayers = new List<Player>(allCharacters);
            targetAllPlayers.Remove(currentTurnPlayer);
            targetAllPlayers.Remove(hornterrorNPC);

            switch (abilityType)
            {
                case AbilityType.NonTargetable:
                    playedCard.CardAbility.ActivateAbility(currentTurnPlayer, null);
                    break;

                case AbilityType.SingleTargetable:
                    ShowSinglePlayerSelectionPanel();
                    StartCoroutine(WaitForPlayerSelection(playedCard, currentTurnPlayer, 1));
                    break;

                case AbilityType.TargetAllPlayers:
                    playedCard.CardAbility.ActivateAbility(currentTurnPlayer, targetAllPlayers);
                    break;

                // unused yet
                case AbilityType.TargetAllCharacters:
                    playedCard.CardAbility.ActivateAbility(currentTurnPlayer, allCharacters);
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
        
        for (int i = 0; i < enemySelectionButton.Count; i++)
        {
            int buttonValue = i;
            enemySelectionButton[i].onClick.AddListener(() => OnPlayerSelectionButton(buttonValue));
        }
        
        yield return new WaitUntil(() => selectedPlayers.Count == targetCount);
        
        playedCard.CardAbility.ActivateAbility(caster, selectedPlayers);
        CloseSinglePlayerSelectionPanel();
        UpdateCardsInPlayerHandPanel(player.playerHandDeck.GetAllCards());

        // Internal function
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

    public void ShowSinglePlayerSelectionPanel()
    {
        singlePlayerSelectionPanel.SetActive(true);

        List<Player> allEnemies = new List<Player>(allCharacters);
        allEnemies.Remove(currentTurnPlayer);

        for (int i = 0; i < allEnemies.Count; i++)
        {
            enemySelectionButton[i].image.sprite = allEnemies[i].animalHero.animalHeroImage;
          
            if (allEnemies[i].isKnockedOut) 
            {
                enemySelectionButton[i].interactable = false;
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

}
