using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Boss _boss = gameObject.GetComponentInParent(typeof(Boss)) as Boss;
        if (other != null)
        {
            if (other.tag == "Projectile" && other.GetComponent<Laser>().WhoOwns() == 0)
            {
                if (_boss.GetBossState() == 1)
                {
                    _boss.UpdateBossState(3);
                }
                else if (_boss.GetBossState() == 0)
                {
                    _boss.UpdateBossState(2);
                }
                Destroy(other.gameObject);
            }
        }
    }
}
