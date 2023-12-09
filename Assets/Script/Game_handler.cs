using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class Game_handler : MonoBehaviour
{
    public AudioClip round_end;
    [SerializeField] scenes scenes;
    [SerializeField] GameObject[] hide;
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Zombie_wave zombie_Wave;
    [SerializeField] Money _money;
    [SerializeField] SAVE_LOAD _Save;
    [SerializeField] AudioClip win;
    AudioSource audioSource;
    private void Start()
    {
        _Save.load();
        _money.Update_money();
        audioSource = this.GetComponent<AudioSource>();
        if (_Save.player.number_save == 0)
        {
            _Save.player.number_save = 2;
        }
    }
    public async void Win()
    {
        audioSource.PlayOneShot(win, 0.7f);
        zombie_Wave.end_round();
        zombie_Wave.enabled = false;
        panel.SetActive(true);
        text.text = $" Round : {zombie_Wave.wave} | kills : {zombie_Wave._killcount} | Money : {zombie_Wave._money._current_money} \r\n\t Highest round reached : { _Save.player._highest_round}";
        for (int i = 0; i < hide.Length; i++)
        {
            hide[i].SetActive(false);
        }
        _Save.player._Money = 0;
        _Save.save();
        await Task.Delay(15 * 1000);
        scenes.load_scenes(0);
    }
    public async void game_over()
    {
        if(_Save.player.number_save != 0)
        {
            if (_money._current_money > 950)
            {
                _Save.player.number_save--;
                _Save.player._Money = _money._current_money;
            }
        }
        if (_Save.player._highest_round < zombie_Wave.wave)
        {
            _Save.player._highest_round = zombie_Wave.wave;
        }
        _Save.save();
        audioSource.PlayOneShot(round_end, 0.7f);
        zombie_Wave.end_round();
        zombie_Wave.enabled = false;
        panel.SetActive(true);
        text.text = $" Round : {zombie_Wave.wave} | kills : {zombie_Wave._killcount} | Money : {zombie_Wave._money._current_money} \r\n\t Highest round reached : { _Save.player._highest_round}";
        for (int i = 0; i < hide.Length; i++)
        {
            hide[i].SetActive(false);
        }
        await Task.Delay(15 * 1000);
        scenes.load_scenes(0);
    }
}
