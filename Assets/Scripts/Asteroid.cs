using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    int _speed = 18;
    [SerializeField]
    GameObject _spawnManager;
    [SerializeField]
    GameObject _explosion;
    GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            explosion = Instantiate(_explosion, this.transform.position, Quaternion.identity);
            Destroy(explosion, 2.38f);
            Destroy(this.gameObject,.15f);
            _spawnManager.SetActive(true);
        }
    }
}
