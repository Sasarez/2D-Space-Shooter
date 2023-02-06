using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private int _owner; //if _owner is 0 it's player, if 1 it's enemy
    private bool _isSpecial = false;
    private bool _altFire = false;
    private Vector3 _direction;
    private Vector3 _playerLocation;
    private GameObject[] _targets;
    private GameObject _target;
    private float _targetDistance;
    private float _checkDistance;
    private Transform _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").transform;
    }

    public void Special()
    {
        _isSpecial = true;
    }
    public void EnemyOwned()
    {
        _owner = 1;
    }
    public int WhoOwns()
    {
        return _owner;
    }
    public void AlternateFire(Vector3 playerLocation)
    {
        _altFire = true;
        _playerLocation = playerLocation;
        _direction = (_playerLocation - transform.position).normalized;
       

    }
    // Update is called once per frame
    void Update()
    {
        if (_isSpecial)
        {

            _targets = GameObject.FindGameObjectsWithTag("Enemy");
            _targetDistance = 9000f;
            
            
                foreach (var target in _targets)
                {
                    if (Vector2.Distance(_player.position, target.transform.position) < _targetDistance)
                    {
                        _targetDistance = Vector2.Distance(_player.position, target.transform.position);
                        _target = target;
                    }
                }
            
        
            this.transform.GetComponent<SpriteRenderer>().color = Color.cyan;

            if (!_target)
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }
            else
            {

                transform.up = _target.transform.position - transform.position;
                this.transform.Translate(Vector3.up * (6f) * Time.deltaTime);
            }


        }
        else
        {
            
            if (_owner == 0)
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }
            else if (_owner == 1)
            {
                if (!_altFire)
                {
                    transform.Translate(Vector3.down * (_speed * 2) * Time.deltaTime);
                }
                else
                {
                    transform.up = _playerLocation - transform.position;
                    
                    transform.position += _direction * _speed * Time.deltaTime;
                 
                }

                
                
            }
        }


        if (transform.position.y > 8 || transform.position.y < -5 || transform.position.x > 9.2 || transform.position.x < -9.2)
        {
            if (transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

}
