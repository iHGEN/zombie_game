using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_forword : MonoBehaviour
{
    public Weapon _weapon;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _weapon.raycastHit.point, 0.1f);
    }
}
