using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject _explosionPrefab;
    bool _exploding = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", .8f);
    }

    // Update is called once per frame
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
        Destroy(_explosion, 1.5f);
        Destroy(gameObject, 1.5f);
        
    }
}
