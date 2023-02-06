using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    bool _dodging = false;
    Vector3 _direction;
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (transform.parent.GetComponent<Enemy>().GetEnemyType() == 6 && other.tag == "Projectile" && other.GetComponent<Laser>().WhoOwns() == 0)
        {
            if (!_dodging)
            {
                if (Random.Range(0, 2) == 0)
                {
                    _direction = Vector3.right;
                }
                else
                {
                    _direction = Vector3.left;
                }
            }           
            _dodging = true;
        }
    }

    private void EnemyDodge(Vector3 direction)
    {    
            transform.parent.Translate(direction * 3.5f * Time.deltaTime);
    }
    private void Update()
    {
        if (_dodging && transform.parent.GetComponent<Enemy>().IsEnemyDying()==false)
        {
            Invoke("DodgeEnd", .5f);
            EnemyDodge(_direction);
        }
    }

    private void DodgeEnd()
    {
        _dodging= false;
    }

}
