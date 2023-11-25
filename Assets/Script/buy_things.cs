using UnityEngine;
using TMPro;

public class buy_things : MonoBehaviour
{
    [SerializeField] Player_Health _player_Health;
    [SerializeField] Money _money;
    [SerializeField] _weapon_switch _Weapon_Switch;
    [SerializeField] Zombie_wave zombie_Wave;
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
    void check_dis()
    {
        for (int i = 0; i < _items_point.Length; i++)
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
                    _can_be_take[num] = false;
                    if (check_money(num))
                    {
                        _player_Health.upgrade_health();
                        audioSource.PlayOneShot(_description_audio[num], 0.7f);
                    }
                }
                break;

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
