using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class scenes : MonoBehaviour
{
    // Start is called before the first frame update
    public void load_scenes(int num)
    {
        SceneManager.LoadScene(num);
    }
    public void exit()
    {
        Application.Quit();
    }
}
