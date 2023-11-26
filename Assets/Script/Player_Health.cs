using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class Player_Health : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] scenes scenes;
    [SerializeField] GameObject[] hide;
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Zombie_wave zombie_Wave;
    public float _player_health;
    public float _max_health;
    public float _Zombie_damage;
    public float _regenerate_health;
    public float _upgrade_health;
    public AudioClip audioClip;
    public AudioClip round_end;
    Coroutine _Coroutine;
    AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        _player_health = _max_health;
        slider.maxValue = _max_health;
        slider.value = _player_health;
    }

    async void take_damage(float num)
    {
        _player_health -= num;
        slider.value = _player_health;
        audioSource.PlayOneShot(audioClip, 0.7f);
        if (_player_health <= 0)
        {
            audioSource.PlayOneShot(round_end, 0.7f);
            zombie_Wave.end_round();
            zombie_Wave.enabled = false;
            panel.SetActive(true);
            text.text = $" Round : {zombie_Wave.wave} | kills : {zombie_Wave._killcount} | Money : {zombie_Wave._money._current_money}";
            for (int i = 0; i < hide.Length; i++)
            {
                hide[i].SetActive(false);
            }
            await Task.Delay(15*1000);
            scenes.load_scenes(0);
        }
    }
    public void upgrade_health()
    {
        _max_health = _upgrade_health;
        slider.maxValue = _max_health;
        add_health();
    }
    void add_health()
    {
        _player_health = _max_health;
        slider.value = _player_health;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "zm_hit" || collision.transform.root.GetComponent<Zombie_health>()._is_die)
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
