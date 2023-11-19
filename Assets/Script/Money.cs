using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int _current_money;
    public int zombie_hit_money;
    private void Start()
    {
        text.text = _current_money.ToString();
    }
    public void add_money(int number)
    {
        _current_money += number;
        text.text = _current_money.ToString();
    }
    public void Take_money(int number)
    {
        _current_money -= number;
        text.text = _current_money.ToString();
    }
}
