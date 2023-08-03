using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseHeroScript : MonoBehaviour
{
    private readonly string x_icon = " × ";

    private string userID;
    private List<string> unlockedAnimalHeroes = new List<string>();

    [Header("Firebase")]
    public FirebaseAuth auth;
    public DatabaseReference firebaseDBReference;

    [Header("Animal Hero Panel")] 
    // GO = GameObject. EX: piggionGO = Piggion Game Object
    public GameObject piggionGO;
    public GameObject catomicGO;
    public GameObject pandragonGO;
    public GameObject beedleGO;

    private Piggion piggion;
    [Header("Piggion")]
    public TMP_Text piggionHeroName;
    public TMP_Text piggionHealth, piggionShield, piggionActionPoint;

    private Catomic catomic;
    [Header("Catomic")]
    public TMP_Text catomicHeroName;
    public TMP_Text catomicHealth, catomicShield, catomicActionPoint;

    private Pandragon pandragon;
    [Header("Pandragon")]
    public TMP_Text pandragonHeroName;
    public TMP_Text pandragonHealth, pandragonShield, pandragonActionPoint;

    private Beedle beedle;
    [Header("Beedle")]
    public TMP_Text beedleHeroName;
    public TMP_Text beedleHealth, beedleShield, beedleActionPoint;

    [Header("Hero Trait Panel")]
    public GameObject heroTraitPanel;
    public Image heroTraitImage;
    public TMP_Text heroTraitText;


    void Awake()
    {
        InitialiseFirebase();

        userID = auth.CurrentUser.UserId;

        piggion = new Piggion();
        catomic = new Catomic();
        pandragon = new Pandragon();
        beedle = new Beedle();

        StartCoroutine(SetPlayerUnlockedHeroesFromDB());
    }

    private void InitialiseFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        firebaseDBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private IEnumerator SetPlayerUnlockedHeroesFromDB() 
    {
        var userTask = firebaseDBReference.Child("users").Child(userID).GetValueAsync();
        yield return new WaitUntil(predicate: () => userTask.IsCompleted);

        if (userTask.Exception != null)
        {
            Debug.LogWarning($"Failed to complete user task with error: {userTask.Exception} at {this.name}");
        }
        else
        {
            DataSnapshot userSnapshot = userTask.Result;

            var playerUnlockedAnimalHeroes = userSnapshot.Child("unlockedAnimalHeroes");
            foreach (var hero in playerUnlockedAnimalHeroes.Children)
            {
                unlockedAnimalHeroes.Add(hero.Value.ToString());
            }

            SetChooseHeroSelection();
        }
    }

    private void SetChooseHeroSelection() 
    {
        foreach (var hero in unlockedAnimalHeroes) 
        {
            if (hero.Equals(Piggion.HERO_NAME)) {
                piggionGO.SetActive(true);
                SetHeroSelectionUI(Piggion.HERO_NAME);
            }
            else if (hero.Equals(Catomic.HERO_NAME))
            {
                catomicGO.SetActive(true);
                SetHeroSelectionUI(Catomic.HERO_NAME);
            }
            else if (hero.Equals(Pandragon.HERO_NAME))
            {
                pandragonGO.SetActive(true);
                SetHeroSelectionUI(Pandragon.HERO_NAME);
            }
            else if (hero.Equals(Beedle.HERO_NAME))
            {
                beedleGO.SetActive(true);
                SetHeroSelectionUI(Beedle.HERO_NAME);
            }
        }
    }

    private void SetHeroSelectionUI(string heroName)
    {
        if (heroName.Equals(Piggion.HERO_NAME))
        {
            #region Piggion
            piggionHeroName.text = Piggion.HERO_NAME.FirstCharacterToUpper();
            piggionHealth.text = x_icon + piggion.defaultHealth.ToString();
            piggionShield.text = x_icon + piggion.defaultShield.ToString();
            piggionActionPoint.text = x_icon + piggion.startingActionPoint.ToString();
            #endregion
        }
        else if (heroName.Equals(Catomic.HERO_NAME))
        { 
            #region Catomic
            catomicHeroName.text = Catomic.HERO_NAME.FirstCharacterToUpper();
            catomicHealth.text = x_icon + catomic.defaultHealth.ToString();
            catomicShield.text = x_icon + catomic.defaultShield.ToString();
            catomicActionPoint.text = x_icon + catomic.startingActionPoint.ToString();
            #endregion
        }
        else if (heroName.Equals(Pandragon.HERO_NAME))
        {
            #region Pandragon
            pandragonHeroName.text = Pandragon.HERO_NAME.FirstCharacterToUpper();
            pandragonHealth.text = x_icon + pandragon.defaultHealth.ToString();
            pandragonShield.text = x_icon + pandragon.defaultShield.ToString();
            pandragonActionPoint.text = x_icon + pandragon.startingActionPoint.ToString();
            #endregion
        }
        else if (heroName.Equals(Beedle.HERO_NAME))
        {
            #region Beedle
            beedleHeroName.text = Beedle.HERO_NAME.FirstCharacterToUpper();
            beedleHealth.text = x_icon + beedle.defaultHealth.ToString();
            beedleShield.text = x_icon + beedle.defaultShield.ToString();
            beedleActionPoint.text = x_icon + beedle.startingActionPoint.ToString();
            #endregion
        }
    }

    public void ShowHeroTraitPanel(string heroName) 
    {
        AudioManagerScript.instance.Play(SoundName.OpenHelpPanel);

        heroTraitPanel.SetActive(true);

        heroName.ToLower();
        if (heroName.Equals(Piggion.HERO_NAME))
        {
            heroTraitImage.sprite = piggion.animalHeroImage;
            heroTraitText.text = piggion.heroTrait;
        }
        else if (heroName.Equals(Catomic.HERO_NAME))
        {
            heroTraitImage.sprite = catomic.animalHeroImage;
            heroTraitText.text = catomic.heroTrait;
        }
        else if (heroName.Equals(Pandragon.HERO_NAME))
        {
            heroTraitImage.sprite = pandragon.animalHeroImage;
            heroTraitText.text = pandragon.heroTrait;
        }
        else if (heroName.Equals(Beedle.HERO_NAME))
        {
            heroTraitImage.sprite = beedle.animalHeroImage;
            heroTraitText.text = beedle.heroTrait;
        }
    }

    public void CloseHeroTraitPanel() 
    {
        AudioManagerScript.instance.Play(SoundName.CloseHelpPanel);

        heroTraitPanel.SetActive(false);
    }

    public void ChooseHero(string heroName) 
    {
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);

        string chosenHero = null;

        if (heroName.Equals(Piggion.HERO_NAME))
        {
            chosenHero = Piggion.HERO_NAME;
        }
        else if (heroName.Equals(Catomic.HERO_NAME))
        {
            chosenHero = Catomic.HERO_NAME;
        }
        else if (heroName.Equals(Pandragon.HERO_NAME)) 
        {
            chosenHero = Pandragon.HERO_NAME;
        }
        else if (heroName.Equals(Beedle.HERO_NAME))
        {
            chosenHero = Beedle.HERO_NAME;
        }
        
        PlayerPrefs.SetString("ChosenHero", chosenHero);

        LoadGameScene();
    }

    public void LoadGameScene() 
    {
        SceneManager.LoadScene("Gamescene");
    }

    public void BackToMainMenu()
    {
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);
        SceneManager.LoadScene("MainMenuScene");
    }
}
