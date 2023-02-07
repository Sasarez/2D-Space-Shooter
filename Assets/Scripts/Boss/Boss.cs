using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] Sprite[] _bossSprites;
    [SerializeField] int _bossState;
    SpawnManager _spawnManager;
    bool _bossEntered = false;
    bool _repairing = false;
    float _speed = 2f;
    bool _left = false;
    float _movingSpeed;
    int _bossDamageDealt;
    int _bossMaxHealth = 6;
    int _bossCurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        _bossCurrentHealth = _bossMaxHealth;
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

        _bossDamageDealt = _bossMaxHealth - _bossCurrentHealth;
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
            Invoke("RepairBoss", 3f);
        }
    }
    void RepairBoss()
    {
        _bossState= 0;
        _repairing = false;
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
                    _bossCurrentHealth--;
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
        }
    }
}
