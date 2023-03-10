using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{

    private ScreenShake _screenShake;
    [SerializeField]
    private float _speed = 5f;

    private int _ammoCount = 30;
    private float _speedBoost = 2f;
    private float _thrusterGas = 200f;
    private float _shieldHealth = 3f;
    SpriteRenderer _shieldColor;
    private float _masterSpeed;
    private float _horizontalInput;
    private float _verticalInput;
    [SerializeField]
    private float _fireRate = .45f;
    float _canFire = -1f;
    Vector3 _direction;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpecialActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;
    private bool _isSlowActive = false;
    private bool _godMode = false;
    private bool _invulnerable = false;
    private bool _playerDead;
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
    [SerializeField] GameObject _explosionPrefab;
    private GameObject[] _powerUpObjects;
    private GameObject[] _powerDownObjects;
    private GameObject[] _allPowerObjects;
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
        _screenShake = GameObject.Find("ScreenShake").GetComponent<ScreenShake>();
        if (_screenShake == null)
        {
            Debug.Log("Screen Shake is NULL");
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
        _uiManager.AmmoInitiate(_ammoCount);
        
    }

    void Update()
    {
        if (_playerDead) return;
        PowerUpVacuum();
        ThrusterCoolDown();
        Movement();
        Fire();
        if (_godMode)
        {
            _score = 0;
            _uiManager.UpdateScore(_score);
            _ammoCount = 30;
            _uiManager.UpdateAmmo(_ammoCount);
        }

    }

    void PowerUpVacuum()
    {
        if (Input.GetKey(KeyCode.C))
        {
            _powerUpObjects = GameObject.FindGameObjectsWithTag("Powerup");
            _powerDownObjects = GameObject.FindGameObjectsWithTag("PowerupNegative");
            _allPowerObjects = _powerUpObjects.Concat(_powerDownObjects).ToArray();
            if (_allPowerObjects != null)
                foreach (var obj in _allPowerObjects)
                {
                    Debug.Log(obj.name);
                    _direction = (transform.position - obj.transform.position).normalized;
                    obj.transform.position += _direction * 6.5f * Time.deltaTime;
                }
        }
    }
    void ThrusterCoolDown()
    {
        if (_thrusterGas <= 0)
        {
            StartCoroutine("ThrusterCoolingDown");

        }
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
                _ammoCount = _uiManager.GetAmmoMax();
                _uiManager.UpdateAmmo(_ammoCount);
                GameObject _hLaser = Instantiate(_laser, transform.position + new Vector3(0, 1.08f, 0), Quaternion.identity);
                _hLaser.GetComponent<SpriteRenderer>().color = Color.yellow;
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
        if (_isSlowActive)
        {
            _masterSpeed = _speed / 2;
        }
        if (_isSpeedActive)
        {
            _masterSpeed = _speed * 1.5f;
        }


        if (Input.GetKey(KeyCode.LeftShift) && _thrusterGas > 0)
        {
            _thrusterGas -= 1f;
            _uiManager.UpdateGas(_thrusterGas);
            _speedBoost = 1.5f;

        }

        transform.Translate(Vector3.up * _verticalInput * (_masterSpeed * _speedBoost) * Time.deltaTime);
        transform.Translate(Vector3.right * _horizontalInput * (_masterSpeed * _speedBoost) * Time.deltaTime);



        if (transform.position.y < -3.97f)
        {
            transform.position = new Vector3(transform.position.x, -3.97f, 0);
        }
        else if (transform.position.y > 4.4f)
        {
            transform.position = new Vector3(transform.position.x, 4.4f, 0);
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
        _screenShake.ShakeScreen(1, .2f, 2f);
        if (!_godMode)
        {
            if (!_invulnerable)
            {
                _lives--;
                _invulnerable= true;
                StartCoroutine("InvulnerabilityFrames");
            }
            
        }
        
    }
    IEnumerator InvulnerabilityFrames()
    {

        transform.GetComponent<SpriteRenderer>().color = Color.blue;
        yield return new WaitForSeconds(1.5f);
        transform.GetComponent<SpriteRenderer>().color = Color.white;        
        _invulnerable = false;
        StopCoroutine("InvulnerabilityFrames");
        
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
        _ammoCount = _uiManager.GetAmmoMax();
        _uiManager.UpdateAmmo(_ammoCount);
    } 
    public void SpecialActive()
    {
        _isSpecialActive = true;
        AmmoActive();
        StartCoroutine(SpecialPowerDown());
    }
    public void SlowActive()
    {
        _isSlowActive = true;
        Invoke("SlowPowerDown", 5f);
    }
    void SlowPowerDown()
    {
        _isSlowActive = false;
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
            PlayerDeath();
            _playerDead = true;
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
    void PlayerDeath()
    {
        GameObject _explosion = Instantiate(_explosionPrefab, new Vector3 (transform.position.x, transform.position.y-1.5f,0),Quaternion.identity);
        
        _explosion.GetComponent<SpriteRenderer>().sortingOrder = 100;
        Destroy(_explosion, 2f);
        Destroy(this.gameObject, 1f);
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
    IEnumerator ThrusterCoolingDown()
    {
        yield return new WaitForSeconds(5f);
        _thrusterGas = 200;
        _uiManager.UpdateGas(_thrusterGas);
        StopCoroutine("ThrusterCoolingDown");
    }
    private void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.tag == "Projectile" && collision.GetComponent<Laser>().WhoOwns() == 1)
        {
            Destroy(collision.gameObject);
            Damage();
        }
        if (collision.tag == "BombExplosion")
        {
            Damage();
        }
    }
}



