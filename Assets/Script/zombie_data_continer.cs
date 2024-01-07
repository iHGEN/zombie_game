using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Assets/Script/scriptableobject", menuName ="Zombie Data",order = 1)]
public class zombie_data_continer : ScriptableObject
{
    public readonly int die = Animator.StringToHash("die");
}
