using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    int page = 1;
    int maxPage = 5;
    int minPage = 1;

	public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void TutorialButton()
    {
        SceneManager.LoadScene(3);
    }

    public void ExitButton()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void NextPage()
    {
        if (page != maxPage)
        {

            GameObject.Find("Page" + page).SetActive(false);
            page++;
            Debug.Log("Page" + page);
            GameObject.Find("Page" + page).SetActive(true);
        }
    }

    public void PreviousPage()
    {
        if (page != minPage)
        {
            GameObject.Find("Page" + page).SetActive(false);
            page--;
            GameObject.Find("Page" + page).SetActive(true);
        }
    }
}
