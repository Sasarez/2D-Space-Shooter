using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]private float _speed = 3.5f;
    [SerializeField] private int owner; //if owner is 0 it's player, if 1 it's enemy
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void EnemyOwned()
    {
        owner = 1;
    }
    public int WhoOwns()
    {
        return owner;
    }
    // Update is called once per frame
    void Update()
    {
        if (owner == 0)
        {    
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        } else if (owner == 1)
        {
            transform.Translate(Vector3.down * (_speed * 2) * Time.deltaTime);
        }
        
        if (transform.position.y > 8 || transform.position.y < -5) 
        {
            if (transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    
}
