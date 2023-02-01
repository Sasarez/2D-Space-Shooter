using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Enemy : MonoBehaviour
{

    Animator _enemyAnim;

    AudioSource _audioSource;
    float _speed = 3.5f;
    private SpawnManager _spawnManager;
    private Player _player;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _audioLaser;
    private Vector2 _spawnPosition;
    private Vector3 _laserRotation;
    [SerializeField]
    private int _enemyEntrance; //0 top to bottom, 1 left to right, 2 right to left
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
        _enemyAnim = GetComponent<Animator>();
        if (_enemyAnim == null) Debug.Log("The Animator is NULL");
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) Debug.LogError("AudioSource on the Enemy is NULL");
        StartCoroutine("EnemyFire");
    }

    public void SetEnemyType(int type)
    {
        _enemyEntrance = type;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.41)
        {
            transform.position = new Vector2(Random.Range(-8.5f, 8.5f), 8);
        } else if (transform.position.x < -9.2f)
        {
            transform.position = new Vector2(9.2f, Random.Range(-3.4f, 6.2f));
        } else if (transform.position.x > 10f)
        {
            transform.position = new Vector2(-9.2f, Random.Range(-3.4f, 6.2f));
        }
        
        

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
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

            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            StopCoroutine("EnemyFire");
            this.GetComponent<BoxCollider2D>().enabled = false;
            _spawnManager.EnemySlain();
            Destroy(gameObject, 2.5f);
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
            Destroy(other.gameObject);
            _enemyAnim.SetTrigger("OnEnemyDeath");

            StopCoroutine("EnemyFire");
            _speed = 0;
            _audioSource.Play();
            
            this.tag = "Undefined";
            this.GetComponent<BoxCollider2D>().enabled = false;
            _spawnManager.EnemySlain();
            Destroy(gameObject, 2.5f);
        }

    }
    private IEnumerator EnemyFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2, 4));
            switch (_enemyEntrance)
            {
                case 0:
                    _spawnPosition = new Vector2(transform.position.x, transform.position.y -1.5f);
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
            }
            
            GameObject laser = Instantiate(_laserPrefab, _spawnPosition, Quaternion.identity);
            
            laser.GetComponent<Laser>().EnemyOwned();
            laser.transform.eulerAngles = _laserRotation;
            
            AudioSource.PlayClipAtPoint(_audioLaser, new Vector3(0, 1, -10), .4f);
        }
    }
}
