using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _ammoText;
    private int _ammoMax;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    [SerializeField]
    private TextMeshProUGUI _restartText;
    private bool _isGameOver;
    private bool _gameOverFlicker;
    private GameManager _gameManager;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
        
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AmmoInitiate(int ammo)
    {
        _ammoMax = ammo;
        UpdateAmmo(ammo);
    }
    public int GetAmmoMax() { return _ammoMax; }
    public void UpdateGas(float gas)
    {
        _thrusterSlider.value = gas;
    }
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateAmmo(int ammo)
    {

        _ammoText.text = "" + ammo + "/" + _ammoMax;
        
        if (ammo < (_ammoMax / 2) +1 ) _ammoText.color = Color.yellow;
        if (ammo < _ammoMax/4) _ammoText.color = Color.red;
        if (ammo > (_ammoMax/2) + 1) _ammoText.color = Color.white;
    }


    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];
        if (currentLives == 0)
        {
            _isGameOver = true;
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            StartCoroutine("GameOverFlicker");
            _gameManager.GameOver();
        }
    }
    IEnumerator GameOverFlicker()
    {
        while (_isGameOver)
        {
            yield return new WaitForSeconds(.3f);
            _gameOverText.gameObject.SetActive(_gameOverFlicker);
            _gameOverFlicker = !_gameOverFlicker;

        }





    }
}
