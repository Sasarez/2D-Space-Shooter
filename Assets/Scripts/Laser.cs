using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private int _owner; //if _owner is 0 it's player, if 1 it's enemy
    private bool _isSpecial = false;
    // Start is called before the first frame update
    void Start()
    {

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
    // Update is called once per frame
    void Update()
    {
        if (_isSpecial)
        {

            GameObject target = GameObject.FindWithTag("Enemy");
            this.transform.GetComponent<SpriteRenderer>().color = Color.cyan;
            if (!target)
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }
            else
            {

                transform.up = target.transform.position - transform.position;
                this.transform.Translate(Vector3.up * (6f) * Time.deltaTime);
            }


        }
        else
        {
            Debug.Log("Normal Fire");
            if (_owner == 0)
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }
            else if (_owner == 1)
            {
                transform.Translate(Vector3.down * (_speed * 2) * Time.deltaTime);
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
