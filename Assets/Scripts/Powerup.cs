using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _powerUpSpeed = 3f;
    private Player _player;

    [SerializeField]
    private int _powerUpType;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);
        if (transform.position.y < -5.8)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player = other.GetComponent<Player>();
            switch (_powerUpType)
            {
                case 0: //Triple Shot Active
                    _player.TripleShotActive();
                    break;
                case 1: //Speed Active
                    _player.SpeedActive();
                    break;
                case 2: //Shields Active
                    _player.ShieldActive();
                    break;
            }
            Destroy(this.gameObject);


        }
    }
}
