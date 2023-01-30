using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private int _ammoCount = 15;
    private float _speedBoost = 2f;
    private float _shieldHealth = 3f;
    SpriteRenderer _shieldColor;
    private float _masterSpeed;
    private float _horizontalInput;
    private float _verticalInput;
    [SerializeField]
    private float _fireRate = .45f;
    float _canFire = -1f;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpecialActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;
    private int _lives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private GameObject _tripleLaser;
    [SerializeField]
    private float _powerUpTime = 4f;
    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _audioFire;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
        transform.position = new Vector3(0, 0, 0);
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) Debug.LogError("AudioSource on the Player is NULL");
        _audioSource.clip = _audioFire;

        _shieldColor = _shieldPrefab.GetComponent<SpriteRenderer>();
        if (_shieldColor == null)
        {
            Debug.LogError("the SpriteRenderer on the Shield is NULL");
        }
    }

    void Update()
    {
        Movement();
        Fire();

    }

    void Fire()
    {
        if (Input.GetKey("space") && Time.time > _canFire && _ammoCount > 0)
        {
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
            _canFire = Time.time + _fireRate;
            if (_isSpecialActive)
            {
                _ammoCount = 15;
                _uiManager.UpdateAmmo(_ammoCount);
                GameObject _hLaser = Instantiate(_laser, transform.position + new Vector3(0, 1.08f, 0), Quaternion.identity);
                _hLaser.GetComponent<Laser>().Special();
            }
            else if (_isTripleShotActive && !_isSpecialActive)
            {

                Instantiate(_tripleLaser, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laser, transform.position + new Vector3(0, 1.08f, 0), Quaternion.identity);
            }

            _audioSource.Play();
        }
    }
    void Movement()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
        //Thruster effect, press shift to boost your speed
        _masterSpeed = _speed;
        _speedBoost = 1;

        if (_isSpeedActive)
        {
            _masterSpeed = _speed * 1.5f;
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speedBoost = 1.5f;

        }

        transform.Translate(Vector3.up * _verticalInput * (_masterSpeed * _speedBoost) * Time.deltaTime);
        transform.Translate(Vector3.right * _horizontalInput * (_masterSpeed * _speedBoost) * Time.deltaTime);



        if (transform.position.y < -3.97f)
        {
            transform.position = new Vector3(transform.position.x, -3.97f, 0);
        }
        else if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        if (transform.position.x > 11.27f)
        {
            transform.position = new Vector3(-11.27f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.27f)
        {
            transform.position = new Vector3(11.27f, transform.position.y, 0);
        }
    }
    public void ShieldCheck()
    {
        if (_isShieldActive)
        {
            _shieldHealth--;
            if (_shieldHealth == 2)
            {
                _shieldColor.color = Color.yellow;
                return;
            }
            else if (_shieldHealth == 1)
            {
                _shieldColor.color = Color.red;
                return;
            }
            else if (_shieldHealth <= 0)
            {
                _shieldPrefab.SetActive(false);
                _isShieldActive = false;
                return;
            }

        }
        _lives--;
    }
    public void Damage()
    {

        ShieldCheck();

        UpdateDamage();


    }
    public void AddScore(int score)
    {

        _score += score;
        _uiManager.UpdateScore(_score);

    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());

    }
    public void SpeedActive()
    {

        _isSpeedActive = true;
        StartCoroutine(SpeedPowerDown());
    }
    public void ShieldActive()
    {
        _shieldHealth = 3;
        _isShieldActive = true;
        _shieldPrefab.SetActive(true);
        _shieldColor.color = Color.white;
    }
    public void AmmoActive()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
    }
    public void SpecialActive()
    {
        _isSpecialActive = true;
        AmmoActive();
        StartCoroutine(SpecialPowerDown());
    }
    private void UpdateDamage()
    {
        if (_lives == 3)
        {
            _rightEngine.SetActive(false);
            _leftEngine.SetActive(false);
        }
        else if (_lives == 2)
        {
            _rightEngine.SetActive(true);
            _leftEngine.SetActive(false);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
            _rightEngine.SetActive(true);
        }
        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            _spawnManager.PlayerDied();

            Destroy(gameObject);
        }
    }
    public void HealthActive()
    {
        if (_lives < 3)
        {
            _lives++;
        }
        UpdateDamage();

    }
    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(_powerUpTime);
        _isTripleShotActive = false;
        StopCoroutine(TripleShotPowerDown());
    }
    IEnumerator SpecialPowerDown()
    {
        yield return new WaitForSeconds(5);
        _isSpecialActive = false;
        StopCoroutine(SpecialPowerDown());
    }
    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(_powerUpTime);
        _isSpeedActive = false;
        StopCoroutine(SpeedPowerDown());
    }
    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.tag == "Projectile" && collision.GetComponent<Laser>().WhoOwns() == 1)
        {
            Damage();
        }
    }
}



