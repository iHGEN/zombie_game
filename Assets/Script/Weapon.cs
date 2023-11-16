using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;
public class Weapon : MonoBehaviour
{

    public GameObject[] _Weapons;
    public GameObject _bullet;
    public GameObject[] _fire_point;
    [SerializeField] int[] _Maximum_ammo;
    [SerializeField] int[] _Mag;
    [SerializeField] int[] _Amintion;
    [SerializeField] float[] firerate;
    [SerializeField] TextMeshProUGUI text;
    public float[] time_to_reload;
    public int[] damge;
    public int[] range;
    private int _Current_bullet;
    public GameObject _Bullet;
    public AudioClip[] audioClips;
    public ParticleSystem[] ammo_seaprt;
    private bool[] isreloading;
    bool[] is_gun_upgrade;
    AudioSource audioSource;
    private float nexttimefire;
    [SerializeField] Camera _camera;
    [SerializeField] Rig rig;
    [SerializeField] TwoBoneIKConstraint[] iKConstraint;
    public RaycastHit raycastHit;
    public int gun_index = 0;
    void Start()
    {
        isreloading = new bool[_Weapons.Length];
        is_gun_upgrade = new bool[_Weapons.Length];
        audioSource = GetComponent<AudioSource>();
        _Weapon_Switch(0);
        text.text = $"{_Amintion[gun_index] } / {_Mag[gun_index]}";
    }
    void _Weapon_Switch(int num)
    {
        num = num > 0 && num < _Weapons.Length ? num : 0;
        gun_index = num;
        for(int i =0; i < _Weapons.Length;i++)
        {
            _Weapons[i].SetActive(false);
            if (i == num)
            {
                _Weapons[i].SetActive(true);
            }
        }
        for (int x = 0; x < _Weapons[num].transform.childCount; x++)
        {
            if (_Weapons[num].transform.GetChild(x).name.Contains("right"))
            {
                iKConstraint[0].data.target = _Weapons[num].transform.GetChild(x);
            }
            if (_Weapons[num].transform.GetChild(x).name.Contains("left"))
            {
                iKConstraint[1].data.target = _Weapons[num].transform.GetChild(x);
            }
        }
        rig.weight = 0;
        rig.weight = 1;
        text.text = $"{_Amintion[num] } / {_Mag[num]}";
    }
    void Upgrade_gun()
    {

    }
    IEnumerator reloading()
    {
            isreloading[gun_index] = true;
            ammo_seaprt[gun_index].Stop();
            if (_Amintion[gun_index] == 0 && _Mag[gun_index] > 0 || _Amintion[gun_index] < _Maximum_ammo[gun_index] && _Mag[gun_index] > 0)
            {
                rig.weight = 0;
                _Current_bullet = _Maximum_ammo[gun_index] - _Amintion[gun_index];
                _Mag[gun_index] -= _Current_bullet;
                _Amintion[gun_index] += _Current_bullet;
            }
            yield return new WaitForSeconds(time_to_reload[gun_index]);
            isreloading[gun_index] = false;
            rig.weight = 1;
            text.text = $"{_Amintion[gun_index] } / {_Mag[gun_index]}";
    }
    void _fire()
    {
        if (Input.GetKey(KeyCode.Mouse0) && nexttimefire > firerate[gun_index])
        {
            if (!ammo_seaprt[gun_index].isPlaying)
            ammo_seaprt[gun_index].Play();
            if (_Amintion[gun_index] > 0)
            {
                _Amintion[gun_index]--;
                text.text = $"{_Amintion[gun_index] } / {_Mag[gun_index]}";
            }
            if(Physics.Raycast(_camera.transform.position, _camera.transform.forward,out raycastHit, range[gun_index]))
            {
                GameObject bullet = Instantiate(_bullet, _fire_point[gun_index].transform.position, Quaternion.Euler(0, 45, 0));
                bullet.AddComponent<bullet_forword>()._weapon = this;
                bullte_forword(bullet, raycastHit.point);
                Destroy(bullet, 2f);
                if(raycastHit.transform.gameObject.tag == "zm" && raycastHit.transform.gameObject.GetComponent<Zombie_health>() != null)
                {
                    raycastHit.transform.gameObject.GetComponent<Zombie_health>().Helath -= damge[gun_index];
                }
            }
            nexttimefire = 0;
            audioSource.PlayOneShot(is_gun_upgrade[gun_index] ? audioClips[1] : audioClips[0] , 0.7f);
        }
        if(!Input.GetKey(KeyCode.Mouse0) && nexttimefire > firerate[gun_index])
        {
            ammo_seaprt[gun_index].Stop();
        }
    }
    void bullte_forword(GameObject gameObject,Vector3 endlocation)
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, endlocation, 0.1f);
    }
    void Update()
    {
        nexttimefire += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1)) { _Weapon_Switch(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { _Weapon_Switch(1); }
        if (_Mag[gun_index] == 0 && _Amintion[gun_index] == 0 && Input.GetKey(KeyCode.Mouse0))
        {
            _Weapon_Switch(Random.Range(0, _Weapons.Length));
        }
        if (isreloading[gun_index])
            return;
        if (Input.GetKeyDown(KeyCode.R) || _Amintion[gun_index] <= 0)
        {
            StartCoroutine(reloading());
            return;
        }
        _fire();
    }
}
