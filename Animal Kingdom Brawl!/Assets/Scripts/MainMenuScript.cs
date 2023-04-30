using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    void Start()
    {
        // TODO: Once get the ID, create player class here from DATABASE
        string id = PlayerPrefs.GetString(LoginScript.PLAYER_ID_KEY);    
        Debug.Log(id + " for testing purpose");
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
