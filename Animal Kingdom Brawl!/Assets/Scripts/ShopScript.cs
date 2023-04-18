using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    private int playerCoin = 30; // TODO: change to get from PLAYER DB 
    private bool piggionOwned = false; // DEV: change to check and get from PLAYER DB
    private string x_icon = "× ";

    private Piggion piggion;
    [Header("Piggion")]
    public TextMeshProUGUI piggionHeroName;
    public TextMeshProUGUI piggionHealth, piggionShield, piggionActionPoint, piggionHeroCost;
    public Button piggionBuyButton;

    private Catomic catomic;
    [Header("Catomic")]
    public TextMeshProUGUI catomicHeroName;
    public TextMeshProUGUI catomicHealth, catomicShield, catomicActionPoint, catomicHeroCost;

    private Pandragon pandragon;
    [Header("Pandragon")]
    public TextMeshProUGUI pandragonHeroName;
    public TextMeshProUGUI pandragonHealth, pandragonShield, pandragonActionPoint, pandragonHeroCost;

    private Beedle beedle;
    [Header("Beedle")]
    public TextMeshProUGUI beedleHeroName;
    public TextMeshProUGUI beedleHealth, beedleShield, beedleActionPoint, beedleHeroCost;

    [Header("Purchase Panel")]
    public GameObject purchasePanel;
    public TextMeshProUGUI purchasePanelText;


    void Awake()
    {
        playerCoin = 1000; // DEV: Use to instant get coins

        piggion = new Piggion();
        catomic = new Catomic();
        pandragon = new Pandragon();
        beedle = new Beedle();
    }

    void Start()
    {
        SetShopHeroUI();
    }

    void Update()
    {

    }

    private void SetShopHeroUI() {
        #region Piggion
        piggionHeroName.text = Piggion.HERO_NAME;
        piggionHealth.text = x_icon + piggion.initialHealth.ToString();
        piggionShield.text = x_icon + piggion.initialShield.ToString();
        piggionActionPoint.text = x_icon + piggion.initialActionPoint.ToString();
        piggionHeroCost.text = Piggion.SHOP_COST.ToString();
        #endregion

        #region Catomic
        catomicHeroName.text = Catomic.HERO_NAME;
        catomicHealth.text = x_icon + catomic.initialHealth.ToString();
        catomicShield.text = x_icon + catomic.initialShield.ToString();
        catomicActionPoint.text = x_icon + catomic.initialActionPoint.ToString();
        catomicHeroCost.text = Catomic.SHOP_COST.ToString();
        #endregion

        #region Pandragon
        pandragonHeroName.text = Pandragon.HERO_NAME;
        pandragonHealth.text = x_icon + pandragon.initialHealth.ToString();
        pandragonShield.text = x_icon + pandragon.initialShield.ToString();
        pandragonActionPoint.text = x_icon + pandragon.initialActionPoint.ToString();
        pandragonHeroCost.text = Pandragon.SHOP_COST.ToString();
        #endregion

        #region Beedle
        beedleHeroName.text = Beedle.HERO_NAME;
        beedleHealth.text = x_icon + beedle.initialHealth.ToString();
        beedleShield.text = x_icon + beedle.initialShield.ToString();
        beedleActionPoint.text = x_icon + beedle.initialActionPoint.ToString();
        beedleHeroCost.text = Beedle.SHOP_COST.ToString();
        #endregion

        UpdateShopUI();
    }

    private void UpdateShopUI() // TODO: Remodify this to fit more with DB
    {
        coinText.text = playerCoin.ToString();

        if (piggionOwned) {
            piggionBuyButton.interactable = false;
            piggionHeroCost.text = "Owned";
        }
        
    }

    public void PurchasePiggion()
    {
        if (playerCoin >= Piggion.SHOP_COST)
        {
            playerCoin -= Piggion.SHOP_COST; // TODO: Should update DB too
            piggionOwned = true; // DEV: Instantly owned piggion
            SetPurchasePanelText("sufficient");
        }
        else 
        {
            SetPurchasePanelText("insufficient");
        }

        UpdateShopUI();
        ShowPurchasePanel();
    }

    public void PurchaseCatomic() 
    { 
    
    }

    public void PurchasePandragon() 
    { 
    
    }

    public void PurchaseBeedle()
    {

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
