using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _weapon_switch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] _Weapons;
    public Weapon[] _Weapons_class;
    public int gun_index;
    void Start()
    {
        load_weapons();
        _Weapon_Switch(0);
    }
    void load_weapons()
    {
        _Weapons_class = new Weapon[_Weapons.Length];
        for(int i = 0; i < _Weapons.Length;i++)
        {
            _Weapons_class[i] = _Weapons[i].GetComponent<Weapon>();
        }
    }
    void _Weapon_Switch(int num)
    {
        num = num >= _Weapons.Length ? 0 : num < 0 ? _Weapons.Length - 1 : num;
        if (_Weapons[num] != null && num < _Weapons.Length)
        {
            gun_index = num;
            for (int i = 0; i < _Weapons.Length; i++)
            {
                _Weapons[i].SetActive(false);
                if (i == num)
                {
                    _Weapons_class[i].isreloading = false;
                    _Weapons_class[i].update_ammo_stats();
                    _Weapons[i].SetActive(true);
                    gun_index = num;
                }
            }
            _Weapons_class[gun_index].iKConstraint[0].data.target = _Weapons_class[gun_index].point_weapons[0];
            _Weapons_class[gun_index].iKConstraint[1].data.target = _Weapons_class[gun_index].point_weapons[1];
            _Weapons_class[gun_index].rig.weight = 0;
            _Weapons_class[gun_index].rig.weight = 1;
        }
    }
    void check_for_ammo()
    {
        if (_Weapons_class[gun_index]._Mag <= 0 && _Weapons_class[gun_index]._Amintion == 0 && Input.GetKey(KeyCode.Mouse0))
        {
            _Weapon_Switch(Random.Range(0, _Weapons.Length));
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            gun_index++;
            _Weapon_Switch(gun_index);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            gun_index--;
            _Weapon_Switch(gun_index);
        }
        check_for_ammo();
    }
}
