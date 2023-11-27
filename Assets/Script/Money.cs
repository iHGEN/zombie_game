using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Game_handler game_Handler;
    public int _current_money;
    public int zombie_hit_money;
    private void Start()
    {
        text.text = $"$ { _current_money}";
    }
    public void Update_money()
    {
        text.text = $"$ { _current_money}";
    }
    public void add_money(int number)
    {
        _current_money += number;
        Update_money();
        if (_current_money > 100000)
        {
            game_Handler.Win();
        }
    }
    public void Take_money(int number)
    {
        _current_money -= number;
        Update_money();
    }
}
