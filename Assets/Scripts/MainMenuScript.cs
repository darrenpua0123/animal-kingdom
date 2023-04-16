using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
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
