using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditorInternal;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    private UIManager _uiManager;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private int _enemyType;
    [SerializeField] private Vector3 _spawnLocation;
    [SerializeField] private Vector3 _spawnRotation;
    [SerializeField] private int _wave;
    [SerializeField] private int _enemiesSpawned;
    [SerializeField] private int[] _enemyTypesToSpawn;
    [SerializeField] private float _minSpawn = 2f;
    [SerializeField] private float _maxSpawn = 4f;
    [SerializeField] private bool _enemiesHaveShields = false;
    private int _enemiesSlain;
    private int _enemiesToSpawn = 4;
    private bool playerDied = false;
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
        switch (wave)
        {
            case int i when (i > 0 && i <= 4):
                _enemyTypesToSpawn[5] = 1;
               // _enemiesHaveShields = true;
                break;
            case int i when (i > 4 && i < 8):
                _enemyTypesToSpawn[1] = 1;
                _maxSpawn = 3.5f;
                break;
            case int i when (i >= 8 && i < 12):
                _enemyTypesToSpawn[2] = 1;
                _minSpawn = 1.5f;
                break;
            case int i when (i >= 12 && 1 < 18):
                _enemyTypesToSpawn[3] = 1;
                break;
        }
        //_enemiesToSpawn = 4 + wave;
        _enemiesToSpawn = 1;

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
    void SpawnEnemyPreperation()
    {
        
    chooseAgain:
        if (_enemiesSpawned < _enemiesToSpawn)
        {
            _enemyType = Random.Range(0, _enemyTypesToSpawn.Length);

            if (_enemyType == 0 && _enemyTypesToSpawn[_enemyType] == 1)
            {
                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);
                _spawnRotation = new Vector3(0, 0, 0);
            }
            else if (_enemyType == 1 && _enemyTypesToSpawn[_enemyType] == 1)
            {
                _spawnLocation = new Vector3(-9.2f, Random.Range(-3.4f, 6.2f));
                _spawnRotation = new Vector3(0, 0, 90);
            }
            else if (_enemyType == 2 && _enemyTypesToSpawn[_enemyType] == 1)
            {
                _spawnLocation = new Vector3(9.2f, Random.Range(-3.4f, 6.2f));
                _spawnRotation = new Vector3(0, 0, -90);
            }
            else if (_enemyType == 3 && _enemyTypesToSpawn[_enemyType] == 1)
            {

                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 5.64f, 0);
                _spawnRotation = new Vector3(0, 0, 0);

            }
            else if (_enemyType==4 && _enemyTypesToSpawn[_enemyType] == 1)
            {
                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);
                _spawnRotation = new Vector3(0, 0, 0);
            }
            else if (_enemyType == 5 && _enemyTypesToSpawn[_enemyType] == 1)
            {
                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);
                _spawnRotation = new Vector3(0, 0, 0);
            }
            else
            {
                goto chooseAgain;
            }

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
        if (_enemiesSpawned < _enemiesToSpawn)
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
            SpawnEnemyPreperation();
        }

    }
    void CheckDeathCount()
    {
        if (_enemiesSlain >= _enemiesToSpawn)
        {

            _wave++;
            _enemiesSlain = 0;
            _enemiesSpawned = 0;
            WaveStart(_wave);
        }
        else
        {
            _uiManager.WaveDisplay(_wave, _enemiesToSpawn, _enemiesSlain);
        }
    }
    public void EnemySlain()
    {
        _enemiesSlain++;
        CheckDeathCount();
    }

    IEnumerator SpawnPowerup()
    {
        while (!playerDied)
        {
            yield return new WaitForSeconds(Random.Range(4, 7) + 1);
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
