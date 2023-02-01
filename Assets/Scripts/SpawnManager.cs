using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private int _enemyType;
    [SerializeField] private Vector3 _spawnLocation;
    [SerializeField] private Vector3 _spawnRotation;
    [SerializeField] private int _wave;
    [SerializeField] private int _enemiesSlain;
    [SerializeField] private bool _waveActive = false;
    [SerializeField] private int _enemiesToSpawn;
    [SerializeField] private int _enemiesSpawned;
    [SerializeField] private int[] _enemyTypesToSpawn;
    private bool playerDied = false;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("SpawnEnemy");
        _wave++;
        WaveStart(_wave);
        StartCoroutine("SpawnPowerup");
        
    }
    void WaveStart(int wave)
    {
        switch (wave)
        {
            case int i when (i > 0 && i < 4):
                Debug.Log("Wave Starting NOW!");
                _enemyTypesToSpawn[0] = 1;
                break;
            case int i when (i > 4 && i < 8):
                _enemyTypesToSpawn[0] = 1;
                _enemyTypesToSpawn[1] = 1;
                break;
            case int i when (i > 8 && i < 12):
                _enemyTypesToSpawn[0] = 1;
                _enemyTypesToSpawn[1] = 1;
                _enemyTypesToSpawn[2] = 1;
                 break;
            case 13:
                Debug.Log("You Won!");
                break;
        }
       
    
        _waveActive = true;
        _enemiesToSpawn = wave * 1;
        Debug.Log("Spawning " + wave * 1 + " Enemies");
        foreach (int i in _enemyTypesToSpawn)
        {
            if (_enemyTypesToSpawn[i] == 1)
            {  
                StartCoroutine("SpawnEnemy");
            }
        }
        
    }
    IEnumerator SpawnEnemy()
    {
        while (!playerDied && _waveActive)
        {
           
            yield return new WaitForSeconds(Random.Range(2f,5f));
            chooseAgain:
            _enemyType = Random.Range(0, 3);

            if (_enemyType == 0 && _enemyTypesToSpawn[_enemyType]==1)
            {
                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);
                _spawnRotation = new Vector3(0, 0, 0);
            } else if (_enemyType == 1 && _enemyTypesToSpawn[_enemyType] == 1)
            {
                _spawnLocation = new Vector3(-9.2f, Random.Range(-3.4f, 6.2f));
                _spawnRotation = new Vector3(0, 0, 90);
            }
            else if (_enemyType == 2 && _enemyTypesToSpawn[_enemyType] == 1 )
            {
                _spawnLocation = new Vector3(9.2f, Random.Range(-3.4f, 6.2f));
                _spawnRotation = new Vector3(0, 0, -90);
            }
            else
            {
                Debug.Log("Choosing again");
                goto chooseAgain;
            }
            if (_enemiesSpawned < _enemiesToSpawn)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, _spawnLocation, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                newEnemy.transform.eulerAngles = _spawnRotation;
                newEnemy.transform.GetComponent<Enemy>().SetEnemyType(_enemyType);
                _enemiesSpawned++;
            }
            else
            {
                StopCoroutine("SpawnEnemy");
            }
            
                
            
        }

    }
    public void CheckDeathCount()
    {
        if (_enemiesSlain >= _enemiesToSpawn)
        {
            _waveActive = false;
            _wave++;
            _enemiesSlain = 0;
            _enemiesToSpawn = 0;
            _enemiesSpawned = 0;
            Debug.Log("Starting Wave # " + _wave);
            
            WaveStart(_wave);
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
            yield return new WaitForSeconds(Random.Range(7, 12) + 1);
            int _powerUpSelect = Random.Range(0, _powerups.Length);
            if (_powerUpSelect == 5)
            {
                Debug.Log("Special Selected!");
                int _secondaryRoll = Random.Range(0, 100);

                if (_secondaryRoll >= 40)
                {
                    Debug.Log("Rerolled cause secondary check failed! " + _secondaryRoll + "Was rolled");
                    _powerUpSelect = Random.Range(0, _powerups.Length);

                }
                else
                {
                    Debug.Log("Secondary Roll passed, the number was " + _secondaryRoll);
                }
            }
            Instantiate(_powerups[_powerUpSelect], new Vector3(Random.Range(-8.5f, 8.5f), 8, 0), Quaternion.identity);

        }
    }
    public void PlayerDied()
    {
        StopCoroutine("SpawnEnemy");
        playerDied = true;
    }
}
