using UnityEngine;
using TMPro;

public class buy_things : MonoBehaviour
{
    [SerializeField] GameObject[] _items;
    [SerializeField] GameObject[] _money_cost;
    [SerializeField] GameObject[] _items_point;
    [SerializeField] TextMeshProUGUI[] _name_of_items;
    [SerializeField] AudioClip[] _description_audio;
    [SerializeField] bool[] _can_take;
}
