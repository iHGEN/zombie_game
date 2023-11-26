using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glitch_detector : MonoBehaviour
{
    [SerializeField] Zombie_wave zombie_Wave;
    [SerializeField] float[] distance;

    // Update is called once per frame
    void check_area()
    {
        for(int i = 0;i < zombie_Wave._area_point.Length;i ++)
        {
            if(Vector3.Distance(this.transform.position,zombie_Wave._area_point[i].transform.position) < distance[i] && !zombie_Wave._is_area_enable[i])
            {
                this.transform.position = zombie_Wave._area_point[0].transform.position;
                break;
            }
        }
    }
    void Update()
    {
        check_area();
        if(this.transform.position.y < -0.1 || this.transform.position.y > 5.5)
        {
            this.transform.position = zombie_Wave._area_point[0].transform.position;
        }
    }
}
