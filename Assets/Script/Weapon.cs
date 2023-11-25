using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;
using System.Threading.Tasks;
using System.Linq;

public class Weapon : MonoBehaviour
{
    public Money _money;
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
    private void Awake()
    {
        _max_Mag = _Mag;
        _Amintion = _Maximum_ammo;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        text.text = $"{_Amintion } / {_Mag}";
    }
    public void update_ammo_stats()
    {
        text.text = $"{_Amintion } / {_Mag}";
    }
    void Upgrade_gun()
    {

    }
    async void reloading()
    {
        isreloading = true;
        audioSource.PlayOneShot(reloading_gun, 0.7f);
        ammo_seaprt.Stop();
        if (_Amintion == 0 && _Mag > 0 || _Amintion < _Maximum_ammo && _Mag > 0)
        {
            rig.weight = 0;
            _Current_bullet = _Maximum_ammo - _Amintion;
            if (_Mag < _Amintion)
            {
                _Amintion += _Mag;
                _Mag = 0;
            }
            else
            {
                _Mag -= _Current_bullet;
                _Amintion += _Current_bullet;
            }
        }
        await Task.Delay(time_to_reload * 1000);
            isreloading = false;
        rig.weight = 1;
        text.text = $"{_Amintion } / {_Mag}";
    }
    void _fire()
    {
        if (Input.GetKey(KeyCode.Mouse0) && nexttimefire > firerate && _Amintion > 0)
        {
            ammo_seaprt.Play();
            if (_Amintion > 0)
            {
                _Amintion--;
                text.text = $"{_Amintion } / {_Mag}";
            }
            if(Physics.Raycast(_camera.transform.position, _camera.transform.forward,out raycastHit, range))
            {
                GameObject bullet = Instantiate(_bullet, _fire_point.transform.position, _camera.transform.rotation);
                if (raycastHit.transform.gameObject.tag == "zm")
                {
                    int zombie_id = zombie_Wave._zombie_id_number.First(x => x.Value == raycastHit.transform.gameObject.GetInstanceID()).Key;
                    if (!zombie_Wave.zombie_Healths[zombie_id]._is_die)
                    {
                        zombie_Wave.zombie_Healths[zombie_id].Helath -= damge;
                        _money.add_money(_money.zombie_hit_money);
                    }
                }
                bullet.GetComponent<bullet_forword>()._weapon = this;
                bullet.GetComponent<bullet_forword>()._is_Sentry_Gun = false;
                bullet.GetComponent<BoxCollider>().isTrigger = true;
                bullte_forword(bullet, raycastHit.point);
                Destroy(bullet, 2f);
            }
            nexttimefire = 0;
            audioSource.PlayOneShot(is_gun_upgrade ? audioClips[1] : audioClips[0] , 0.7f);
        }
        if(!Input.GetKey(KeyCode.Mouse0) || _Amintion <= 0)
        {
            ammo_seaprt.Stop();
        }
    }
    void bullte_forword(GameObject gameObject,Vector3 endlocation)
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, endlocation, 0.1f);
    }
    void Update()
    {
        nexttimefire += Time.deltaTime;
        if (isreloading)
            return;
        if (Input.GetKeyDown(KeyCode.R) && _Mag > 0 && _Amintion != _Maximum_ammo || _Amintion <= 0 && _Mag > 0)
        {
            reloading();
            return;
        }
        _fire();
    }
}
