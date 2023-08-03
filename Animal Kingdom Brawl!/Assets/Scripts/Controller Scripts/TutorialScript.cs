using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    public List<GameObject> pagePanels;
    private int pageIndex = 0;

    void Start()
    {
        ShowPagePanel(pageIndex);
    }

    private void ShowPagePanel(int index)
    {
        foreach (GameObject panel in pagePanels)
        {
            panel.SetActive(false);
        }

        pagePanels[index].SetActive(true);
    }

    public void NextPageButton()
    {
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);

        pageIndex = (pageIndex + 1) % pagePanels.Count;
        ShowPagePanel(pageIndex);
    }

    public void PreviousPageButton()
    {
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);

        pageIndex = (pageIndex - 1 + pagePanels.Count) % pagePanels.Count;
        ShowPagePanel(pageIndex);
    }

    public void BackToMainMenu()
    {
        AudioManagerScript.instance.Play(SoundName.ButtonPressed);

        SceneManager.LoadScene("MainMenuScene");
    }
}
