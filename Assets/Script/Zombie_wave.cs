using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class Zombie_wave : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] lucky_obj;
    [SerializeField] _weapon_switch _Weapons;
    [SerializeField] Money _money;
    [SerializeField] Zombie_health zombie_Health;
    [SerializeField] GameObject[] zombie;
    [SerializeField] GameObject target;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int zombie_count;
    [SerializeField] int max_zombie_count;
    [SerializeField] AudioClip audioClip;
    [SerializeField] GameObject[] _Spawon_point;
    [SerializeField] GameObject[] _area_point;
    [SerializeField] float Zombie_hight;
    [SerializeField] float Zombie_radius;
    [SerializeField] RuntimeAnimatorController animatorController;
    [SerializeField] float stoppingDistance;
    [SerializeField] bool[] _is_area_enable;
    Zombie_health[] zombie_Healths;
    AudioSource audioSource;
    Animator[] zombei_animator;
    GameObject[] _Zombie_charcter;
    NavMeshAgent[] Zombie_Agents;
    public bool _is_insta_kill_active;
    int wave = 0;
    int zombie_per_round;
    int random_zombie_charcter;
    int random_zombie_spawon;
    float[] distance_from_point_spawon;
    int _start_point;
    int _end_point;
    bool give_him_luck;
    int number_fo_luck;
    readonly int attack = Animator.StringToHash("attack");
    private void Awake()
    {
        _is_area_enable = new bool[_area_point.Length];
        _is_area_enable[0] = true;
        zombie_Healths = new Zombie_health[max_zombie_count];
        distance_from_point_spawon = new float[_area_point.Length];
        gameObject.AddComponent<AudioSource>();
        audioSource = gameObject.GetComponent<AudioSource>();
        _Zombie_charcter = new GameObject[max_zombie_count];
        zombei_animator = new Animator[max_zombie_count];
        Zombie_Agents = new NavMeshAgent[max_zombie_count];
        create_zombie_wave(zombie_count, true);
        check_wave();
    }
    public void max_ammo()
    {
        for (int i = 0; i < _Weapons._Weapons_class.Length; i++)
        {
            _Weapons._Weapons_class[i]._Amintion = _Weapons._Weapons_class[i]._Maximum_ammo;
            _Weapons._Weapons_class[i]._Mag = _Weapons._Weapons_class[i]._max_Mag;
        }
    }
    public void insta_kill()
    {
        for(int i = 0; i < _Zombie_charcter.Length;i++)
        {
            if(_Zombie_charcter[i] != null)
            {
                zombie_Healths[i].Helath = 1;
            }
        }
    }
    public void cabom()
    {
        for (int i = 0; i < _Zombie_charcter.Length; i++)
        {
            if (_Zombie_charcter[i] != null)
            {
                zombie_Healths[i].Helath = 0;
            }
        }
        _money.add_money(1000);
    }
    // Start is called before the first frame update
    void create_zombie_wave(int number, bool first_wave)
    {
        if (number == 0 || first_wave)
        {
            //   audioSource.PlayOneShot(audioClip, 0.7f);
            wave++;
            zombie_Health.Helath += 5f;
            zombie_count = (first_wave) ? zombie_count : zombie_count = zombie_count + 2;
            zombie_per_round = zombie_count;
            get_nearest_point(true);
            for (int i = 0; i < zombie_count; i++)
            {
            random_point:
                random_zombie_spawon = Random.Range(_start_point, _end_point);
                if (random_zombie_spawon > _Spawon_point.Length - 1 || _Spawon_point[random_zombie_spawon] == null)
                {
                    goto random_point;
                }
                if (!give_him_luck)
                {
                    number_fo_luck = Random.Range(0, lucky_obj.Length);
                    give_him_luck = true;
                }
                random_zombie_charcter = Random.Range(0, zombie.Length);
                _Zombie_charcter[i] = Instantiate(zombie[random_zombie_charcter], _Spawon_point[random_zombie_spawon].transform.position, Quaternion.identity);
                _Zombie_charcter[i].AddComponent<Zombie_health>().Helath = zombie_Health.Helath;
                zombie_Healths[i] = _Zombie_charcter[i].GetComponent<Zombie_health>();
                zombei_animator[i] = _Zombie_charcter[i].GetComponent<Animator>();
                zombei_animator[i].runtimeAnimatorController = animatorController;
                _Zombie_charcter[i].GetComponent<CapsuleCollider>().isTrigger = true;
                _Zombie_charcter[i].AddComponent<NavMeshAgent>();
                Zombie_Agents[i] = _Zombie_charcter[i].GetComponent<NavMeshAgent>();
                Zombie_Agents[i].radius = Zombie_radius;
                Zombie_Agents[i].speed = Random.Range(0.5f, 2f);
                Zombie_Agents[i].height = Zombie_hight;
                Zombie_Agents[i].stoppingDistance = stoppingDistance;
                Zombie_Agents[i].obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
                if (_is_insta_kill_active)
                {
                    insta_kill();
                }
            }
        }
    }
    void get_nearest_point(bool nearby)
    {
        bool is_spaown_found = false;
        float small;
        int index = 0;
        for (int i = 0; i < _area_point.Length; i++)
        {
            if (_area_point[i] != null)
                distance_from_point_spawon[i] = Vector3.Distance(this.transform.position, _area_point[i].transform.position);
        }
        small = distance_from_point_spawon[0];
        for (int x = 0; x < distance_from_point_spawon.Length; x++)
        {
            if (nearby)
            {
                if (distance_from_point_spawon[x] < small && _is_area_enable[x])
                {
                    is_spaown_found = true;
                    small = distance_from_point_spawon[x];
                    index = x;
                }
            }
            else
            {
                if (distance_from_point_spawon[x] > small && _is_area_enable[x])
                {
                    is_spaown_found = true;
                    small = distance_from_point_spawon[x];
                    index = x;
                }
            }
        }
        if(!is_spaown_found)
        {
            for(int i = 0;i < _is_area_enable.Length; i++)
            {
                if(_is_area_enable[i])
                {
                    index = i;
                    break;
                }
            }
        }
        switch (index)
        {
            case 0:
                _start_point = 0;
                _end_point = 3;
                break;
            case 1:
                _start_point = 4;
                _end_point = 7;
                break;
            case 2:
                _start_point = 7;
                _end_point = 10;
                break;
            case 3:
                _start_point = 10;
                _end_point = 13;
                break;
        }
    }
    void check_wave()
    {
        create_zombie_wave(check_zombie_number(), false);
        text.text = $" Wave {wave} zombie Count {check_zombie_number()} of {zombie_per_round}";
    }
    public int check_zombie_number()
    {
        int result = 0;
        foreach (var x in _Zombie_charcter)
        {
            if (x != null)
            {
                result++;
            }
        }
        return result;
    }
    void go_to_target(int number)
    {
        for (int i = 0; i < number; i++)
        {
            if (_Zombie_charcter[i] != null)
            {
                var distance = Vector3.Distance(_Zombie_charcter[i].transform.position, target.transform.position);
                zombei_animator[i].SetBool(attack, distance <= Zombie_Agents[i].stoppingDistance && !zombie_Healths[i]._is_die);
                if(zombie_Healths[i]._is_die)
                {
                    Zombie_Agents[i].isStopped = true;
                    if (give_him_luck)
                    {
                        Instantiate(lucky_obj[number_fo_luck], _Zombie_charcter[i].transform.position, Quaternion.identity);
                        give_him_luck = false;
                    }
                }
                var lockd = Quaternion.LookRotation(new Vector3(Zombie_Agents[i].destination.x, 0, Zombie_Agents[i].destination.z));
                if (lockd.eulerAngles.x != 0 || lockd.eulerAngles.z != 0)
                {
                    Zombie_Agents[i].transform.Rotate(0, Zombie_Agents[i].destination.x, 0);
                }
                if (distance > Zombie_Agents[i].stoppingDistance && !zombie_Healths[i]._is_die)
                {
                    Zombie_Agents[i].destination = target.transform.position;
                }
            }
            else
            {
                check_wave();
            }
        }
    }
    public void Update()
    {
        go_to_target(zombie_count);
    }
}