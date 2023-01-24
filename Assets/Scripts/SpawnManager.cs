using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    private bool playerDied = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnEnemy");
        StartCoroutine("SpawnPowerup");
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SpawnEnemy()
    {
        while (!playerDied)
        {
            yield return new WaitForSeconds(5f);
            GameObject newEnemy = Instantiate(_enemy, new Vector3(Random.Range(-8.5f, 8.5f), 8, 0), Quaternion.identity);
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
            // switch (_powerUpSelect)
            // {
            //     case 0:
            //         Instantiate(_tripleShotPowerup, new Vector3(Random.Range(-8.5f, 8.5f), 8, 0), Quaternion.identity);
            //         break;
            //     case 1:
            //         Instantiate(_speedPowerup, new Vector3(Random.Range(-8.5f, 8.5f), 8, 0), Quaternion.identity);
            //         break;
            // }



        }


    }
    public void PlayerDied()
    {
        StopCoroutine("SpawnEnemy");
        playerDied = true;
    }
}
