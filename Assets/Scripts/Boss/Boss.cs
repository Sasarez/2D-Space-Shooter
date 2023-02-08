using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Boss : MonoBehaviour
{
    [SerializeField] Sprite[] _bossSprites;
    [SerializeField] int _bossState;
    SpawnManager _spawnManager;
    [SerializeField] GameObject _laserPrefab;
    [SerializeField] GameObject _bombPrefab;
    bool _bossEntered = false;
    bool _repairing = false;
    float _speed = 2f;
    bool _left = false;
    float _movingSpeed;
    UIManager _uiManager;
    float _bossMaxHealth = 1f;
    float _bossCurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        

        _bossCurrentHealth = _bossMaxHealth;
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager on the Boss is NULL");
        }
        //_uiManager.UpdateBossHealth(_bossMaxHealth, _bossCurrentHealth);
        //_uiManager.UpdateBossColor(Color.black);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("the Spawn Manager is NULL");
        }
    }

  

    void BossMovementEntrance()
    {
        if (this.transform.position.y < 3.31)
        {
            _bossEntered = true;
            _movingSpeed = 1.5f;
            StartCoroutine("BossBasicAttack");
            StartCoroutine("BossSpecialAttack");

        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
    }

    void BossMovement()
    {

        // boss X boundaries are -4.4, and 4.4
        if (_left)
        {
            transform.Translate(Vector3.left * _movingSpeed * Time.deltaTime);
            if (transform.position.x < -4.4)
            {
                _left = false;
                _movingSpeed = Random.Range(1.5f, 2f);
            }
        }
        else
        {
            transform.Translate(Vector3.right * _movingSpeed * Time.deltaTime);
            if (transform.position.x > 4.4)
            {
                _left = true;
                _movingSpeed = Random.Range(1.5f, 2f);
            }
        }
        

    }

    IEnumerator BossBasicAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            GameObject bLaser1 = Instantiate(_laserPrefab, transform.position + new Vector3(1.44f, -2.0f, 0), Quaternion.identity);
            GameObject bLaser2 = Instantiate(_laserPrefab, transform.position + new Vector3(-1.5f, -2.0f, 0), Quaternion.identity);
            bLaser1.GetComponent<Laser>().EnemyOwned();
            bLaser1.transform.localScale = new Vector3(.3f, .3f, 0);
            bLaser2.GetComponent<Laser>().EnemyOwned();
            bLaser2.transform.localScale = new Vector3(.3f, .3f, 0); ;
            bLaser1.GetComponent<SpriteRenderer>().color = Color.green;
            bLaser2.GetComponent<SpriteRenderer>().color = Color.green;
            
        }
       
        
    }

    IEnumerator BossSpecialAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(8f);
            GameObject _bomb = Instantiate(_bombPrefab, transform.position + new Vector3(-3.74f, -1.5f, 0), Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
            GameObject _bomb3 = Instantiate(_bombPrefab, transform.position + new Vector3(0, -3.0f, 0), Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
            GameObject _bomb2 = Instantiate(_bombPrefab, transform.position + new Vector3(3.14f, -1.5f, 0), Quaternion.identity);
            
        }
    }

    public void UpdateBossState(int bossState)
    {
        _bossState= bossState;
    }

    public int GetBossState()
    {
        return _bossState;
    }
    // Update is called once per frame
    void Update()
    {

        
        UpdateBossSprite(_bossState);
        if (!_bossEntered)
        {
            BossMovementEntrance();
        } else
        {
            BossMovement();
        }
        if (_bossState == 3 && _repairing == false)
        {
            _repairing = true;
            _uiManager.UpdateBossColor(Color.red);
            Invoke("RepairBoss", 5f);
        }
    }
    void RepairBoss()
    {
        _bossState= 0;
        _repairing = false;
        _uiManager.UpdateBossColor(Color.blue);
    }
    void UpdateBossSprite(int state)
    {
        this.transform.GetComponent<SpriteRenderer>().sprite = _bossSprites[state];
    }

    void BossCheckDeath()
    {
        if (_bossCurrentHealth <= 0)
        {
            _spawnManager.BossSlain();
            Destroy(this.gameObject);
        }
    }

    void NormalBossColor()
    {
        transform.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other!=null)
        {
            if (other.tag == "Projectile" && other.GetComponent<Laser>().WhoOwns()==0)
            {
                if (_bossState == 3)
                {
                    _bossCurrentHealth = _bossCurrentHealth - .1f;
                    _uiManager.UpdateBossHealth(_bossCurrentHealth, _bossMaxHealth);
                    
                    transform.GetComponent<SpriteRenderer>().color = Color.red;
                    BossCheckDeath();
                }
                else
                {
                    transform.GetComponent<SpriteRenderer>().color = Color.blue;
                    
                }
                Invoke("NormalBossColor", .1f);

                Destroy(other.gameObject);
            }
            if (other.tag == "Player")
            {
                other.GetComponent<Player>().Damage();
            }
        }
    }
}
