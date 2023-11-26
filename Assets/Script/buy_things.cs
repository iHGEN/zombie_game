using UnityEngine;
using System.Collections;
using TMPro;

public class buy_things : MonoBehaviour
{
    [SerializeField] Player_Health _player_Health;
    [SerializeField] Money _money;
    [SerializeField] _weapon_switch _Weapon_Switch;
    [SerializeField] Zombie_wave zombie_Wave;
    [SerializeField] Sentry_Gun _sentry;
    [SerializeField] GameObject[] _items;
    [SerializeField] int[] _money_cost;
    [SerializeField] GameObject[] _items_point;
    [SerializeField] string[] item_name;
    [SerializeField] TextMeshProUGUI _name_of_items;
    [SerializeField] AudioClip[] _description_audio;
    [SerializeField] float _maxdis;
    [SerializeField] bool[] _can_be_take;
    [SerializeField] AudioClip deny;
    [SerializeField] AudioClip accept;
    AudioSource audioSource;
    int gumcount;
    bool[] _is_nearby;
    bool nearby;
    void take_gum()
    {
        if(gumcount <= 0) { return; }
        gumcount--;
        zombie_Wave.get_nearest_point(false);
        transform.position = zombie_Wave._Spawon_point[Random.Range(zombie_Wave._start_point, zombie_Wave._end_point)].transform.position;
        _items[0].GetComponent<TextMeshProUGUI>().text = $"Gum Count : {gumcount}";
    }
    void add_gum()
    {
        gumcount += 3;
        _items[0].GetComponent<TextMeshProUGUI>().text = $"Gum Count : {gumcount}";
    }
    private void Start()
    {
        _is_nearby = new bool[_items.Length];
        audioSource = GetComponent<AudioSource>();
    }
    IEnumerator sentry(int num)
    {
        _sentry._is_finsh = false;
        audioSource.PlayOneShot(_description_audio[num], 0.7f);
        yield return new WaitForSeconds(_sentry.sentry_time);
        _sentry._is_finsh = true;
    }
    void check_dis()
    {
        for (int i = 0; i < _items_point.Length; i++)
        {
            if (_items_point[i] != null)
            {
                nearby = false;
                var dis = Vector3.Distance(this.transform.position, _items_point[i].transform.position);
                if (dis < _maxdis)
                {
                    _is_nearby[i] = true;
                    nearby = true;
                    _name_of_items.gameObject.SetActive(nearby);
                    get_info(i);
                    if (Input.GetKeyDown(KeyCode.F)) { get_items(i); }
                    break;
                }
                else
                {
                    _is_nearby[i] = false;
                }
            }
            if (!nearby)
            {
                _name_of_items.gameObject.SetActive(nearby);
            }
        }
    }
    bool check_money(int num)
    {
        if (_money_cost[num] > _money._current_money)
        {
            audioSource.PlayOneShot(deny, 0.7f);
            return false;
        }
        _money.Take_money(_money_cost[num]);
        audioSource.PlayOneShot(accept, 0.7f);
        return true;
    }
    void get_items(int num)
    {
        switch (num)
        {
            case 0:
                if (_can_be_take[num])
                {
                    if (check_money(num))
                    {
                        add_gum();
                        audioSource.PlayOneShot(_description_audio[num], 0.7f);
                    }
                }
                break;
            case 1:
                if (_can_be_take[num])
                {
                    if (check_money(num))
                    {
                        _can_be_take[num] = false;
                        _player_Health.upgrade_health();
                        audioSource.PlayOneShot(_description_audio[num], 0.7f);
                    }
                }
                break;
            case 2:
                if (_can_be_take[num])
                {
                    if (check_money(num) && _sentry._is_finsh)
                    {
                            StartCoroutine(sentry(num));
                    }
                }
                break;
            case 3:
                door(num, 1);
                break;
            case 4:
                door(num, 2);
                break;
            case 5:
                door(num,2);
                break;
                case 6:
                ammo(num);
                    break;
            case 7:
                ammo(num);
                break;
            case 8:
                ammo(num);
                break;

        }
    }
    void upgrade_weapon(int number)
    {
        if (_can_be_take[number])
        {
            if (check_money(number))
            {
                _Weapon_Switch._Weapons_class[_Weapon_Switch.gun_index].Upgrade_gun();
                audioSource.PlayOneShot(_description_audio[number], 0.7f);
            }
        }
    }
    void door (int number,int areanumber)
    {
        if (_can_be_take[number])
        {
            if (check_money(number))
            {
                _can_be_take[number] = false;
                Destroy(_items_point[number]);
                zombie_Wave._is_area_enable[areanumber] = true;
                audioSource.PlayOneShot(_description_audio[number], 0.7f);
            }
        }
    }    
    void ammo(int number)
    {
        if (_can_be_take[number])
        {
            if (check_money(number))
            {
                zombie_Wave.max_ammo();
                audioSource.PlayOneShot(_description_audio[number], 0.7f);
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) { take_gum(); }
        check_dis();
    }
    void get_info(int num)
    {
        if (item_name[num] != null)
            _name_of_items.text = $"Press F To buy {item_name[num]} {_money_cost[num]}";     
    }
}
