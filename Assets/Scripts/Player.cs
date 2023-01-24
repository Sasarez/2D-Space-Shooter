using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    float horizontalInput;
    float verticalInput;
    [SerializeField]
    private float _fireRate = .45f;
    private float _oldSpeed;
    [SerializeField]
    private float _powerUpSpeed = 8f;
    float _canFire = -1f;
    [SerializeField]
    private bool _isTripleShotActive = false;
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
    // Start is called before the first frame update
    void Start()
    {
        _uiManager= GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if ( _uiManager == null )
        {
            Debug.Log("UI Manager is NULL");
        }
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetKey("space") && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            if (_isTripleShotActive)
            {

                Instantiate(_tripleLaser, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laser, transform.position + new Vector3(0, 1.08f, 0), Quaternion.identity);
            }

        }
    }

    void Movement()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        if (_isSpeedActive)
        {
            transform.Translate(Vector3.up * verticalInput * _powerUpSpeed * Time.deltaTime);
            transform.Translate(Vector3.right * horizontalInput * _powerUpSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        }
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
    public void Damage()
    {
        if (_isShieldActive)
        {
            _shieldPrefab.SetActive(false);
            _isShieldActive = false;
            return;
        }
        _lives--;
        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            _spawnManager.PlayerDied();
            Destroy(gameObject);
        }
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
        _isShieldActive = true;
        _shieldPrefab.SetActive(true);
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(_powerUpTime);
        _isTripleShotActive = false;
        StopCoroutine(TripleShotPowerDown());
    }
    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(_powerUpTime);
        _isSpeedActive = false;
        StopCoroutine(SpeedPowerDown());
    }
}


