using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using System.Threading.Tasks;
public class SAVE_LOAD : MonoBehaviour
{
    [SerializeField] Money _money;
    BinaryFormatter formatter;
    public string _filename;
    public string _filepath;
    FileStream file;
    public Player player;
    public TextMeshProUGUI text;
    public void save()
    {
        if (!Directory.Exists(_filepath))
            Directory.CreateDirectory(_filepath);
        formatter = new BinaryFormatter();
        file = File.Create($"{_filepath } / {_filename} .bin");
        formatter.Serialize(file, player);
        file.Close();
    }
    public async void Delete_file()
    {
        string path = $"{_filepath } / {_filename} .bin";
        if (File.Exists(path))
        {
            File.Delete(path);
            if (text != null)
            {
                text.text = "File Deleted"; await Task.Delay(2500); text.text = "Delete your save";
            }
        }
        else
        {
            if (text != null)
            {
                text.text = "No file found"; await Task.Delay(2500); text.text = "Delete your save";
            }
        }
    }
    public void load()
    {
        if (!File.Exists($"{_filepath } / {_filename} .bin"))
            return;
        formatter = new BinaryFormatter();
        file = File.Open($"{_filepath } / {_filename} .bin", FileMode.Open);
        player = (Player)formatter.Deserialize(file);
        if (player._Money > 950 && player._Money != 0)
        {
            if (player.number_save == 0)
            {
                player._Money = 950;
            }
            _money._current_money = player._Money;
        }
        file.Close();
    }
}
