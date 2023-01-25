using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    Animator _enemyAnim;
    
    float _speed = 3.5f;
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.Log("The Player is NULL");
        _enemyAnim = GetComponent<Animator>();
        if (_enemyAnim == null) Debug.Log("The Animator is NULL");
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

            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            
            Destroy(gameObject, 2.5f);
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
            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed= 0;
           
            Destroy (gameObject, 2.5f);
        }
    }
}
