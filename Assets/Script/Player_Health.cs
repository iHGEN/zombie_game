using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    [SerializeField] Slider slider;
    public float _player_health;
    public float _max_health;
    public float _Zombie_damage;
    public float _regenerate_health;
    public float _upgrade_health;
    Coroutine _Coroutine;
    void Start()
    {
        _player_health = _max_health;
        slider.maxValue = _max_health;
        slider.value = _player_health;
    }

    void take_damage(float num)
    {      
        _player_health -= num;
        slider.value = _player_health;
    }
    public void upgrade_health()
    {
        _max_health = _upgrade_health;
        add_health();
    }
    void add_health()
    {
        _player_health = _max_health;
        slider.value = _player_health;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "zm_hit")
        {
            return;
        }
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
        take_damage(_Zombie_damage);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "zm_hit")
            return;
        _Coroutine = StartCoroutine(regenerate());
    }
    IEnumerator regenerate()
    {
        yield return new WaitForSeconds(_regenerate_health);
        add_health();
    }
    // Update is called once per frame
}
