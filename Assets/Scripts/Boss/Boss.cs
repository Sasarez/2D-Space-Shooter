using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] Sprite[] _bossSprites;
    [SerializeField] int _bossState;
    bool _repairing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateBossState(int bossState)
    {
        _bossState= bossState;
    }

    public int GetBossState()
    {
        return _bossState;
    }
    // Update is called once per frame
    void Update()
    {
       
      
        UpdateBossSprite(_bossState);
        if (_bossState == 3 && _repairing == false)
        {
            _repairing = true;
            Invoke("RepairBoss", 3f);
        }
    }
    void RepairBoss()
    {
        _bossState= 0;
        _repairing = false;
    }
    void UpdateBossSprite(int state)
    {
        this.transform.GetComponent<SpriteRenderer>().sprite = _bossSprites[state];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other!=null)
        {
            if (other.tag == "Projectile" && other.GetComponent<Laser>().WhoOwns()==0)
            {
                if (_bossState == 3)
                {
                    Debug.Log("Gah you did damage");
                }
                else
                {
                    Debug.Log("You will have to do better I am immune to you");
                }
                Destroy(other.gameObject);
            }
        }
    }
}
