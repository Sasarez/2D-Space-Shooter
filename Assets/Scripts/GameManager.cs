using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    public void GameOver()
    {
        _isGameOver= true;

    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyUp(KeyCode.Escape))
            {
            Application.Quit();
            }
    }
}

