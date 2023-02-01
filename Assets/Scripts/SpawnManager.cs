using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private int _enemyType;
    [SerializeField] private Vector3 _spawnLocation;
    [SerializeField] private Vector3 _spawnRotation;
    [SerializeField] private int _wave = 0;
    [SerializeField] private int _enemiesSlain;
    [SerializeField] private bool _waveActive = false;
    [SerializeField] private int _enemiesToSpawn;
    [SerializeField] private int _enemiesSpawned;
    private bool playerDied = false;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("SpawnEnemy");
        WaveStart(1);
        StartCoroutine("SpawnPowerup");
        
    }
    void WaveStart(int wave)
    {
        _waveActive = true;
        _enemiesToSpawn = wave * 5;
        StartCoroutine("SpawnEnemy");
    }
    IEnumerator SpawnEnemy()
    {
        while (!playerDied && _waveActive)
        {
           
            yield return new WaitForSeconds(Random.Range(2f,5f));
            _enemyType = Random.Range(0, 3);
            if (_enemyType == 0)
            {
                _spawnLocation = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);
                _spawnRotation = new Vector3(0, 0, 0);
            } else if (_enemyType == 1)
            {
                _spawnLocation = new Vector3(-9.2f, Random.Range(-3.4f, 6.2f));
                _spawnRotation = new Vector3(0, 0, 90);
            }
            else if (_enemyType == 2)
            {
                _spawnLocation = new Vector3(9.2f, Random.Range(-3.4f, 6.2f));
                _spawnRotation = new Vector3(0, 0, -90);
            }
            if (_enemiesSpawned < _enemiesToSpawn)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, _spawnLocation, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                newEnemy.transform.eulerAngles = _spawnRotation;
                newEnemy.transform.GetComponent<Enemy>().SetEnemyType(_enemyType);
                _enemiesSpawned++;
            } 
            
                
            
        }

    }
    public void EnemySlain()
    {
        _enemiesSlain++;
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
