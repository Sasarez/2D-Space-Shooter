using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    private bool playerDied = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnEnemy");
        StartCoroutine("SpawnPowerup");
    }

    IEnumerator SpawnEnemy()
    {
        while (!playerDied)
        {
            yield return new WaitForSeconds(5f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8.5f, 8.5f), 8, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
    }

    IEnumerator SpawnPowerup()
    {
        while (!playerDied)
        {
            yield return new WaitForSeconds(Random.Range(3, 7) + 1);
            int _powerUpSelect = Random.Range(0, _powerups.Length);
            Instantiate(_powerups[_powerUpSelect], new Vector3(Random.Range(-8.5f, 8.5f), 8, 0), Quaternion.identity);
        }
    }
    public void PlayerDied()
    {
        StopCoroutine("SpawnEnemy");
        playerDied = true;
    }
}
