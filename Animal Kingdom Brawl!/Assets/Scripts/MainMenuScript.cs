using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public static readonly string PLAYER_CURRENCY_KEY = "playerCurrency";
    public static readonly string PLAYER_UNLOCKED_HEROES_KEY = "playerUnlockedHeroes";

    [Header("Firebase")]
    private DatabaseReference firebaseDBReference = FirebaseDatabase.DefaultInstance.RootReference;

    private string userID;
    private string username;
    private string currency;
    public List<string> unlockedAnimalHeroes = new List<string>();

    [Header("Welcome Text")]
    public TMP_Text welcomeText;

    void Awake()
    {
        userID = PlayerPrefs.GetString(LoginScript.PLAYER_ID_KEY);
    }

    void Start()
    {
        StartCoroutine(LoadUserData());
    }

    private IEnumerator LoadUserData() 
    {
        var userTask = firebaseDBReference.Child("users").Child(userID).GetValueAsync();
        yield return new WaitUntil(predicate: () => userTask.IsCompleted);

        if (userTask.Exception != null)
        {
            Debug.LogWarning($"Failed to complete username task with error: {userTask.Exception}");
        }
        else 
        {
            DataSnapshot userSnapshot = userTask.Result;

            username = userSnapshot.Child("username").Value.ToString();
            UpdateWelcomeText(username);

            // Check for player's currency on whether the player is first time logging in
            var playerCurrency = userSnapshot.Child("currency");
            if (!playerCurrency.Exists)
            {
                firebaseDBReference.Child("users").Child(userID).Child("currency").SetValueAsync(GameData.DefaultStartingCurrency);
            }
            else 
            {
                currency = playerCurrency.Value.ToString();
            }

            // Check for player's unlocked animal heroes on whether the player is first time logging in
            var playerAnimalHeroes = userSnapshot.Child("unlockedAnimalHeroes");
            if (!playerAnimalHeroes.Exists)
            {
                firebaseDBReference.Child("users").Child(userID).Child("unlockedAnimalHeroes").SetValueAsync(GameData.DefaultHeroes);
            }
            else 
            {
                foreach (var hero in playerAnimalHeroes.Children) {
                    unlockedAnimalHeroes.Add(hero.Value.ToString());
                }
            }

            PlayerPrefs.SetString(PLAYER_CURRENCY_KEY, currency);
            // TODO: Optimize this code
            string unlockedAnimalHeroesAsString = string.Join("#",unlockedAnimalHeroes);
            PlayerPrefs.SetString(PLAYER_UNLOCKED_HEROES_KEY, unlockedAnimalHeroesAsString);
        }
    }

    private void UpdateWelcomeText(string username)
    {
        welcomeText.text = $"Welcome back, <br><b>{username}</b>";
    }

    public void PlayGame() 
    {
        // TODO: transition animation tutorial
        // https://www.youtube.com/watch?v=CE9VOZivb3I
        SceneManager.LoadScene("GameScene");
    }

    public void ViewTutorial()
    {
        Debug.Log("Tutorial Page");
    }

    public void ViewShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void QuitGame() 
    {
        Debug.Log("Quit");
        Application.Quit();
    }


}
