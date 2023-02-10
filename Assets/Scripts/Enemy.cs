using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{

    Animator _enemyAnim;

    AudioSource _audioSource;
    [SerializeField]float _speed = 3.5f;
    private SpawnManager _spawnManager;
   
    private Player _player;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _audioLaser;
    private Vector2 _spawnPosition;
    private float _distance;
    private Vector3 _laserRotation;
    [SerializeField] private GameObject _enemyShield;
    [SerializeField] private GameObject _explosionPrefab;
    GameObject _explosion;
    private bool _movingLeft;
    private bool _hasShield;
    [SerializeField] private bool _isInvincible = false;
    private Vector3 _direction;
    
    private bool _enemyDying;
    private bool _ramming;
    private bool _powerupDetected;
    private Vector3 _originalUp;
   
    private bool _detectionCooldown;
    [SerializeField]
    private int _enemyType; //0 top to bottom, 1 left to right, 2 right to left
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager= GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if ( _spawnManager == null ) 
        {
            Debug.Log("the Spawn Manager is NULL");
        }
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.Log("The Player is NULL");
        if (_enemyType== 0)
        {
            _enemyAnim = GetComponent<Animator>();
            if (_enemyAnim == null) Debug.Log("The Animator is NULL");
        }
        
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) Debug.LogError("AudioSource on the Enemy is NULL");
        

        
        

        
            StartCoroutine("EnemyFire");
        
        
    }

    public bool IsEnemyDying()
    {
        return _enemyDying;
    }
    public void SetInvincible(bool invincible)
    {
        _isInvincible=invincible;
    }

    public void SetEnemyType(int type)
    {
        _enemyType = type;
    }
    public int GetEnemyType()
    {
        return _enemyType;
    }

    public void SetEnemyShield(bool shield)
    {
        if (shield)
        {
            _enemyShield.SetActive(true);
            _hasShield = true;
        }
    }


    Vector3 GetPlayerLocation()
    {
        if (_player == null)
        {
            return Vector3.zero;
        }
        return _player.transform.position;

    }

    public void RamPlayer(Vector3 playerLocation)
    {
        if (!_ramming && !_enemyDying)
        {
            
            _direction = (playerLocation - transform.position).normalized;
            _originalUp = transform.up;
            transform.up = playerLocation - transform.position;
            transform.localRotation *= Quaternion.Euler(0, 0, 180);

            _ramming = true;
        }
        else
        {        
            if (!_enemyDying)
            {             
                transform.position += _direction * 5f * Time.deltaTime;
            }          
            if (_distance > 3f)
            {
                _ramming = false;
                if (_originalUp != null)
                {
                    transform.up = _originalUp;
                }
            }
        }
       
    }
    public void EnemyMovement()
    {
        if (_enemyType == 3)
        {
            Debug.Log("Yes were running this");
            if (transform.position.x <= -9)
            {
                _movingLeft = false;
            }
            else if (transform.position.x >= 9)
            {
                _movingLeft = true;
            }
            if (_movingLeft)
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }
            else if (_movingLeft == false)
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }

            return;
        }
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.41)
        {
            transform.position = new Vector2(Random.Range(-8.5f, 8.5f), 8);
        }
        else if (transform.position.x < -9.2f)
        {
            transform.position = new Vector2(9.2f, Random.Range(-3.4f, 6.2f));
        }
        else if (transform.position.x > 10f)
        {
            transform.position = new Vector2(-9.2f, Random.Range(-3.4f, 6.2f));
        }

        
    }
   
    void Update()
    {
        _distance = Vector3.Distance(GetPlayerLocation(), this.transform.position);

        if ((_distance <= 3f || _ramming) && _enemyType == 4 && !_enemyDying)
        {
            RamPlayer(GetPlayerLocation());

        }

        if (!_ramming)
        
        {
            
            EnemyMovement();
            ObjectRaycastCheck();

        }

    }


  




    void ObjectRaycastCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 5f, LayerMask.GetMask("Powerup"));

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Powerup" && !_detectionCooldown && !_enemyDying)
            {
                _powerupDetected = true;
              

                
                SetUpLaser();
            }
        }
    }





    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasShield && other.tag == "Projectile" && other.GetComponent<Laser>().WhoOwns()==0)
        {
            _hasShield = false;
            _enemyShield.SetActive(false);
            Destroy(other.gameObject);
            return;
        } else if (_hasShield && other.tag == "Player")
        {
           
            _enemyShield.SetActive(false);
            _hasShield = false;
            return;
        }
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {

                player.Damage();
                _audioSource.Play();
            }
            else
            {
                Debug.LogError("PLAYER IS NULL!");
            }
            if (!_isInvincible)
            {
                _enemyDying = true;
                if (_enemyType == 0)
                {
                    _enemyAnim.SetTrigger("OnEnemy0Death");
                }
                else
                {
                    _explosion = Instantiate(_explosionPrefab, transform.position,Quaternion.identity);
                    _explosion.transform.localScale = transform.localScale;
                }
                
                _speed = 0;
                StopCoroutine("EnemyFire");
                this.GetComponent<Collider2D>().enabled = false;
                _spawnManager.EnemySlain();
                if (_explosion!= null)
                    Destroy(_explosion, 2.5f);
                if (_enemyType == 0)
                {
                    Destroy(gameObject, 2.5f);
                }
                else
                {
                    Destroy(gameObject, 1f);
                }
                
            }
            
        }
        if (other.tag == "Projectile" && other.GetComponent<Laser>().WhoOwns() == 0)
        {
             

            if (_player != null)
            {
                _player.AddScore(10);

            }
            else
            {
                Debug.Log("_player is NULL");
            }
            if (!_isInvincible)
            {
                _enemyDying = true;
                Destroy(other.gameObject);
                if (_enemyType == 0)
                {
                    _enemyAnim.SetTrigger("OnEnemy0Death");
                }
                else
                {
                    _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                    _explosion.transform.localScale = transform.localScale;
                }


                StopCoroutine("EnemyFire");
                _speed = 0;
                _audioSource.Play();

                this.tag = "Undefined";
                this.GetComponent<Collider2D>().enabled = false;
                _spawnManager.EnemySlain();
                if (_explosion != null)
                    Destroy(_explosion, 2.5f);
                if (_enemyType == 0)
                {
                    Destroy(gameObject, 2.5f);
                }
                else
                {
                    Destroy(gameObject, 1.0f);
                }
                
            }
           
        }

    }


    void PowerUpDetectionCooldown()
    {
        _detectionCooldown = false;
    }
    void SetUpLaser()
    {
        switch (_enemyType)
        {
            case 0:
                _spawnPosition = new Vector2(transform.position.x, transform.position.y - 1.5f);
                _laserRotation = new Vector3(0, 0, 0);
                break;
            case 1:
                _spawnPosition = new Vector2(transform.position.x + 1.5f, transform.position.y);
                _laserRotation = new Vector3(0, 0, 90);
                break;
            case 2:
                _spawnPosition = new Vector2(transform.position.x - 1.5f, transform.position.y);
                _laserRotation = new Vector3(0, 0, -90);
                break;
            case 5:
                
                _spawnPosition = new Vector2(transform.position.x, transform.position.y);
                _laserRotation = new Vector3(0, 0, 0);
                break;
            default:
                _spawnPosition = new Vector2(transform.position.x, transform.position.y - 1.5f);
                _laserRotation = new Vector3(0, 0, 0);
                break;
        }
        if (!_ramming)
        {
            if (_enemyType == 5 && GetPlayerLocation().y > transform.position.y && !_powerupDetected)
            {
                _spawnPosition = new Vector2(transform.position.x, transform.position.y + 1.5f);
                _laserRotation = new Vector3(0, 0, 180);
            }
            if (_powerupDetected)
            {
                GameObject qLaser = Instantiate(_laserPrefab, _spawnPosition, Quaternion.identity);
                qLaser.GetComponent<Laser>().EnemyOwned();
                qLaser.GetComponent<SpriteRenderer>().color = Color.green;
                qLaser.transform.eulerAngles = _laserRotation;
                _powerupDetected = false;
                _detectionCooldown = true;
               
                Invoke("PowerUpDetectionCooldown", 1.5f);
            }
        }
    }

    void FireLaser()
    {
        if (!_powerupDetected)
        {
            GameObject laser = Instantiate(_laserPrefab, _spawnPosition, Quaternion.identity);


            laser.GetComponent<Laser>().EnemyOwned();
            if (_enemyType == 3)
            {
                laser.GetComponent<Laser>().AlternateFire(GetPlayerLocation());
            }


            laser.transform.eulerAngles = _laserRotation;

            AudioSource.PlayClipAtPoint(_audioLaser, new Vector3(0, 1, -10), .4f);
           
        }
        
    }
    
    private IEnumerator EnemyFire()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(Random.Range(2, 4));
            SetUpLaser();
            FireLaser();
            
               
            
           
        }
    }
}
