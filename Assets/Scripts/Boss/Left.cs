using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
         
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Boss _boss = gameObject.GetComponentInParent(typeof(Boss)) as Boss;
        if (other != null)
        {
            if (other.tag=="Projectile" && other.GetComponent<Laser>().WhoOwns()==0)
            {
                if (_boss.GetBossState() == 0)
                {
                    _boss.UpdateBossState(1);
                } else if (_boss.GetBossState() == 2) 
                {
                    _boss.UpdateBossState(3);
                }
                Destroy(other.gameObject);
            }
            if (other.tag == "Player")
            {
                other.GetComponent<Player>().Damage();
            }
        }
    }
}
