using System.Linq;
using UnityEngine;

public class bullet_forword : MonoBehaviour
{
    public Weapon _weapon;
    public Zombie_wave zombie_Wave;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag  == "zm")
        {
            int zombie_id = zombie_Wave._zombie_id_number.First(x => x.Value == other.gameObject.GetInstanceID()).Key;
            if (!zombie_Wave.zombie_Healths[zombie_id]._is_die)
            {
                zombie_Wave.zombie_Healths[zombie_id].Helath -= _weapon.damge;
                _weapon._money.add_money(_weapon._money.zombie_hit_money);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _weapon.raycastHit.point, 0.1f);
    }
}
