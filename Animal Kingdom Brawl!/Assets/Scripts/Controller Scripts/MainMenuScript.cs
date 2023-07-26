using Firebase.Auth;
using Firebase.Database;
using System;
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
        Sound mainMenuSound = AudioManagerScript.instance.GetSound(SoundName.MainMenu);

        if (!mainMenuSound.isPlaying)
        {
            AudioManagerScript.instance.Play(SoundName.MainMenu);     
        }

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
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);
        // TODO: transition animation tutorial
        // https://www.youtube.com/watch?v=CE9VOZivb3I
        SceneManager.LoadScene("ChooseHeroScene");
    }

    public void ViewTutorial()
    {
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);

        SceneManager.LoadScene("TutorialScene");
    }

    public void ViewShop()
    {
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);

        SceneManager.LoadScene("ShopScene");
    }

    public void QuitGame()
    {
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);

        auth.SignOut();

        Application.Quit();
    }
}
