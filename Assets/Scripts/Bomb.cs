using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject _explosionPrefab;
    bool _exploding = false;
  
    void Start()
    {
        Invoke("Explode", .8f);
    }

  
    void Update()
    {
        if (!_exploding)
        {
        transform.Translate(Vector3.down * 2f * Time.deltaTime);

        }
    }

    void Explode()
    {
        _exploding= true;
        GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _explosion.GetComponent<CircleCollider2D>().enabled = true;
        _explosion.transform.localScale = new Vector3(.7f, .7f, 1);
        Destroy(_explosion, 1.5f);
        Destroy(gameObject, .5f);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().Damage();
        }
    }
}
