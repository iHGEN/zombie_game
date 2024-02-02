using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using System.Threading.Tasks;

public class Zombie_wave : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] lucky_obj;
    [SerializeField] _weapon_switch _Weapons;
    [SerializeField] Zombie_health zombie_Health;
    [SerializeField] GameObject[] zombie;
    [SerializeField] GameObject target;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int zombie_count;
    [SerializeField] int max_zombie_count;
    [SerializeField] AudioClip new_round;
    [SerializeField] float Zombie_hight;
    [SerializeField] float Zombie_radius;
    [SerializeField] ObstacleAvoidanceType obstacleAvoidanceType;
    [SerializeField] RuntimeAnimatorController animatorController;
    [SerializeField] float stoppingDistance;
    [SerializeField] float _attack_distance;

    public bool[] _is_area_enable;
    public Money _money;
    public GameObject[] _Spawon_point;
    public GameObject[] _area_point;
    public Zombie_health[] zombie_Healths;
    public Animator[] zombei_animator;
    public GameObject[] _Zombie_charcter;
    public NavMeshAgent[] Zombie_Agents;
    public bool _is_insta_kill_active;
    public int _killcount = 0;
    public int wave = 0;
    public int _start_point;
    public int _end_point;
    public bool is_setup_finish = false;
    public Dictionary<int, int> _zombie_id_number = new();

    AudioSource audioSource;
    int zombie_per_round;
    int random_zombie_charcter;
    int random_zombie_spawon;
    float[] distance_from_point_spawon;
    bool give_him_luck;
    int number_fo_luck;
    int lucky_obj_round;
    bool is_check_wave_finish = false;
    readonly int attack = Animator.StringToHash("attack");
    private void Awake()
    {
        ZombeiSetup();
    }
    void ZombeiSetup()
    {
        lucky_obj_round = 2;
        _is_area_enable = new bool[_area_point.Length];
        _is_area_enable[0] = true;
        zombie_Healths = new Zombie_health[max_zombie_count];
        distance_from_point_spawon = new float[_area_point.Length];
        gameObject.AddComponent<AudioSource>();
        audioSource = gameObject.GetComponent<AudioSource>();
        _Zombie_charcter = new GameObject[max_zombie_count];
        zombei_animator = new Animator[max_zombie_count];
        Zombie_Agents = new NavMeshAgent[max_zombie_count];
        StartCoroutine(create_zombie_wave(zombie_count, true));
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void max_ammo()
    {
        for (int i = 0; i < _Weapons._Weapons_class.Length; i++)
        {
            _Weapons._Weapons_class[i]._Amintion = _Weapons._Weapons_class[i]._Maximum_ammo;
            _Weapons._Weapons_class[i]._Mag = _Weapons._Weapons_class[i]._max_Mag;
            _Weapons._Weapons_class[i].update_ammo_stats();
        }
        _Weapons._Weapons_class[_Weapons.gun_index].update_ammo_stats();
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
    public void end_round()
    {
        for (int i = 0; i < _Zombie_charcter.Length; i++)
        {
            if (_Zombie_charcter[i] != null)
            {
                Destroy(_Zombie_charcter[i].gameObject);
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
    void RoundSetup(bool IsFristRound,bool IsSetupFinish)
    {
        if (IsSetupFinish)
        {
            is_setup_finish = false;
            wave++;
            zombie_Health.Helath += 5f;
            audioSource.PlayOneShot(new_round, 0.7f);
            zombie_count = (IsFristRound) ? zombie_count : zombie_count = zombie_count + 2;
            give_him_luck = (lucky_obj_round == wave) ? true : false;
            zombie_per_round = zombie_count;
            get_nearest_point(true);
        }
        else
        {
            check_wave();
            is_setup_finish = true;
            is_check_wave_finish = false;
        }
    }
    // Start is called before the first frame update
    IEnumerator create_zombie_wave(int number, bool first_wave)
    {
        if (number == 0 || first_wave)
        {
            yield return new WaitForSeconds(2.5f);
            RoundSetup(first_wave,true);
            for (int i = 0; i < zombie_count; i++)
            {
                do
                {
                    random_zombie_spawon = Random.Range(_start_point, _end_point + 1);
                }
                while (random_zombie_spawon > _Spawon_point.Length || _Spawon_point[random_zombie_spawon] == null);
                if (_Zombie_charcter[i] == null)
                {
                    random_zombie_charcter = Random.Range(0, zombie.Length);
                    _Zombie_charcter[i] = Instantiate(zombie[random_zombie_charcter], _Spawon_point[random_zombie_spawon].transform.position, Quaternion.identity);
                    zombie_Healths[i] = _Zombie_charcter[i].GetComponent<Zombie_health>();
                    zombie_Healths[i].Helath = zombie_Health.Helath;
                    zombie_Healths[i].zombie_Wave = this;
                    zombie_Healths[i].id = i;
                    zombei_animator[i] = _Zombie_charcter[i].GetComponent<Animator>();
                    zombei_animator[i].runtimeAnimatorController = animatorController;
                    Zombie_Agents[i] = _Zombie_charcter[i].GetComponent<NavMeshAgent>();
                    Zombie_Agents[i].radius = Zombie_radius;
                    Zombie_Agents[i].speed = Random.Range(1f, 3.5f);
                    Zombie_Agents[i].height = Zombie_hight;
                    Zombie_Agents[i].stoppingDistance = stoppingDistance;
                    Zombie_Agents[i].obstacleAvoidanceType = obstacleAvoidanceType;
                    _zombie_id_number.Add(i, _Zombie_charcter[i].GetInstanceID());
                }
                if(_Zombie_charcter[i] != null)
                {
                    _Zombie_charcter[i].SetActive(true);
                    _Zombie_charcter[i].transform.position = _Spawon_point[random_zombie_spawon].transform.position;
                    zombei_animator[i].runtimeAnimatorController = animatorController;
                    zombie_Healths[i].Helath = zombie_Health.Helath;
                    zombie_Healths[i]._is_die = false;
                    Zombie_Agents[i].isStopped = false;
                }
                if (_is_insta_kill_active)
                {
                    insta_kill();
                }
            }
            RoundSetup(false, false);
        }
    }

    public void get_nearest_point(bool nearby)
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
           if(!nearby)
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
                _end_point = 2;
                break;
            case 1:
                _start_point = 3;
                _end_point = 5;
                break;
            case 2:
                _start_point = 6;
                _end_point = 9;
                break;
        }
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void check_wave()
    {
        if (check_zombie_number() == 0)
        {
            if (!is_check_wave_finish)
            {
                is_check_wave_finish = true;
                StartCoroutine(create_zombie_wave(check_zombie_number(), false));
            }
        }
        else
        {
            text.text = $" Wave {wave} zombie Count {check_zombie_number()} of {zombie_per_round}";
        }
    }
    public int check_zombie_number()
    {
        int result = 0;
        for (int i = 0; i < zombie_count; i++)
        {
            if (!zombie_Healths[i]._is_die)
            {
                result++;
            }
        }
        return result;
    }
    void SpawonLockyObject(int ZombieId)
    {
        lucky_obj_round += Random.Range(1, 5);
        Instantiate(lucky_obj[Random.Range(0, lucky_obj.Length)], new Vector3(_Zombie_charcter[ZombieId].transform.position.x, _Zombie_charcter[ZombieId].transform.position.y + 0.5f, _Zombie_charcter[ZombieId].transform.position.z), Quaternion.identity);
        give_him_luck = false;
    }
    void GoToPlayer(int number)
    {
        for (int i = 0; i < number; i++)
        {
            if (!zombie_Healths[i]._is_die)
            {
                var distance = Vector3.Distance(_Zombie_charcter[i].transform.position, target.transform.position);
                zombei_animator[i].SetBool(attack, distance <= _attack_distance && !zombie_Healths[i]._is_die);
                if (distance > _attack_distance && _Zombie_charcter[i].activeInHierarchy)
                {
                    Zombie_Agents[i].destination = target.transform.position;
                }
            }
            else
            {
                if (give_him_luck)
                {
                    SpawonLockyObject(i);
                }
                check_wave();
            }
        }
    }
    public void Update()
    {
        if (is_setup_finish)
        {
            GoToPlayer(zombie_count);
        }
    }
}