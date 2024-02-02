using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform campos;

    // Update is called once per frame
    void Update()
    {
        if(transform.position != campos.position)
        transform.position = campos.position;
    }
}
