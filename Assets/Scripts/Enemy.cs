using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Enemy : MonoBehaviour
{
    
    Animator _enemyAnim;
    
    AudioSource _audioSource;
    float _speed = 3.5f;
    private Player _player;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _audioLaser;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.Log("The Player is NULL");
        _enemyAnim = GetComponent<Animator>();
        if (_enemyAnim == null) Debug.Log("The Animator is NULL");
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) Debug.LogError("AudioSource on the Enemy is NULL");
        StartCoroutine("EnemyFire");
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
                _audioSource.Play();
            } else{
                Debug.LogError("PLAYER IS NULL!");
            }

            _enemyAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            StopCoroutine("EnemyFire");
            this.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 2.5f);
        }
        if (other.tag == "Projectile" && other.GetComponent<Laser>().WhoOwns() == 0)
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

            StopCoroutine("EnemyFire");
            _speed= 0;
            _audioSource.Play();
            this.GetComponent<BoxCollider2D>().enabled = false;
            Destroy (gameObject, 2.5f);
        }
        
    }
    private IEnumerator EnemyFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2, 4));
            GameObject laser = Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y -1.5f, 0), Quaternion.identity);
            laser.GetComponent<Laser>().EnemyOwned();
            AudioSource.PlayClipAtPoint(_audioLaser, new Vector3(0,1,-10),.4f);
        }
    }
}
