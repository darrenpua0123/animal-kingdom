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

    // TODO: Check all the TODO Test, to make sure 3) is working
    //2) test run one game
    //3) fixed card deck issue ? try test one game and see hornterror still bug or not
    //4) consider add when Revive text? Like "Enemy 1 has been revived this turn!"

    //TODO: Make tutorial scene
    public void BackToMainMenu()
    {
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);

        SceneManager.LoadScene("MainMenuScene");
    }
}
