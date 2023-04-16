using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopScript : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    private int number = 30;
    private string x_icon = "× ";

    private Piggion piggion;
    [Header("Piggion")]
    public TextMeshProUGUI piggionHeroName;
    public TextMeshProUGUI piggionHealth, piggionShield, piggionActionPoint, piggionHeroCost;

    private Catomic catomic;
    [Header("Catomic")]
    public TextMeshProUGUI catomicHeroName;
    public TextMeshProUGUI catomicHealth, catomicShield, catomicActionPoint, catomicHeroCost;

    private Piggion pandragon;
    [Header("Pandragon")]
    public TextMeshProUGUI pandragonHeroName;
    public TextMeshProUGUI pandragonHealth, pandragonShield, pandragonActionPoint, pandragonHeroCost;

    private Piggion beedle;
    [Header("Beedle")]
    public TextMeshProUGUI beedleHeroName;
    public TextMeshProUGUI beedleHealth, beedleShield, beedleActionPoint, beedleHeroCost;


    void Awake()
    {
        number = 20;

        piggion = new Piggion();
        
    }

    void Start()
    {
        coinText.text = number.ToString();

        piggionHeroName.text = Piggion.HERO_NAME;
        piggionHealth.text = x_icon + piggion.initialHealth.ToString();
        piggionShield.text = x_icon + piggion.initialShield.ToString();
        piggionActionPoint.text = x_icon + piggion.initialActionPoint.ToString();
        piggionHeroCost.text = Piggion.SHOP_COST.ToString();
    }

    void Update()
    {
        // TODO: Add in the panel for purchase successful or failed
    }

    public void BackToMainMenu() 
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
