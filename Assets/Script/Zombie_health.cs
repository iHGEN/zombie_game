using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_health : MonoBehaviour
{
    public float Helath;
   public bool _is_die;
    // Update is called once per frame
    void Update()
    {
        if(Helath <= 0)
        {
            if(!_is_die)
            {
                _is_die = true;
                gameObject.GetComponent<Animator>().SetTrigger("die");
                Destroy(gameObject, 2f);
            }
        }
    }
}
