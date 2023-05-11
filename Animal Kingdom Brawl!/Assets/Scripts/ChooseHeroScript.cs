using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseHeroScript : MonoBehaviour
{
    private string userID;
    private List<string> unlockedAnimalHeroes = new List<string>();

    [Header("Firebase")]
    public FirebaseAuth auth;
    public DatabaseReference firebaseDBReference;
    // TODO: Finish creating dynamically adding heroes selectioon from the DATABASE and pass them to gamescene

    void Awake()
    {
        InitialiseFirebase();

        userID = auth.CurrentUser.UserId;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void InitialiseFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        firebaseDBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
