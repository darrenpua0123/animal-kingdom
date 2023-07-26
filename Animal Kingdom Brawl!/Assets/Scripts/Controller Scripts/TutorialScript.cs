using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO: Make tutorial scene
    public void BackToMainMenu()
    {
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);

        SceneManager.LoadScene("MainMenuScene");
    }
}
