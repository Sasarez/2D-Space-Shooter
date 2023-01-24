using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    float _speed = 3.5f;
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down *  _speed * Time.deltaTime);
        if (transform.position.y < -5.41) {
            transform.position = new Vector3(Random.Range(-8.5f,8.5f),8, 0) ;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player") 
        {
            
            Player player = other.transform.GetComponent<Player>();
            //Destroy (other.gameObject);
            if (player != null) {
                player.Damage();
            } else{
                Debug.LogError("PLAYER IS NULL!");
            }
           
            Destroy(gameObject);
        }
        if (other.tag == "Projectile")
        {
            

            if (_player != null)
            {
                _player.AddScore(10);
               
            }
            else
            {
                Debug.Log("_player is NULL");
            }
            Destroy (other.gameObject);
            Destroy (gameObject);
        }
    }
}
