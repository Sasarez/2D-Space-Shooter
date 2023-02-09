using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadInstructions(bool state)
    {
        _panel.SetActive(state);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
