using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void BackToMainMenu() => SceneManager.LoadScene(0);

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
