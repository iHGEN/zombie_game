using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lucky_object : MonoBehaviour
{
    public Zombie_wave _zombie_Wave;
    public AudioClip[] audioClips;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    IEnumerator disable_all()
    {
        yield return new WaitForSeconds(30);
        _zombie_Wave._is_insta_kill_active = false;
    }
 
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "max":
                audioSource.PlayOneShot(audioClips[0], 0.7f);
                _zombie_Wave.max_ammo();
                Destroy(other.gameObject);
                break;
            case "insta-kill":
                audioSource.PlayOneShot(audioClips[1], 0.7f);
                _zombie_Wave._is_insta_kill_active = true;
                _zombie_Wave.insta_kill();;
                Destroy(other.gameObject);
                StartCoroutine(disable_all());
                break;
            case "cabom":
                audioSource.PlayOneShot(audioClips[2], 0.7f);
                _zombie_Wave.cabom();
                Destroy(other.gameObject);
                break;
        }
    }
}
