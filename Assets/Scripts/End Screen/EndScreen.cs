using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    int _playerScore;
    
    void Start()
    {
        _scoreText.text = "Your Final Score Was: " + _playerScore.ToString();
    }


    private void OnEnable()
    {       
            _playerScore = PlayerPrefs.GetInt("score");  
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
