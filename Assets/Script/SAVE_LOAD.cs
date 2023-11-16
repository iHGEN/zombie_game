using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SAVE_LOAD : MonoBehaviour
{
    BinaryFormatter formatter;
    public string _filename;
    public string _filepath;
    FileStream file;
    public Player player;
    public void save()
    {
        if (!Directory.Exists(_filepath))
            Directory.CreateDirectory(_filepath);
        formatter = new BinaryFormatter();
        file = File.Create($"{_filepath } / {_filename} .bin");
        formatter.Serialize(file, player);
        file.Close();
        print(Directory.GetCurrentDirectory().ToString());
    }
    public void load()
    {
        if (!File.Exists($"{_filepath } / {_filename} .bin"))
            return;
        formatter = new BinaryFormatter();
        file = File.Open($"{_filepath } / {_filename} .bin", FileMode.Open);
        player = (Player)formatter.Deserialize(file);
        print(player._id);
        print(player._player_name);
        print(player._life);
        file.Close();
    }
}
