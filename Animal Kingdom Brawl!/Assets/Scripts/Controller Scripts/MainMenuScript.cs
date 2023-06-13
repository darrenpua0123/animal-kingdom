using Firebase.Auth;
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
    private string username;

    [Header("Firebase")]
    public FirebaseAuth auth;
    public DatabaseReference firebaseDBReference;

    [Header("Welcome Text")]
    public TMP_Text welcomeText;

    void Awake()
    {
        InitialiseFirebase();

        username = auth.CurrentUser.DisplayName;
    }

    void Start()
    {
        UpdateWelcomeText(username);
    }

    private void InitialiseFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        firebaseDBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void UpdateWelcomeText(string username)
    {
        welcomeText.text = $"Welcome back, <br><b>{username}</b>";
    }

    public void PlayGame() 
    {
        // TODO: transition animation tutorial
        // https://www.youtube.com/watch?v=CE9VOZivb3I
        SceneManager.LoadScene("ChooseHeroScene");
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
        //auth.SignOut();

        Application.Quit();
    }


}
