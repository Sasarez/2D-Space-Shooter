using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditorInternal;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    private UIManager _uiManager;
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private int _enemyType;
    [SerializeField] private Vector3 _spawnLocation;
    [SerializeField] private Vector3 _spawnRotation;
    [SerializeField] private int _wave =7;
    [SerializeField] private int _enemiesSpawned;
    [SerializeField] private int[] _enemyTypesToSpawn;
    [SerializeField] private float _minSpawn = 2f;
    [SerializeField] private float _maxSpawn = 4f;
    [SerializeField] private bool _enemiesHaveShields = false;
    [SerializeField] GameObject[] _enemies;
    private bool _spawnerTest = false;
    private int _enemiesSlain;
    private int _enemiesToSpawn = 4;
    private int _enemiesAlive;
    bool _bossSpawned = false;
    private bool playerDied = false;
    private bool _bossWave = false;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager on the Spawn Manager is NULL");
        }
        _wave++;
        WaveStart(_wave);
        StartCoroutine("SpawnPowerup");

    }
    void WaveStart(int wave)
    {

        //Enemy Types
        //0 top to bottom,
        //1 left to right,
        //2 right to left,
        //3 side to side on top, lasers fired directly toward player
        //4 same as 0 except they will attempt to ram the player,
        //5 same as 0 except they will fire behind if the player is behind
        //6 same as 0 except they will attempt to dodge the players shots
        //7 is our boss
        switch (wave)
        {
            case int i when (i > 0 && i <= 2):
                _enemyTypesToSpawn[0] = 1;

               
                break;
            case int i when (i > 2 && i <= 4):
                _enemyTypesToSpawn[1] = 1;
                _enemyTypesToSpawn[2] = 1;
                _maxSpawn = 3.5f;
                break;
            case int i when (i > 4 && i <= 6):
                _enemiesHaveShields = true;
                _enemyTypesToSpawn[4] = 1;
                _enemyTypesToSpawn[5] = 1;
                break;
            case int i when (i > 6 && i < 10):
                _enemyTypesToSpawn[3] = 1;
                _enemyTypesToSpawn[6] = 1;
                break;
            case 10:
                for (int i=0; i < _enemyTypesToSpawn.Length; i++)
                {
                    _enemyTypesToSpawn[i] = 0;
                    
                }
                _enemyTypesToSpawn[7] = 1;
                _bossWave = true;
                _enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in _enemies)
                    {
                        Destroy(enemy);
                    }
                
                break;
            /*case int i when (i >= 8 && i < 12):
                _enemyTypesToSpawn[2] = 1;
                _minSpawn = 1.5f;
                break;
            case int i when (i >= 12 && 1 < 18):
                _enemyTypesToSpawn[0] = 1;
                break;*/
        }
        if (_spawnerTest)
        {
            _enemiesToSpawn = 500;
            _minSpawn = .5f;
            _maxSpawn = 1f;

        }
        else
        {
            _enemiesToSpawn = 4 + wave;
        }
       if (_wave == 10)
        {
            _enemiesToSpawn = 0;
        }

        for (int i = 0; i <= _enemyTypesToSpawn.Length; i++)
        {

            if (_enemyTypesToSpawn[i] == 1)
            {

                _uiManager.WaveDisplay(_wave, _enemiesToSpawn, 0);
               
                    SpawnEnemyPreperation();
                
                
                break;
            }
        }

       

    }
    void SpawnEnemyPreperation(int enemyType = -1)
    {
        
    
        if ((_enemiesSpawned < _enemiesToSpawn) || _bossWave)
        {
            ChooseAgain:
            if (enemyType == -1)
            {
                enemyType = Random.Range(0, _enemyTypesToSpawn.Length);
            } else if(_bossSpawned)
            {
               
                _enemyTypesToSpawn[enemyType] = 1;
            }
            

            if (enemyType == 0 && _enemyTypesToSpawn[enemyType] == 1)
            {
                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);
                _spawnRotation = new Vector3(0, 0, 0);
            }
            else if (enemyType == 1 && _enemyTypesToSpawn[enemyType] == 1)
            {
                _spawnLocation = new Vector3(-9.2f, Random.Range(-3.4f, 6.2f));
                _spawnRotation = new Vector3(0, 0, 90);
            }
            else if (enemyType == 2 && _enemyTypesToSpawn[enemyType] == 1)
            {
                _spawnLocation = new Vector3(9.2f, Random.Range(-3.4f, 6.2f));
                _spawnRotation = new Vector3(0, 0, -90);
            }
            else if (enemyType == 3 && _enemyTypesToSpawn[enemyType] == 1)
            {

                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 5.64f, 0);
                _spawnRotation = new Vector3(0, 0, 0);

            }
            else if (enemyType ==4 && _enemyTypesToSpawn[enemyType] == 1)
            {
                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);
                _spawnRotation = new Vector3(0, 0, 0);
            }
            else if (enemyType == 5 && _enemyTypesToSpawn[enemyType] == 1)
            {
                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);
                _spawnRotation = new Vector3(0, 0, 0);
            }
            else if (enemyType == 6 && _enemyTypesToSpawn[enemyType] == 1)
            {
                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);
                _spawnRotation = new Vector3(0, 0, 0);
            }
            else if (enemyType == 7 && _enemyTypesToSpawn[enemyType] == 1)
            {
                _spawnLocation = new Vector3(0, 12, 0);
                //_spawnRotation = new Vector3(0, 0, 0);
                
            }
            else
            {
                enemyType = -1;
                goto ChooseAgain;
            }
            _enemyType= enemyType;
            
            Invoke("SpawnEnemy", Random.Range(_minSpawn, _maxSpawn));

        }

    }

    void SpawnEnemy()
    {
        if (playerDied)
        {
            _enemiesToSpawn = 0;
            return;
        }
     
        
        
            if ((_enemiesSpawned < _enemiesToSpawn) || _bossSpawned)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, _spawnLocation, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                newEnemy.transform.eulerAngles = _spawnRotation;
                newEnemy.transform.GetComponent<Enemy>().SetEnemyType(_enemyType);


                if (_enemiesHaveShields == true)
                {
                    int i = Random.Range(0, 50);
                    if (i > 0 && i < 20)
                    {
                        newEnemy.transform.GetComponent<Enemy>().SetEnemyShield(true);
                    }
                }
                _enemiesSpawned++;
                if (_bossWave)
                {

                }
                else
                {

                SpawnEnemyPreperation();
                }
            

        } if (_bossSpawned==false && _bossWave)
        {
            GameObject Boss = Instantiate(_bossPrefab, _spawnLocation, Quaternion.identity);
            Boss.transform.parent = _enemyContainer.transform;
            _bossSpawned = true;
            //StartCoroutine("BossSpawn");
            
        }
        
     

    }
    void CheckDeathCount()
    {
        if (_bossWave)
        {

        }
        else if (_enemiesSlain >= _enemiesToSpawn)
        {

            _wave++;
            _enemiesSlain = 0;
            _enemiesSpawned = 0;
            WaveStart(_wave);
        }
        else if (_enemiesSlain <= _enemiesToSpawn)
        {
            _uiManager.WaveDisplay(_wave, _enemiesToSpawn, _enemiesSlain);
        } 
    }
    public void EnemySlain()
    {
        _enemiesSlain++;
        CheckDeathCount();
    }
    public void BossSlain()
    {
        Debug.Log("end the game");
        StopCoroutine("BossSpawn");
    }

    IEnumerator BossSpawn()
    {
        while (true)
        {
        yield return new WaitForSeconds(Random.Range(2f, 4f));
            _enemiesAlive = _enemiesSpawned - _enemiesSlain;
            Debug.Log("Enemies Alive " + _enemiesAlive);
            if (_enemiesAlive >= 2)
            {
                
            }
            else
            {
               
                SpawnEnemyPreperation(3);
            }
            
           
        }
    }
    IEnumerator SpawnPowerup()
    {
        while (!playerDied)
        {
            if (_spawnerTest)
            {
                yield return new WaitForSeconds(Random.Range(.5f, 1f) + 1);
            } else
            {
                yield return new WaitForSeconds(Random.Range(4f, 7f) + 1);
            }
            
            int _randomPowerup = Random.Range(0, 72);


            Instantiate(_powerups[SelectPowerup(_randomPowerup)], new Vector3(Random.Range(-8.5f, 8.5f), 8, 0), Quaternion.identity);

        }
    }

    int SelectPowerup(int powerup)
    {
        switch (powerup)
        {
            case int i when (i > 0 && i <= 10): //Triple Shot
                return 0;
            case int i when (i > 10 && i <= 20): //Speed
                return 1;
            case int i when (i > 20 && i <= 27)://Shield
                return 2;
            case int i when (i > 27 && i <= 50)://Ammo
                return 3;
            case int i when (i > 50 && i <= 57)://Health
                return 4;
            case int i when (i > 57 && i <= 62)://Special
                return 5;
            case int i when (i > 62 && i <= 72)://Slow
                return 6;
        }
        return 0;
    }
    public void PlayerDied()
    {

        playerDied = true;
    }
}
