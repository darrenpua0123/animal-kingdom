using Firebase.Database;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    private string userID;
    private int playerCurrency;
    private List<string> unlockedAnimalHeroes = new List<string>();

    public TextMeshProUGUI coinText;
    private readonly string x_icon = "× ";

    [Header("Firebase")]
    private DatabaseReference firebaseDBReference = FirebaseDatabase.DefaultInstance.RootReference;

    private Piggion piggion;
    [Header("Piggion")]
    public TextMeshProUGUI piggionHeroName;
    public TextMeshProUGUI piggionHealth, piggionShield, piggionActionPoint, piggionHeroCost;
    public Button piggionBuyButton;

    private Catomic catomic;
    [Header("Catomic")]
    public TextMeshProUGUI catomicHeroName;
    public TextMeshProUGUI catomicHealth, catomicShield, catomicActionPoint, catomicHeroCost;
    public Button catomicBuyButton;

    private Pandragon pandragon;
    [Header("Pandragon")]
    public TextMeshProUGUI pandragonHeroName;
    public TextMeshProUGUI pandragonHealth, pandragonShield, pandragonActionPoint, pandragonHeroCost;
    public Button pandragonBuyButton;

    private Beedle beedle;
    [Header("Beedle")]
    public TextMeshProUGUI beedleHeroName;
    public TextMeshProUGUI beedleHealth, beedleShield, beedleActionPoint, beedleHeroCost;
    public Button beedleBuyButton;

    [Header("Purchase Panel")]
    public GameObject purchasePanel;
    public TextMeshProUGUI purchasePanelText;

    void Awake()
    {
        userID = PlayerPrefs.GetString(LoginScript.PLAYER_ID_KEY);
        playerCurrency = int.Parse(PlayerPrefs.GetString(MainMenuScript.PLAYER_CURRENCY_KEY));
        unlockedAnimalHeroes = PlayerPrefs.GetString(MainMenuScript.PLAYER_UNLOCKED_HEROES_KEY).Split(new[] {"#"}, StringSplitOptions.None).ToList();

        piggion = new Piggion();
        catomic = new Catomic();
        pandragon = new Pandragon();
        beedle = new Beedle();
    }

    void Start()
    {
        SetShopHeroUI();
    }

    private void SetShopHeroUI() {
        #region Piggion
        piggionHeroName.text = Piggion.HERO_NAME.FirstCharacterToUpper();
        piggionHealth.text = x_icon + piggion.initialHealth.ToString();
        piggionShield.text = x_icon + piggion.initialShield.ToString();
        piggionActionPoint.text = x_icon + piggion.initialActionPoint.ToString();
        piggionHeroCost.text = GameData.PiggionShopCost.ToString();
        #endregion

        #region Catomic
        catomicHeroName.text = Catomic.HERO_NAME.FirstCharacterToUpper();
        catomicHealth.text = x_icon + catomic.initialHealth.ToString();
        catomicShield.text = x_icon + catomic.initialShield.ToString();
        catomicActionPoint.text = x_icon + catomic.initialActionPoint.ToString();
        catomicHeroCost.text = GameData.CatomicShopCost.ToString();
        #endregion

        #region Pandragon
        pandragonHeroName.text = Pandragon.HERO_NAME.FirstCharacterToUpper();
        pandragonHealth.text = x_icon + pandragon.initialHealth.ToString();
        pandragonShield.text = x_icon + pandragon.initialShield.ToString();
        pandragonActionPoint.text = x_icon + pandragon.initialActionPoint.ToString();
        pandragonHeroCost.text = GameData.PandragonShopCost.ToString();
        #endregion

        #region Beedle
        beedleHeroName.text = Beedle.HERO_NAME.FirstCharacterToUpper();
        beedleHealth.text = x_icon + beedle.initialHealth.ToString();
        beedleShield.text = x_icon + beedle.initialShield.ToString();
        beedleActionPoint.text = x_icon + beedle.initialActionPoint.ToString();
        beedleHeroCost.text = GameData.BeedleShopCost.ToString();
        #endregion

        UpdateShopUI();
    }

    private void UpdateShopUI() 
    {
        coinText.text = playerCurrency.ToString();

        foreach (var hero in unlockedAnimalHeroes) {
            if (hero.Equals(Piggion.HERO_NAME))
            {
                piggionBuyButton.interactable = false;
                piggionHeroCost.text = "Owned";
            }
            else if (hero.Equals(Catomic.HERO_NAME)) 
            {
                catomicBuyButton.interactable = false;
                catomicHeroCost.text = "Owned";
            }
            else if (hero.Equals(Pandragon.HERO_NAME))
            {
                pandragonBuyButton.interactable = false;
                pandragonHeroCost.text = "Owned";
            }
            else if (hero.Equals(Beedle.HERO_NAME))
            {
                beedleBuyButton.interactable = false;
                beedleHeroCost.text = "Owned";
            }
        }
    }

    public void PurchasePiggionButton()
    {
        int piggionCost = GameData.PiggionShopCost;

        if (playerCurrency >= piggionCost)
        {
            playerCurrency -= piggionCost; 
            firebaseDBReference.Child("users").Child(userID).Child("currency").SetValueAsync(playerCurrency);

            unlockedAnimalHeroes.Add(Piggion.HERO_NAME);
            firebaseDBReference.Child("users").Child(userID).Child("unlockedAnimalHeroes").SetValueAsync(unlockedAnimalHeroes);

            SetPurchasePanelText("sufficient");
        }
        else 
        {
            SetPurchasePanelText("insufficient");
        }

        UpdateShopUI();
        ShowPurchasePanel();
    }

    public void PurchaseCatomicButton() 
    {
        int catomicCost = GameData.CatomicShopCost;

        if (playerCurrency >= catomicCost)
        {
            playerCurrency -= catomicCost;
            firebaseDBReference.Child("users").Child(userID).Child("currency").SetValueAsync(playerCurrency);

            unlockedAnimalHeroes.Add(Catomic.HERO_NAME);
            firebaseDBReference.Child("users").Child(userID).Child("unlockedAnimalHeroes").SetValueAsync(unlockedAnimalHeroes);

            SetPurchasePanelText("sufficient");
        }
        else
        {
            SetPurchasePanelText("insufficient");
        }

        UpdateShopUI();
        ShowPurchasePanel();
    }

    public void PurchasePandragonButton() 
    {
        int pandragonCost = GameData.PandragonShopCost;

        if (playerCurrency >= pandragonCost)
        {
            playerCurrency -= pandragonCost;
            firebaseDBReference.Child("users").Child(userID).Child("currency").SetValueAsync(playerCurrency);

            unlockedAnimalHeroes.Add(Pandragon.HERO_NAME);
            firebaseDBReference.Child("users").Child(userID).Child("unlockedAnimalHeroes").SetValueAsync(unlockedAnimalHeroes);

            SetPurchasePanelText("sufficient");
        }
        else
        {
            SetPurchasePanelText("insufficient");
        }

        UpdateShopUI();
        ShowPurchasePanel();
    }

    public void PurchaseBeedleButton()
    {
        int beedleCost = GameData.BeedleShopCost;

        if (playerCurrency >= beedleCost)
        {
            playerCurrency -= beedleCost;
            firebaseDBReference.Child("users").Child(userID).Child("currency").SetValueAsync(playerCurrency);

            unlockedAnimalHeroes.Add(Beedle.HERO_NAME);
            firebaseDBReference.Child("users").Child(userID).Child("unlockedAnimalHeroes").SetValueAsync(unlockedAnimalHeroes);

            SetPurchasePanelText("sufficient");
        }
        else
        {
            SetPurchasePanelText("insufficient");
        }

        UpdateShopUI();
        ShowPurchasePanel();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ShowPurchasePanel() 
    {
        purchasePanel.SetActive(true);
    }

    private void SetPurchasePanelText(string condition) 
    {
        string purchaseSuccessul = "Purchase Succesful! <br><br> Thank you!";
        string purchaseUnsuccessful = "Purchase Unsuccessful! <br><br> Insufficient Fund!";
        string unknown = "UNKNOWN";

        switch (condition)
        {
            case "sufficient":
                purchasePanelText.text = purchaseSuccessul;
                break;

            case "insufficient":
                purchasePanelText.text = purchaseUnsuccessful;
                break;

            default:
                purchasePanelText.text = unknown;
                break;
        }
    }

    public void ClosePurchasePanel() 
    {
        purchasePanel.SetActive(false);
    }
}
