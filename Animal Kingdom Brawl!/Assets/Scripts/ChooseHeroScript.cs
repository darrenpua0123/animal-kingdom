using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseHeroScript : MonoBehaviour
{
    private string userID;
    // TODO: Finish creating dynamically adding heroes selectioon from the DATABASE and pass them to gamescene
    void Awake()
    {
        userID = PlayerPrefs.GetString(LoginScript.PLAYER_ID_KEY);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
