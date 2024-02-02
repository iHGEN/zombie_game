using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Weapon" , menuName ="Create Weapon",order = 1)]
public class Weapons : ScriptableObject
{
    // Start is called before the first frame update
    public Money _money;
    public GameObject _HitMark;
    public GameObject weapon__to_color;
    public Material[] materials;
    public Zombie_wave zombie_Wave;
    public GameObject _bullet;
    public GameObject _fire_point;
    public int _Maximum_ammo;
    public int _max_Mag;
    public int _Mag;
    public int _Amintion;
    public float firerate;
    public TextMeshProUGUI text;
    public AudioClip reloading_gun;
    public int time_to_reload;
    public int damge;
    public int range;
    private int _Current_bullet;
    public AudioClip[] audioClips;
    public ParticleSystem ammo_seaprt;
    public bool isreloading;
    private bool is_gun_upgrade;
    AudioSource audioSource;
    private float nexttimefire;
    public Camera _camera;
    public Rig rig;
    public Transform[] point_weapons;
    public TwoBoneIKConstraint[] iKConstraint;
    public RaycastHit raycastHit;
    bool is_ammo_upgrade;
}
