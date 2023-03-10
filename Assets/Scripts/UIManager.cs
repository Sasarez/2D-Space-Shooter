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
    [SerializeField]private TextMeshProUGUI _waveText;
    [SerializeField] GameObject _healthBar;
    [SerializeField] GameObject _healthColorBar;
    [SerializeField] GameObject _bossBar;
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

  /*  private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }*/

    public void EnableBossBar()
    {
        _bossBar.SetActive(true);
    }
    public void WaveDisplay(int wave, int enemiestospawn, int enemieskilled)
    {
        if (wave != 10)
        {
            _waveText.text = "Wave " + wave + "     " + (enemiestospawn - enemieskilled) + "/" + enemiestospawn + " Enemies Left";
        } else
        {
            _waveText.text = "BOSS WAVE";
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

    public void UpdateBossHealth(float bossHealth, float maxHealth)
    {
        //.9 =9 health
        _healthBar.transform.localScale = new Vector3(bossHealth, 1, 1);
    }
    public void UpdateBossColor(Color bossColor)
    {
        _healthColorBar.GetComponent<Image>().color = bossColor;
    }
    public int GetAmmoMax() { return _ammoMax; }
    public void UpdateGas(float gas)
    {
        _thrusterSlider.value = gas;
    }
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
        PlayerPrefs.SetInt("score", score);
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
        if (currentLives <=3)
        {
            _livesImage.sprite = _livesSprites[currentLives];

        }
        
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
