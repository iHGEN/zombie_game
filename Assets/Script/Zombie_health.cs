using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Zombie_health : MonoBehaviour
{
    public Zombie_wave zombie_Wave;
    public int id = 0;
    public float Helath;
    public bool _is_die;
    readonly int die = Animator.StringToHash("die");
    // Update is called once per frame
    IEnumerator hide_zombie(int num)
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        zombie_Wave.zombei_animator[num].runtimeAnimatorController = null;
    }
    void Update()
    {
        if(Helath <= 0)
        {
            if (!_is_die)
            {
                Helath = 0;
                _is_die = true;
                if (!zombie_Wave.Zombie_Agents[id].isStopped)
                {
                    zombie_Wave.Zombie_Agents[id].isStopped = true;
                    zombie_Wave._killcount++;
                }
                zombie_Wave.zombei_animator[id].SetTrigger(die);
                StartCoroutine(hide_zombie(id));
            }
        }
    }
}
