using System.Linq;
using UnityEngine;

public class bullet_forword : MonoBehaviour
{
    public Weapon _weapon;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _weapon.raycastHit.point, 0.1f);
    }
}
